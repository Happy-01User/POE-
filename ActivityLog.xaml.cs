using LEMO_PART3;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;

namespace lemo3
{
    public partial class ActivityLog : Window
    {
        private string databaseConnection =
                          "Server=127.0.0.1;Database=LActivityLog;Port=3306;Uid=root;Pwd=Paprika@14;";

        public ActivityLog()
        {
            InitializeComponent();

            DisplayActivityHistory();
        }

        //===========================
        // LOAD ACTIVITY HISTORY
        //===========================

        private void DisplayActivityHistory()
        {
            try
            {
                using (MySqlConnection database =
                    new MySqlConnection(databaseConnection))
                {
                    database.Open();

                    string sql =
                        @"SELECT *
                          FROM LActivityLog
                          ORDER BY ActivityTime DESC";

                    MySqlDataAdapter activityAdapter =
                        new MySqlDataAdapter(sql, database);

                    DataTable activityTable =
                        new DataTable();

                    activityAdapter.Fill(activityTable);

                    ActivityGrid.ItemsSource =
                        activityTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load activity history.\n\n" +
                    ex.Message,
                    "Database Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        //===========================
        // NAVIGATION
        //===========================

        private void btnChat_Click(object sender, RoutedEventArgs e)
        {
            ChatDashboard dashboard = new ChatDashboard();
            dashboard.Show();

            Close();
        }

        private void btnTask_Click(object sender, RoutedEventArgs e)
        {
            Task_Assistant taskWindow = new Task_Assistant();
            taskWindow.Show();

            Close();
        }

        private void btnQuiz_Click(object sender, RoutedEventArgs e)
        {
            securityQuiz quizWindow = new securityQuiz();
            quizWindow.Show();

            Close();
        }
    }
}