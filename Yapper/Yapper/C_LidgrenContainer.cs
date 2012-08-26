using System;
using Lidgren.Network;

namespace Yapper.Client
{
    #region Delegates & EventArgs

    #region Custom EventArgs

    /// <summary>
    /// EventArgs for Lidgren message-received events that need to pass
    /// a string to the event handler.
    /// </summary>
    public class MessageEventArgs : EventArgs
    {
        public string text;

        public MessageEventArgs(string setText)
        { text = setText; }
    }

    /// <summary>
    /// EventArgs for message: status changed received event.
    /// Passes the new status to the event handler. 
    /// Note: Can use ToString() on the status to convert it to readable.
    /// </summary>
    public class StatusChangedEventArgs : EventArgs
    {
        public NetConnectionStatus status;
        public string reason;

        public StatusChangedEventArgs(NetConnectionStatus setStatus, string setReason)
        { 
            status = setStatus;
            reason = setReason;
        }
    }

    /// <summary>
    /// EventArgs for message: unhandled message type receieved event.
    /// Passes the message type and length in bytes to the event handler.
    /// </summary>
    public class UnhandledMessageEventArgs : EventArgs
    {
        public NetIncomingMessageType messageType;
        public int lengthBytes;

        public UnhandledMessageEventArgs(NetIncomingMessageType myMessageType, int myLengthBytes)
        {
            messageType = myMessageType;
            lengthBytes = myLengthBytes;
        }
    }

    #endregion Custom EventArgs

    #region Delegates

    //Define delegates for this class's message-received events.
    //Delegates that don't need to pass any additional info are marked with // after.
    public delegate void MessageStatusConnectedEventHandler(object sender, EventArgs e);         //
    public delegate void MessageStatusDisconnectedEventHandler(object sender, EventArgs e);      //
    public delegate void MessageStatusChangedEventHandler(object sender, StatusChangedEventArgs e);
    public delegate void MessageDebugEventHandler(object sender, MessageEventArgs e);
    public delegate void MessageErrorEventHandler(object sender, MessageEventArgs e);
    public delegate void MessageWarningEventHandler(object sender, MessageEventArgs e);
    public delegate void MessageVerboseDebugEventHandler(object sender, MessageEventArgs e); 
    public delegate void MessageConnectionApprovalEventHandler(object sender, EventArgs e);      //
    public delegate void MessageOtherEventHandler(object sender, UnhandledMessageEventArgs e);

    public delegate void CMessageChatEventHandler(object sender, MessageEventArgs e);
    public delegate void CMessageNickReqEventHandler(object sender, EventArgs e);               //

    #endregion Delegates

    #endregion Delegates & EventArgs

    /// <summary>
    /// A wrapper for a single instance of Lidgren's NetClient.
    /// Simplifies and exposes the most critical functions.
    /// Raises an event each time a new network message is received
    /// and passes its data along to all subscribers.
    /// </summary>
    public class C_LidgrenContainer
    {
        const float TIMEOUT_S = 10.000F;  //Timeout in seconds
        const float HEARTBEAT_S = 0.500F;   //Ping frequency in seconds

        #region Fields

        private NetClient myLidClient;

        #region Events Declaration

        public event MessageStatusConnectedEventHandler StatusConnectedReceived = delegate { };
        public event MessageStatusDisconnectedEventHandler StatusDisconnectedReceived = delegate { };
        public event MessageStatusChangedEventHandler StatusChangedReceived = delegate { };
        public event MessageDebugEventHandler DebugReceived = delegate { };
        public event MessageErrorEventHandler ErrorReceived = delegate { };
        public event MessageWarningEventHandler WarningReceived = delegate { };
        public event MessageVerboseDebugEventHandler VerboseDebugReceived = delegate { };
        public event MessageConnectionApprovalEventHandler ConnectionApprovalReceived = delegate { };
        public event MessageOtherEventHandler OtherReceived = delegate { };

        public event CMessageChatEventHandler ChatReceived = delegate { };
        public event CMessageNickReqEventHandler NickReqReceived = delegate { };

        #endregion Events Declaration

        #endregion Fields

        #region Properties

        public  NetPeerStatus CurrentNetPeerStatus
        {
            get
            {
                if (myLidClient != null) return myLidClient.Status;
                else return NetPeerStatus.NotRunning;
            }
        }

        public  NetConnectionStatus CurrentConnectionStatus
        {
            get
            {
                if (myLidClient != null) return myLidClient.ConnectionStatus;
                else return NetConnectionStatus.None;
            }
        }

        public float PingTimeFloat
        {
            get
            {
                if (myLidClient.ConnectionStatus == NetConnectionStatus.Connected)
                {
                    return myLidClient.ServerConnection.AverageRoundtripTime;
                }
                else return 0;
            }
        }

        public string PingTimeReadable
        {
            get
            {
                if (myLidClient.ConnectionStatus == NetConnectionStatus.Connected)
                {
                    return NetTime.ToReadable(myLidClient.ServerConnection.AverageRoundtripTime);
                }
                else return "";
            }
        }

        #endregion Properties

        #region Methods

        #region Initialization, Connection, Disconnection

        /// <summary>
        /// Initialize the Lidgren NetClient.
        /// </summary>
        public C_LidgrenContainer(ChatWindow setChatWindow)  //Need to know an instance of ChatWindow so we can pass commands along to it in HandleCustomMessages.
        {
            NetPeerConfiguration lidClientConfig = new NetPeerConfiguration("chat");

            lidClientConfig.PingInterval = HEARTBEAT_S;
            lidClientConfig.ConnectionTimeout = TIMEOUT_S;

            myLidClient = new NetClient(lidClientConfig);

            myLidClient.Start();
        }

        /// <summary>
        /// Connect to ServerIP:ServerPort as long as there isn't already an active connection.
        /// </summary>
        public  void Connect(string ServerIP, ushort ServerPort)
        {
            if (myLidClient.ConnectionStatus == NetConnectionStatus.None || myLidClient.ConnectionStatus == NetConnectionStatus.Disconnected) //is there already a connection?
            {
                NetOutgoingMessage hail = myLidClient.CreateMessage();
                hail.Write("Hello there!");

                myLidClient.Connect(ServerIP, ServerPort, hail);
            }
        }

        public  void Disconnect()
        {
            myLidClient.Disconnect("User decided to disconnect");
        }

        #endregion Initialization, Connection, Disconnection

        #region Handle Incoming Messages

        /// <summary>
        /// To be called in the program's main loop.
        /// Will check for any newly arrived messages and raise the appropriate event(s).
        /// Branch into HandleCustomMessages for user defined messages.
        /// </summary>
        public  void HandleIncomingMessage()   
        {
            string text;
            NetIncomingMessage im;
            while ((im = myLidClient.ReadMessage()) != null)
            {
                // handle incoming message
                switch (im.MessageType)
                {
                    case NetIncomingMessageType.DebugMessage:
                        text = im.ReadString();
                        DebugReceived(this, new MessageEventArgs(text));
                        break;

                    case NetIncomingMessageType.ErrorMessage:
                        text = im.ReadString();
                        ErrorReceived(this, new MessageEventArgs(text));
                        break;

                    case NetIncomingMessageType.WarningMessage:
                        text = im.ReadString();
                        WarningReceived(this, new MessageEventArgs(text));
                        break;

                    case NetIncomingMessageType.VerboseDebugMessage:
                        text = im.ReadString();
                        VerboseDebugReceived(this, new MessageEventArgs(text));
                        break;

                    case NetIncomingMessageType.ConnectionApproval:
                        ConnectionApprovalReceived(this, EventArgs.Empty);
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();
                        string reason = im.ReadString();
                        StatusChangedReceived(this, new StatusChangedEventArgs(status, reason));

                        if (status == NetConnectionStatus.Connected)        //On connect
                            StatusConnectedReceived(this, EventArgs.Empty);                              
                        else                                               //On disconnect                
                            StatusDisconnectedReceived(this, EventArgs.Empty);  
                    
                        break;

                    case NetIncomingMessageType.Data:
                        HandleCustomMessage(im);            //Branch off into function HandleCustomMessages for all the game-related messages.
                        break;

                    default:
                        OtherReceived(this, new UnhandledMessageEventArgs(im.MessageType, im.LengthBytes));
                        break;
                }
            }
        }

        public void HandleCustomMessage(NetIncomingMessage im)
        {
            CustomMessages incomingType = (CustomMessages)im.ReadByte();
            switch (incomingType)
            {
                case CustomMessages.CHAT:
                    string chat = im.ReadString();
                    ChatReceived(this, new MessageEventArgs(chat));
                    break;

                case CustomMessages.NICKREQ:
                    NickReqReceived(this, EventArgs.Empty);
                    break;
            }
        }

        #endregion Handle Incoming

        #region Handle Outgoing Messages

        public  NetOutgoingMessage NewOutgoing
        {
            get
            {
                NetOutgoingMessage om = myLidClient.CreateMessage();
                return om;
            }
        }

        public void SendString(string text)
        {
            NetOutgoingMessage om = myLidClient.CreateMessage();
            om.Write((byte)CustomMessages.CHAT);
            om.Write(text);
            myLidClient.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
            myLidClient.FlushSendQueue();
        }

        public void SendNickReq(string nick)
        {
            NetOutgoingMessage om = myLidClient.CreateMessage();
            om.Write((byte)CustomMessages.NICKREQ);
            om.Write(nick);
            myLidClient.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
            myLidClient.FlushSendQueue();
        }

        public void Send(NetOutgoingMessage om, NetDeliveryMethod deliveryMethod)
        {
            myLidClient.SendMessage(om, deliveryMethod);
            myLidClient.FlushSendQueue();
        }

        #endregion Handle Outgoing

        #endregion Methods

    }
}
