using LEMO_PART3;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Windows;

namespace lemo3
{
    /// <summary>
    /// Interaction logic for securityQuiz.xaml
    /// </summary>
    public partial class securityQuiz : Window
    {
        private string connectionString =
            "Server=127.0.0.1;Database=LActivityLog;Port=3306;Uid=root;Pwd=Paprika@14;";

        private int currentQuestion = 0;
        private int finalScore = 0;

        private string[] questions =
        {
            "1. Which authentication method provides the best account security?",
            "2. What should you do before opening an email attachment from an unknown sender?",
            "3. What is the purpose of ransomware?",
            "4. Which password below is the strongest?",
            "5. Why should you enable Multi-Factor Authentication (MFA)?",
            "6. What does data encryption do?",
            "7. What is the safest way to use public Wi-Fi?",
            "8. Why should software updates be installed?",
            "9. What is social engineering?",
            "10. What should you do first if your online account is compromised?"
        };

        private string[,] options =
        {
            {
                "Username only",
                "Multi-Factor Authentication",
                "Security questions only",
                "PIN only"
            },

            {
                "Open it immediately",
                "Delete your antivirus",
                "Verify who sent it",
                "Forward it to friends"
            },

            {
                "Increase internet speed",
                "Lock files and demand payment",
                "Create stronger passwords",
                "Install updates"
            },

            {
                "password123",
                "John2004",
                "Qwerty",
                "G#7Lm!29Qa"
            },

            {
                "It makes your PC faster",
                "It provides an additional security layer",
                "It removes malware",
                "It deletes cookies"
            },

            {
                "Deletes files",
                "Makes data unreadable without a key",
                "Improves internet speed",
                "Creates backups"
            },

            {
                "Connect using a VPN",
                "Turn off your firewall",
                "Share files with everyone",
                "Log into banking sites without protection"
            },

            {
                "To fix bugs and security flaws",
                "To make software look different",
                "To slow down hackers",
                "To remove files"
            },

            {
                "Repairing hardware",
                "Tricking people into giving away information",
                "Formatting a hard drive",
                "Building a network"
            },

            {
                "Ignore it",
                "Change your password immediately",
                "Continue using it",
                "Delete your browser"
            }
        };

        private int[] answers =
        {
            1,
            2,
            1,
            3,
            1,
            1,
            0,
            0,
            1,
            1
        };

        private string[] explanations =
        {
            "Multi-Factor Authentication uses more than one verification method, making accounts much harder to access illegally.",

            "Always verify the sender before opening unexpected attachments because they may contain malware.",

            "Ransomware encrypts files and demands payment before restoring access.",

            "Strong passwords are long and contain uppercase letters, lowercase letters, numbers and symbols.",

            "MFA adds another security layer even if your password becomes known to someone else.",

            "Encryption converts readable information into unreadable data that only authorised users can access.",

            "Using a VPN encrypts your internet traffic and protects your information on public Wi-Fi.",

            "Updates fix vulnerabilities and protect devices against newly discovered cyber threats.",

            "Social engineering manipulates people into revealing confidential information.",

            "Changing your password immediately helps prevent further unauthorised access."
        };

        public securityQuiz()
        {
            InitializeComponent();
            LoadQuestion();
        }

        private void LoadQuestion()
        {
            txtQuestion.Text = questions[currentQuestion];

            rbA.Content = "A. " + options[currentQuestion, 0];
            rbB.Content = "B. " + options[currentQuestion, 1];
            rbC.Content = "C. " + options[currentQuestion, 2];
            rbD.Content = "D. " + options[currentQuestion, 3];

            rbA.IsChecked = false;
            rbB.IsChecked = false;
            rbC.IsChecked = false;
            rbD.IsChecked = false;
        }


        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            int userChoice = -1;

            if (rbA.IsChecked == true)
            {
                userChoice = 0;
            }

            if (rbB.IsChecked == true)
            {
                userChoice = 1;
            }

            if (rbC.IsChecked == true)
            {
                userChoice = 2;
            }

            if (rbD.IsChecked == true)
            {
                userChoice = 3;
            }

            if (userChoice == -1)
            {
                MessageBox.Show(
                    "Please choose an answer before moving to the next question.",
                    "Selection Required",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            bool correct = userChoice == answers[currentQuestion];

            if (correct)
            {
                finalScore++;

                MessageBox.Show(
                    "Correct!\n\n" +
                    explanations[currentQuestion],
                    "Result",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                string answerLetter = "";
                string answerText = "";

                switch (answers[currentQuestion])
                {
                    case 0:
                        answerLetter = "A";
                        break;

                    case 1:
                        answerLetter = "B";
                        break;

                    case 2:
                        answerLetter = "C";
                        break;

                    default:
                        answerLetter = "D";
                        break;
                }

                answerText = options[currentQuestion, answers[currentQuestion]];

                MessageBox.Show(
                    "Incorrect.\n\n" +
                    "Correct Answer:\n\n" +
                    answerLetter + ". " + answerText +
                    "\n\nExplanation:\n\n" +
                    explanations[currentQuestion],
                    "Result",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }

            currentQuestion++;

            if (currentQuestion < questions.Length)
            {
                LoadQuestion();
                return;
            }

            FinishQuiz();
        }


        private void FinishQuiz()
        {
            txtQuestion.Text = "Quiz Complete!";

            rbA.Visibility = Visibility.Collapsed;
            rbB.Visibility = Visibility.Collapsed;
            rbC.Visibility = Visibility.Collapsed;
            rbD.Visibility = Visibility.Collapsed;

            btnNext.Visibility = Visibility.Collapsed;

            txtScore.Text = "Final Score: " + finalScore + " / " + questions.Length;

            string resultMessage = "";

            if (finalScore == questions.Length)
            {
                resultMessage =
                    "Outstanding!\n\n" +
                    "Final Score: " + finalScore + "/" + questions.Length +
                    "\n\nYou demonstrated excellent cybersecurity knowledge.";
            }
            else if (finalScore >= 8)
            {
                resultMessage =
                    "Very Good!\n\n" +
                    "Final Score: " + finalScore + "/" + questions.Length +
                    "\n\nYou have strong cybersecurity awareness.";
            }
            else if (finalScore >= 5)
            {
                resultMessage =
                    "Good Attempt!\n\n" +
                    "Final Score: " + finalScore + "/" + questions.Length +
                    "\n\nKeep practising to strengthen your cybersecurity skills.";
            }
            else
            {
                resultMessage =
                    "Quiz Finished\n\n" +
                    "Final Score: " + finalScore + "/" + questions.Length +
                    "\n\nReview cybersecurity basics and try again.";
            }

            MessageBox.Show(
                resultMessage,
                "Quiz Complete",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            SaveQuizResult();
        }

        private void SaveQuizResult()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string sql =
                        @"INSERT INTO LActivityLog
                (ActivityType, Description)
                VALUES
                (@activity, @details)";

                    MySqlCommand command = new MySqlCommand(sql, connection);

                    command.Parameters.AddWithValue("@activity", "Security Quiz");

                    command.Parameters.AddWithValue(
                        "@details",
                        "User completed the Cybersecurity Quiz with a score of "
                        + finalScore + "/" + questions.Length + ".");

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to save quiz activity.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ChatBtn_Click(object sender, RoutedEventArgs e)
        {
            ChatDashboard dashboard = new ChatDashboard();
            dashboard.Show();
            Close();
        }

   

         private void TaskBtn_Click(object sender, RoutedEventArgs e)
         {
            Task_Assistant assistant = new Task_Assistant();
            assistant.Show();
             Close();
         }

         private void ActivityBtn_Click(object sender, RoutedEventArgs e)
         {
             ActivityLog history = new ActivityLog();
             history.Show();
             Close();
         }
    }
}