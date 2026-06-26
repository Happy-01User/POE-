using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace lemo3
{
    class NlpProcessor
    {

        public string Type;
        public string Title;
        public string Description;
        public DateTime Date;
    }

    public class NlpProcessor2
    {
        private static string connectionString =
        "server=127.0.0.1;port=3306;database=Reminder;uid=root;pwd=Paprika@14;";

        private static string activityConnectionString =
                      "Server=127.0.0.1;Database=LActivityLog;Port=3306;Uid=root;Pwd=Paprika@14;";

        // ================= TASK (CAN BE USED IN IF) =================
        public static bool NlpReminderSet(string sentence)
        {
            string lowerSentence = sentence.ToLower();

            if (!(lowerSentence.Contains("add task") ||
                  lowerSentence.Contains("create task") ||
                  lowerSentence.Contains("create a task") ||
                  lowerSentence.Contains("new task") ||
                  lowerSentence.Contains("make a task") ||
                  lowerSentence.Contains("set reminder") ||
                  lowerSentence.Contains("set a reminder") ||
                  lowerSentence.Contains("create reminder") ||
                  lowerSentence.Contains("add reminder") ||
                  lowerSentence.Contains("add a reminder") ||
                  lowerSentence.Contains("remind me to") ||
                  lowerSentence.Contains("remind me")))
            {
                return false;
            }

            NlpProcessor task = new NlpProcessor();


            // ---------------- DATE DETECTION ----------------
            if (sentence.ToLower().Contains("tomorrow"))
            {
                task.Date = DateTime.Today.AddDays(1);
                sentence = sentence.Replace("tomorrow", "");
            }
            else if (sentence.ToLower().Contains("in 2 days"))
            {
                task.Date = DateTime.Today.AddDays(2);
                sentence = sentence.Replace("in 2 days", "");
            }
            else if (sentence.ToLower().Contains("in 3 days"))
            {
                task.Date = DateTime.Today.AddDays(3);
                sentence = sentence.Replace("in 3 days", "");
            }
            else if (sentence.ToLower().Contains("in 4 days"))
            {
                task.Date = DateTime.Today.AddDays(4);
                sentence = sentence.Replace("in 4 days", "");
            }
            else if (sentence.ToLower().Contains("in 5 days"))
            {
                task.Date = DateTime.Today.AddDays(5);
                sentence = sentence.Replace("in 5 days", "");
            }
            else if (sentence.ToLower().Contains("in 6 days"))
            {
                task.Date = DateTime.Today.AddDays(6);
                sentence = sentence.Replace("in 6 days", "");
            }
            else if (sentence.ToLower().Contains("in a week"))
            {
                task.Date = DateTime.Today.AddDays(7);
                sentence = sentence.Replace("in a week", "");
            }

            // ---------------- CLEAN COMMANDS ----------------
            string[] commands =
            {
                "add task",
                "create task",
                "create a task",
                "new task",
                "make a task",
                "set reminder",
                "set a reminder",
                "create reminder",
                "add reminder",
                "add a reminder",
                "remind me to",
                "remind me"
            };

            foreach (string cmd in commands)
            {
                sentence = sentence.Replace(cmd, "");
            }

            task.Title = sentence.Trim();

            if (task.Title.Length > 0)
            {
                task.Title = char.ToUpper(task.Title[0]) + task.Title.Substring(1);
            }

            task.Type = "task";
            task.Description = "Created through NLP command";
            task.Date = task.Date;




            // ---------------- SAVE TO DATABASE ----------------
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"INSERT INTO Reminder
                                    (title, description, reminder)
                                    VALUES
                                    (@title, @description, @reminder)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@title", task.Title);
                    cmd.Parameters.AddWithValue("@description", task.Description);
                    cmd.Parameters.AddWithValue("@reminder", task.Date);

                    cmd.ExecuteNonQuery();

                    //CODE OF BLOCK TO RECORD THE REMINDER MADE THROUGH NLP
                       using (MySqlConnection con = new MySqlConnection(activityConnectionString))
                       {
                           con.Open();

                           string activityQuery = @"INSERT INTO LActivityLog
                               (ActivityType, Description)
                               VALUES
                               (@type, @description)";

                           MySqlCommand activityCmd = new MySqlCommand(activityQuery, con);

                           activityCmd.Parameters.AddWithValue("@type", "Reminder Added");

                           activityCmd.Parameters.AddWithValue(
                               "@description",
                               "Reminder '" + task.Title + "' was created through NLP.");

                           activityCmd.ExecuteNonQuery();
                       }
                   }

                   return true; // SUCCESS
               }
               catch
               {
                   return false; // FAIL
               }
                
        }

        // ================= QUIZ DETECTION =================
        public static bool ContainsQuiz(string sentence)
        {
            sentence = sentence.ToLower();

            return sentence.Contains("quiz") ||
                   sentence.Contains("play quiz") ||
                   sentence.Contains("start quiz") ||
                   sentence.Contains("test me");
        }
    }
}
