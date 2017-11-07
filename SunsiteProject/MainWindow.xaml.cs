using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SunsiteProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int selectedGroup = -1;
        private int selectedArticle = -1;
        private Client client;
        public MainWindow()
        {
            InitializeComponent();
            client = new Client();
            this.DataContext = client;
        }

        private void groupsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedGroup = groupsList.SelectedIndex;
            Console.WriteLine(groupsList.SelectedIndex);
        }

        private void chooseButton_Click(object sender, RoutedEventArgs e)
        {
            string [] message = client.GroupList[selectedGroup].Split();
            client.sendMessage("group " + message[0] + client.enter);
            client.readMessage();
            client.sendMessage("xover " + client.enter);
            client.readListArticles();
        }

        private void readArticleButton_Click(object sender, RoutedEventArgs e)
        {
            string[] message = client.ArticleList[selectedArticle].Split();
            client.sendMessage("article " + message[0] + client.enter);
            client.readArticle();
        }

        private void articlesView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedArticle = articlesView.SelectedIndex;
            Console.WriteLine(selectedArticle);
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            WaitWindow loadingWindow = new WaitWindow();
            //loadingWindow.Show();

            try
            {
                client.logIn();    
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception : " + ex.Message);
            }
            finally
            {
                chooseButton.IsEnabled = true;
                readArticleButton.IsEnabled = true;
                postArticle.IsEnabled = true;

                //loadingWindow.Close();

            }
            //client.logIn();
            /*
            client.readMessage();
            client.sendMessage("AUTHINFO user " + client.userName + Char.ConvertFromUtf32(13) + Char.ConvertFromUtf32(10));
            client.readMessage();
            client.sendMessage("AUTHINFO PASS " + client.password + Char.ConvertFromUtf32(13) + Char.ConvertFromUtf32(10));
            client.readMessage();
            client.sendMessage("list " + client.enter);
            client.readStreamLine();
            */
        }

        private void saveArticle_Click(object sender, RoutedEventArgs e)
        {
            PostArticle postArticle = new PostArticle(client);
            postArticle.Show();
            
        }
    }
}
