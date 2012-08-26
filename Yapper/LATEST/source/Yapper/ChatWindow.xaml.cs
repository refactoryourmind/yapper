using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading.Tasks;
using System.Threading;

namespace Yapper.Client
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        #region Fields

        private static bool isRunning;
        private C_LidgrenContainer lidClient;

        #endregion Fields

        #region Methods

        public ChatWindow()
        {
            InitializeComponent();
            lidClient = new C_LidgrenContainer(this);

            lidClient.StatusChangedReceived += HandleStatusChanged;
            lidClient.StatusConnectedReceived += HandleConnected;
            lidClient.StatusDisconnectedReceived += HandleDisconnected;
            lidClient.DebugReceived += HandleDebug;
            lidClient.VerboseDebugReceived += HandleVerboseDebug;
            lidClient.ConnectionApprovalReceived += HandleConnectionApproval;
            lidClient.WarningReceived += HandleWarning;

            lidClient.ChatReceived += HandleChat;
            lidClient.NickReqReceived += HandleNickReq;

            //'Window Loaded' event will trigger soon after.
        }

        #region Connected/Disconnected

        public void OnConnected()
        {
            sendBox.IsEnabled = true;
            sendBtn.IsEnabled = true;
            ipBox.IsEnabled = false;
            portBox.IsEnabled = false;
            connectBtn.Content = "Disconnect";
        }

        public void OnDisconnected()
        {
            sendBox.IsEnabled = false;
            sendBtn.IsEnabled = false;
            ipBox.IsEnabled = true;
            portBox.IsEnabled = true;
            connectBtn.Content = "Connect";
        }

        #endregion Connected/Disconnected

        private void DispatchChatWindowMessage(string newMessage)
        {
            messageBox.Dispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                new Action(
                    delegate()
                    {
                        messageBox.AppendText(newMessage);
                        messageBox.AppendText("\n");
                        messageBox.ScrollToLine(messageBox.LineCount - 1);
                    }
             ));
        }

        private void RequestNickname()
        {
            string newNick = Microsoft.VisualBasic.Interaction.InputBox("Please enter a nickname.", "Nick Entry");
            lidClient.SendNickReq(newNick);
        }

        private void SendSendBoxContents()
        {
            if (sendBox.Text != "")
            {
                lidClient.SendString(sendBox.Text);
                sendBox.Clear();
            }
        }

        #region Events

        #region Window Loaded

        private void chatWindow_Loaded(object sender, RoutedEventArgs e)
        {
            isRunning = true;

            //Define incoming message handler loop thread.
            Action<object> handleIncoming = (object obj) =>
            {
                while (isRunning)
                {
                    lidClient.HandleIncomingMessage();
                }
            };

            //Define ping display update loop thread.
            Action<object> updatePing = (object obj) =>
            {
                while (isRunning)
                {
                    Dispatcher.Invoke(
                              System.Windows.Threading.DispatcherPriority.Normal,
                              new Action(
                                  delegate()
                                  {
                                      pingBlk.Text = lidClient.PingTimeReadable;
                                  }
                           ));

                    Thread.Sleep(100);
                }
            };

            //Start both loop threads.
            Task t1 = Task.Factory.StartNew(handleIncoming, "stopHandleIncoming");
            Task t2 = Task.Factory.StartNew(updatePing, "stopUpdatePing");
        }

        #endregion Window Loaded

        #region Buttons

        private void connectBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((string)connectBtn.Content == "Connect")
            {
                ushort port;
                bool portValid;
                portValid = UInt16.TryParse(portBox.Text, out port);
                if (portValid == false)
                {
                    MessageBox.Show("The Port field should consist of an integer between 0 and 65535.", "Field Error");
                    portBox.Text = "2323";
                    return;
                }
                lidClient.Connect(ipBox.Text, port);
                DispatchChatWindowMessage("Connecting to server...");
            }

            else if ((string)connectBtn.Content == "Disconnect")
            {
                lidClient.Disconnect();
            }
        }

        private void sendBtn_Click(object sender, RoutedEventArgs e)
        {
            SendSendBoxContents();
        }

        #endregion Buttons

        #region Send Box

        private void sendBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendSendBoxContents();
            }
        }

        #endregion Send Box

        #region Message Handling

        private void HandleConnectionApproval(object sender, EventArgs e)
        {
            DispatchChatWindowMessage("Connection approval");
        }

        private void HandleStatusChanged(object sender, StatusChangedEventArgs e)
        {
            DispatchChatWindowMessage("New status: " + e.status.ToString() + "  Reason: " + e.reason);

            Dispatcher.Invoke(
                              System.Windows.Threading.DispatcherPriority.Normal,
                              new Action(
                                  delegate()
                                  {
                                      statusBlk.Text = e.status.ToString();
                                  }
                           ));
        }

        private void HandleConnected(object sender, EventArgs e)
        {
            Dispatcher.Invoke(
            System.Windows.Threading.DispatcherPriority.Normal,
                new Action(
                    delegate()
                    {
                        OnConnected();
                    }));

            RequestNickname();
        }

        private void HandleDisconnected(object sender, EventArgs e)
        {
            Dispatcher.Invoke(
            System.Windows.Threading.DispatcherPriority.Normal,
                new Action(
                    delegate()
                    {
                        OnDisconnected();
                    }));
        }

        private void HandleWarning(object sender, MessageEventArgs e)
        {
            DispatchChatWindowMessage(e.text);
        }

        private void HandleDebug(object sender, MessageEventArgs e)
        {
            DispatchChatWindowMessage(e.text);
        }

        private void HandleVerboseDebug(object sender, MessageEventArgs e)
        {
            DispatchChatWindowMessage(e.text);
        }

        private void HandleChat(object sender, MessageEventArgs e)
        {
            DispatchChatWindowMessage(e.text);
        }

        private void HandleNickReq(object sender, EventArgs e)
        {
            RequestNickname();
        }

        #endregion Message Handling

        #endregion Events

        #endregion Methods

    }
}
