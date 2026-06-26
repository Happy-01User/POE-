using LEMO_PART3;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;

namespace lemo3
{
    public partial class Task_Assistant : Window
    {
        private string reminderConnection =
                  "Server=127.0.0.1;Database=Reminder;Port=3306;Uid=root;Pwd=Paprika@14;";

        private string activityConnection =
            "Server=127.0.0.1;Database=LActivityLog;Port=3306;Uid=root;Pwd=Paprika@14;";

        public Task_Assistant()
        {
            InitializeComponent();
            DisplayReminders();
        }

        //===========================
        // SAVE REMINDER
        //===========================

        private void btnSaveReminder_Click(object sender, RoutedEventArgs e)
        {
            string reminderTitle = txtTitle.Text.Trim();
            string reminderDescription = txtDescription.Text.Trim();

            if (reminderTitle == "")
            {
                MessageBox.Show(
                    "Please enter a reminder title.",
                    "Missing Information",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (reminderDescription == "")
            {
                MessageBox.Show(
                    "Please enter a reminder description.",
                    "Missing Information",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            SaveReminder(reminderTitle, reminderDescription);
        }

        private void SaveReminder(string title, string description)
        {
            try
            {
                using (MySqlConnection database =
                    new MySqlConnection(reminderConnection))
                {
                    database.Open();

                    string sql =
                        @"INSERT INTO Reminder
                        (title, description, reminder)
                        VALUES
                        (@title,@description,@date)";

                    MySqlCommand command =
                        new MySqlCommand(sql, database);

                    command.Parameters.AddWithValue("@title", title);
                    command.Parameters.AddWithValue("@description", description);

                    if (dpReminderDate.SelectedDate != null)
                    {
                        command.Parameters.AddWithValue(
                            "@date",
                            dpReminderDate.SelectedDate.Value.Date);
                    }
                    else
                    {
                        command.Parameters.AddWithValue(
                            "@date",
                            DBNull.Value);
                    }

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show(
                            "Reminder saved successfully!",
                            "Success",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);

                        RecordActivity(
                            "Reminder Created",
                            "Reminder '" + title + "' was added.");

                        txtTitle.Clear();
                        txtDescription.Clear();
                        dpReminderDate.SelectedDate = null;

                        DisplayReminders();
                    }
                    else
                    {
                        MessageBox.Show(
                            "Unable to save reminder.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Database Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        //===========================
        // LOAD GRID
        //===========================

        private void DisplayReminders()
        {
            try
            {
                using (MySqlConnection database =
                    new MySqlConnection(reminderConnection))
                {
                    database.Open();

                    string sql =
                        @"SELECT
                            Id,
                            title,
                            description,
                            reminder,
                            DateCreated
                          FROM Reminder";

                    MySqlDataAdapter adapter =
                        new MySqlDataAdapter(sql, database);

                    DataTable table = new DataTable();

                    adapter.Fill(table);

                    ReminderGrid.ItemsSource = table.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Database Error");
            }
        }
        //===========================
        // COMPLETE REMINDER
        //===========================

        private void btnComplete_Click(object sender, RoutedEventArgs e)
        {
            if (ReminderGrid.SelectedItem == null)
            {
                MessageBox.Show(
                    "Please select a reminder first.",
                    "Nothing Selected",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            DataRowView selectedRow =
                (DataRowView)ReminderGrid.SelectedItem;

            string reminderTitle =
                selectedRow["title"].ToString();

            RecordActivity(
                "Reminder Completed",
                "Reminder '" + reminderTitle +
                "' was marked as completed.");

            MessageBox.Show(
                "Reminder marked as completed.",
                "Completed",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        //===========================
        // DELETE REMINDER
        //===========================

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ReminderGrid.SelectedItem == null)
            {
                MessageBox.Show(
                    "Select a reminder before deleting.",
                    "Delete Reminder",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            DataRowView selectedRow =
                (DataRowView)ReminderGrid.SelectedItem;

            int reminderId =
                Convert.ToInt32(selectedRow["Id"]);

            string reminderTitle =
                selectedRow["title"].ToString();

            MessageBoxResult choice =
                MessageBox.Show(
                    "Delete the selected reminder?",
                    "Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

            if (choice != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                using (MySqlConnection database =
                    new MySqlConnection(reminderConnection))
                {
                    database.Open();

                    string sql =
                        "DELETE FROM Reminder WHERE Id=@Id";

                    MySqlCommand command =
                        new MySqlCommand(sql, database);

                    command.Parameters.AddWithValue(
                        "@Id",
                        reminderId);

                    command.ExecuteNonQuery();
                }

                RecordActivity(
                    "Reminder Deleted",
                    "Reminder '" + reminderTitle +
                    "' was removed.");

                MessageBox.Show(
                    "Reminder deleted successfully.",
                    "Deleted",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                DisplayReminders();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Database Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        //===========================
        // CLEAR FORM
        //===========================

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtTitle.Text = "";
            txtDescription.Text = "";
            dpReminderDate.SelectedDate = null;
        }

        //===========================
        // ACTIVITY LOGGER
        //===========================

        private void RecordActivity(string activity, string details)
        {
            try
            {
                using (MySqlConnection database =
                    new MySqlConnection(activityConnection))
                {
                    database.Open();

                    string sql =
                        @"INSERT INTO LActivityLog
                        (ActivityType, Description)
                        VALUES
                        (@activity,@details)";

                    MySqlCommand command =
                        new MySqlCommand(sql, database);

                    command.Parameters.AddWithValue(
                        "@activity",
                        activity);

                    command.Parameters.AddWithValue(
                        "@details",
                        details);

                    command.ExecuteNonQuery();
                }
            }
            catch
            {
                // Ignore logging failures
            }
        }       
        
        //===========================
        // NAVIGATION
        //===========================

        private void ChatBtn_Click(object sender, RoutedEventArgs e)
        {
            ChatDashboard chatWindow = new ChatDashboard();
            chatWindow.Show();

            Close();
        }

        private void QuizBtn_Click(object sender, RoutedEventArgs e)
        {
            securityQuiz quizWindow = new securityQuiz();
            quizWindow.Show();

            Close();
        }

        private void ActivityBtn_Click(object sender, RoutedEventArgs e)
        {
            Task_Assistant assistant = new Task_Assistant();
            assistant.Show();
            this.Close();
        }
    }
}