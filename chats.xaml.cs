using lemo3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LEMO_PART3
{
    public partial class ChatDashboard : Window
    {
        static Random random = new Random();

        string currentTopic = "";
        List<string> favouriteTopics = new List<string>();

        public ChatDashboard()
        {
            InitializeComponent();

            AddBotMessage(
                "Bot: Hello " + MainWindow.VisitorName +
                "! I'm your cybersecurity awareness bot.\n" +
                "Type 'help' for assistance or 'exit' to close the chat."
            );
        }

        static Dictionary<string, List<string>> botReplies =
        new Dictionary<string, List<string>>()
        {
            {
                "Hello",
                new List<string>()
                {
                    "Hello " + MainWindow.VisitorName + "! How Are You?",
                    "Hi " + MainWindow.VisitorName + "! Nice To See You.",
                    "Welcome Back " + MainWindow.VisitorName + "!"
                }
            },

            {
                "Help",
                new List<string>()
                {
                    "You Can Ask Me About Phishing, Malware, Passwords, And Safe Browsing.",
                    "Ask Me Cybersecurity Questions To Stay Safe Online.",
                    "I Can Help With Scams, Viruses, Passwords, And Online Safety."
                }
            },

            {
                "Phishing",
                new List<string>()
                {
                    "Phishing Is When Scammers Try To Steal Your Information.",
                    "Phishing Scams Pretend To Be Trusted Companies.",
                    "Phishing Messages Trick People Into Sharing Passwords."
                }
            },

            {
                "Password",
                new List<string>()
                {
                    "Passwords Protect Your Accounts And Information.",
                    "Passwords Help Keep Your Personal Data Secure.",
                    "Passwords Prevent Unauthorized Access To Accounts."
                }
            },

            {
                "Malware",
                new List<string>()
                {
                    "Malware Is Harmful Software.",
                    "Malware Can Damage Your Computer.",
                    "Malware Can Steal Information From Your Device."
                }
            },

            {
                "Exit",
                new List<string>()
                {
                    "Goodbye! Stay Safe Online!",
                    "See You Later!",
                    "Bye " + MainWindow.VisitorName + "!"
                }
            }
        };

        static Dictionary<string, List<string>> followUpReplies =
        new Dictionary<string, List<string>>()
        {
            {
                "Phishing",
                new List<string>()
                {
                    "Phishing Emails Often Create Panic To Trick Users.",
                    "Scammers May Pretend To Be Banks Or Trusted Companies.",
                    "Never Click Suspicious Links In Emails."
                }
            },

            {
                "Malware",
                new List<string>()
                {
                    "Malware Can Spread Through Unsafe Downloads.",
                    "Always Scan Files Before Opening Them.",
                    "Keep Your Antivirus Updated."
                }
            },

            {
                "Password",
                new List<string>()
                {
                    "Strong Passwords Should Be Long.",
                    "Avoid Using The Same Password Everywhere.",
                    "Password Managers Help Secure Passwords."
                }
            }
        };

        // ADDED (was missing in your original code)
        static Dictionary<string, string> memoryReplies =
        new Dictionary<string, string>()
        {
            { "Phishing", "Since you're interested in phishing, always verify emails before clicking links." },
            { "Malware", "Since you're interested in malware, keep your antivirus updated." },
            { "Password", "Since you're interested in passwords, avoid reusing them across accounts." }
        };

        // ADDED (sentiment system from second version)
        static Dictionary<string, List<string>> sentimentReplies =
        new Dictionary<string, List<string>>()
        {
            {
                "worried",
                new List<string>()
                {
                    "It's understandable to feel worried about online threats.",
                    "Cybersecurity threats can feel scary, but learning helps."
                }
            },
            {
                "frustrated",
                new List<string>()
                {
                    "Cybersecurity can feel frustrating, but you're improving.",
                    "It's okay to struggle while learning security concepts."
                }
            },
            {
                "curious",
                new List<string>()
                {
                    "Curiosity is great for learning cybersecurity.",
                    "Being curious helps you stay safe online."
                }
            }
        };

        // ADDED (sentiment tips)
        static Dictionary<string, List<string>> sentimentTips =
        new Dictionary<string, List<string>>()
        {
            {
                "worried",
                new List<string>()
                {
                    "Avoid clicking unknown links.",
                    "Always verify sender identity."
                }
            },
            {
                "frustrated",
                new List<string>()
                {
                    "Start with strong passwords.",
                    "Keep software updated."
                }
            },
            {
                "curious",
                new List<string>()
                {
                    "Learn phishing basics first.",
                    "Practice safe browsing habits."
                }
            }
        };

        public string FormatMessage()
        {
            string input = txtMessage.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                AddBotMessage("Bot: Please enter a message.");
                return "";
            }

            string formatted =
                char.ToUpper(input[0]) +
                input.Substring(1).ToLower();

            AddUserMessage(formatted);

            return formatted;
        }

        private void btnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            string formattedMessage = FormatMessage();

            if (formattedMessage.Contains("tell me more", StringComparison.OrdinalIgnoreCase)
               || formattedMessage.Contains("explain more", StringComparison.OrdinalIgnoreCase)
               || formattedMessage.Contains("another tip", StringComparison.OrdinalIgnoreCase))
            {
                FollowUpResponse();
                txtMessage.Clear();
                return;
            }
            else if (formattedMessage.Contains("interested", StringComparison.OrdinalIgnoreCase))
            {
                favouriteTopic(formattedMessage);
                txtMessage.Clear();
                return;
            }
            else if (formattedMessage.Contains("worried", StringComparison.OrdinalIgnoreCase)
                     || formattedMessage.Contains("frustrated", StringComparison.OrdinalIgnoreCase)
                     || formattedMessage.Contains("curious", StringComparison.OrdinalIgnoreCase))
            {
                sentimentReply(formattedMessage);
                txtMessage.Clear();
                return;
            }
            else if (formattedMessage.Contains("activity log", StringComparison.OrdinalIgnoreCase))
            {
                ActivityLog log = new ActivityLog();
                log.Show();
                this.Close();
            }
            else if (NlpProcessor2.ContainsQuiz(formattedMessage))
            {
                AddBotMessage("Bot: Opening quiz...");
                txtMessage.Clear();
                securityQuiz quiz = new securityQuiz();
                quiz.Show();
                this.Close();

                return;
            }
            else if (NlpProcessor2.NlpReminderSet(formattedMessage))
            {
                AddBotMessage("Your reminder is set. Go to Task assistant to view your reminders");
                txtMessage.Clear();
            }
          
            else
            {
                BotReply(formattedMessage);
                txtMessage.Clear();
                return;
            }
        }

        public void FollowUpResponse()
        {
            if (followUpReplies.ContainsKey(currentTopic))
            {
                int index = random.Next(followUpReplies[currentTopic].Count);
                AddBotMessage("Bot: " + followUpReplies[currentTopic][index]);
            }
            else
            {
                AddBotMessage("Bot: I don't have more information on that topic yet.");
            }
        }

        public void BotReply(string formattedMessage)
        {
            bool found = false;

            foreach (var item in botReplies.OrderByDescending(x => x.Key.Length))
            {
                if (formattedMessage.Contains(item.Key, StringComparison.OrdinalIgnoreCase))
                {
                    currentTopic = item.Key;

                    if (memoryReplies.ContainsKey(currentTopic)
                        && favouriteTopics.Contains(currentTopic))
                    {
                        AddBotMessage("Bot: " + memoryReplies[currentTopic]);
                    }
                    else
                    {
                        int i = random.Next(item.Value.Count);
                        AddBotMessage("Bot: " + item.Value[i]);
                    }

                    found = true;
                    break;
                }
            }

            if (!found)
            {
                AddBotMessage("Bot: I'm not sure I understand. Can you rephrase?");
            }
        }

        public void favouriteTopic(string formattedMessage)
        {
            foreach (var item in botReplies.OrderByDescending(x => x.Key.Length))
            {
                if (formattedMessage.Contains(item.Key, StringComparison.OrdinalIgnoreCase))
                {
                    currentTopic = item.Key;

                    if (!favouriteTopics.Contains(currentTopic))
                    {
                        favouriteTopics.Add(currentTopic);

                        AddBotMessage(
                            "Bot: I'll remember you're interested in " + currentTopic
                        );

                        return;
                    }
                }
            }

            AddBotMessage("Bot: I don't recognise that topic.");
        }

        public void sentimentReply(string formattedMessage)
        {
            foreach (var item in sentimentReplies)
            {
                if (formattedMessage.Contains(item.Key, StringComparison.OrdinalIgnoreCase))
                {
                    int r = random.Next(item.Value.Count);

                    if (sentimentTips.ContainsKey(item.Key))
                    {
                        int t = random.Next(sentimentTips[item.Key].Count);

                        AddBotMessage(
                            "Bot: " + item.Value[r] +
                            " Tip -> " + sentimentTips[item.Key][t]
                        );
                    }

                    return;
                }
            }
        }

        public void AddUserMessage(string text)
        {
            TextBlock message = new TextBlock
            {
                Text = text,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(5),
                Padding = new Thickness(10),
                Background = Brushes.LightGreen,
                HorizontalAlignment = HorizontalAlignment.Right,
                MaxWidth = 700
            };

            conversationContainer.Children.Add(message);
            conversationScroll.ScrollToEnd();
        }

        public void AddBotMessage(string text)
        {
            TextBlock message = new TextBlock
            {
                Text = text,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(5),
                Padding = new Thickness(10),
                Background = Brushes.LightYellow,
                HorizontalAlignment = HorizontalAlignment.Left,
                MaxWidth = 700
            };

            conversationContainer.Children.Add(message);
            conversationScroll.ScrollToEnd();
        }

        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            ActivityLog history = new ActivityLog();
            history.Show();
            this.Close();
        }

        private void btnQuiz_Click(object sender, RoutedEventArgs e)
        {
            securityQuiz quiz = new securityQuiz();
            quiz.Show();
            this.Close();
        }

        private void btnTasks_Click(object sender, RoutedEventArgs e)
        {
            Task_Assistant assistant = new Task_Assistant();
            assistant.Show();
            this.Close();
        }
    }
}