using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using NntpClientLib;
using System.IO;
using System.Collections.ObjectModel;

namespace SunsiteProject
{
    class Client
    {
        //Rfc977NntpClientWithExtensions test = new Rfc977NntpClientWithExtensions();

        private const int SERVPORT = 119;
        public string SERVHOST { get; set; } = "news.sunsite.dk";

        public string userName { get; set; } = "nikosxfx@gmail.com";
        public string userPassword { get; set; } = "9ed347";

        private string OK = "200";

        private TcpClient client = null;
        private StreamReader ns = null;
        private StreamWriter sw = null;
        private NetworkStream networkStream = null;

        private string responseMessage = "";
        public string enter = Char.ConvertFromUtf32(13) + Char.ConvertFromUtf32(10);
        private ObservableCollection<string> groupsList = new ObservableCollection<string>();
        private ObservableCollection<string> articleList = new ObservableCollection<string>();
        private ObservableCollection<string> articles = new ObservableCollection<string>();

        public Client()
        {
            //test.Connect(SERVHOST, SERVPORT);
            //connect();

        }

        public ObservableCollection<string> GroupList
        {
            get { return groupsList; }
            set { groupsList = value; }
        }

        public ObservableCollection<string> ArticleList
        {
            get { return articleList; }
            set { articleList = value; }
        }

        public ObservableCollection<string> Articles
        {
            get { return articles; }
            set { articles = value; }
        }

        public string connect()
        {
            string returnCode = "";
            try
            {
                client = new TcpClient(SERVHOST, SERVPORT);
                ns = new StreamReader(client.GetStream());
                returnCode = checkResponse(returnCode = readMessage());
                Console.WriteLine(returnCode);
                return returnCode;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return "";
        }

        public void logIn()
        {
            if (connect().Equals(OK))
            {
                //readMessage();
                sendMessage("AUTHINFO user " + userName + Char.ConvertFromUtf32(13) + Char.ConvertFromUtf32(10));
                readMessage();
                sendMessage("AUTHINFO PASS " + userPassword + Char.ConvertFromUtf32(13) + Char.ConvertFromUtf32(10));
                readMessage();
                sendMessage("list " + enter);
                readStreamLine();
            }
            else
            {
                MessageBox.Show("Try again");
            }
        }

        public void readStreamLine()
        {
            string line;
            while (!(line = ns.ReadLine()).StartsWith("."))
            {
                GroupList.Add(line);
            }
            Console.WriteLine(GroupList.Count);
        }

        public void readListArticles()
        {
            string line;
            while (!(line = ns.ReadLine()).StartsWith("."))
            {
                ArticleList.Add(line);
            }
            Console.WriteLine(ArticleList.Count);
        }

        public void readArticle()
        {
            string line;
            while (!(line = ns.ReadLine()).StartsWith("."))
            {
                Articles.Add(line);
            }
            Console.WriteLine(Articles.Count);
        }

        public void readStream()
        {
            networkStream = client.GetStream();

            if (networkStream.CanRead)
            {
                byte[] myReadBuffer = new byte[1024];
                StringBuilder myCompleteMessage = new StringBuilder();
                int numberOfBytesRead = 0;

                // Incoming message may be larger than the buffer size.
                do
                {
                    numberOfBytesRead = networkStream.Read(myReadBuffer, 0, myReadBuffer.Length);

                    myCompleteMessage.AppendFormat("{0}", Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead));

                }
                while (networkStream.DataAvailable);

                // Print out the received message to the console.
                Console.WriteLine("You received the following message : " +
                                             myCompleteMessage);
            }
            else
            {
                Console.WriteLine("Sorry.  You cannot read from this NetworkStream.");
            }
        }

        public string readMessage()
        {
            responseMessage = ns.ReadLine();
            Console.WriteLine("the message from server is: " + responseMessage);
            return responseMessage;
        }

        public void readMessageList()
        {
            responseMessage = ns.ReadLine();
            Console.WriteLine("the message from server is: " + responseMessage);
        }

        public void logIN()
        {
            sendMessage(writeMessage("200"));
            sendMessage(writeMessage("381"));
        }

        public void sendMessage(string message)
        {
            try
            {
                Console.WriteLine("Connected to server......");

                // Send message no 1 to the server 
                sw = new StreamWriter(client.GetStream());
                sw.Write(message);
                sw.Flush();
                Console.WriteLine("Message sent: " + message);
                Console.Read();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public string checkResponse(string message)
        {
            string[] returnCode = responseMessage.Split();

            message = returnCode[0];

            return message;
        }

        public string writeMessage(string code)
        {
            string message = "";
            switch (code)
            {
                case "200":
                    return message = "AUTHINFO user " + userName + Char.ConvertFromUtf32(13) + Char.ConvertFromUtf32(10);
                case "381":
                    return message = "AUTHINFO PASS " + userPassword + Char.ConvertFromUtf32(13) + Char.ConvertFromUtf32(10);
            }
            return message;
        }

        public void disconnect()
        {
            if (ns != null)
            {
                ns.Close();
            }
            if (client != null)
            {
                client.Close();
            }
        }
    }
}
