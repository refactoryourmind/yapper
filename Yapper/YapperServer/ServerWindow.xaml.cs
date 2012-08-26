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

namespace Yapper.Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ServerWindow : Window
    {
        private static bool isRunning;
        private S_LidgrenContainer lidServer;

        public ServerWindow()
        {
            InitializeComponent();
            lidServer = new S_LidgrenContainer(this);

            lidServer.StatusChangedReceived += HandleStatusChanged;
            lidServer.StatusConnectedReceived += HandleConnected;
            lidServer.StatusDisconnectedReceived += HandleDisconnected;
            lidServer.DebugReceived += HandleDebug;
            lidServer.VerboseDebugReceived += HandleVerboseDebug;
            lidServer.ConnectionApprovalReceived += HandleConnectionApproval;
            lidServer.WarningReceived += HandleWarning;

            lidServer.ChatReceived += HandleChat;
            lidServer.NickReqReceived += HandleNickReq;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            isRunning = true;

            Action<object> action = (object obj) =>
            {
                while (isRunning)
                    lidServer.HandleIncomingMessage();
            };

            Task t1 = Task.Factory.StartNew(action, "stop");
        }

        #region Message Handling

        private void HandleConnectionApproval(object sender, ConnectionApprovedEventArgs e)
        {
            DispatchChatWindowMessage("Approved of connection: " + e.connectionApproved.ToString());
        }

        private void HandleStatusChanged(object sender, StatusChangedEventArgs e)
        {
            DispatchChatWindowMessage("New status: " + e.status.ToString() + "  Reason: " + e.reason);
        }

        private void HandleConnected(object sender, EventArgs e)
        {      }

        private void HandleDisconnected(object sender, EventArgs e)
        {      }

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

        private void HandleChat(object sender, ChatEventArgs e)
        {
            DispatchChatWindowMessage(e.sender.Nickname + ": " + e.text);
        }

        private void HandleNickReq(object sender, EventArgs e)
        {        }

        #endregion Message Handling

        private void DispatchChatWindowMessage(string newMessage)
        {
            logBox.Dispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                new Action(
                    delegate()
                    {
                        logBox.AppendText(newMessage);
                        logBox.AppendText("\n");
                        logBox.ScrollToLine(logBox.LineCount - 1);
                    }
             ));
        }
    }
}
