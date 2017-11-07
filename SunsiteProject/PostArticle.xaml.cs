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
using System.Windows.Shapes;

namespace SunsiteProject
{
    /// <summary>
    /// Interaction logic for PostArticle.xaml
    /// </summary>
    public partial class PostArticle : Window

    {


        Client client;
        public PostArticle(Client client)
        {
            InitializeComponent();
            this.client = client;
            this.DataContext = client;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            client.sendMessage("Post " + client.enter);
            client.readMessage();
            client.uploadArticle(client.email, client.groupName, client.subject, client.articleText);
            MessageBox.Show(client.readMessage());

        }
    }
}
