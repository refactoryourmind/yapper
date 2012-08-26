using System;
using Lidgren.Network;
using System.Collections.Generic;

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
/// EventArgs for custom chat messages. Passes the sender and the text.
/// </summary>
public class ChatEventArgs : EventArgs
{
    public string text;
    public NetConnection sender;

    public ChatEventArgs(string setText, NetConnection setSender)
    { 
        text = setText;
        sender = setSender;
    }
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
/// EventArgs for connection approval received event.
/// Passes the net connection that requested approval
/// to the event handler.
/// </summary>
public class ConnectionApprovedEventArgs : EventArgs
{
    public NetConnection connectionApproved;

    public ConnectionApprovedEventArgs(NetConnection setConnectionApproved)
    {
        connectionApproved = setConnectionApproved;
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
public delegate void MessageConnectionApprovalEventHandler(object sender, ConnectionApprovedEventArgs e);    
public delegate void MessageOtherEventHandler(object sender, UnhandledMessageEventArgs e);

public delegate void CMessageChatEventHandler(object sender, ChatEventArgs e);
public delegate void CMessageNickReqEventHandler(object sender, EventArgs e);               //

#endregion Delegates

#endregion Delegates & EventArgs

namespace Yapper.Server
{
    /// <summary>
    /// Encapsulates a single instance of NetServer, simplifying and exposing some of its
    /// most useful functions. Raises events when new messages are received.
    /// </summary>
    public class S_LidgrenContainer
    {
        const float TIMEOUT_S = 10.000F;  //Timeout in seconds
        const float HEARTBEAT_S = 0.500F;   //Ping frequency in seconds

        #region Fields

        private NetServer myLidServer;

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
                if (myLidServer != null) return myLidServer.Status;
                else return NetPeerStatus.NotRunning;
            }
        }

        #endregion Properties

        #region Methods

        #region Initialization, Connection, Disconnection

        /// <summary>
        /// Initialize the Lidgren NetServer.
        /// </summary>
        public S_LidgrenContainer(ServerWindow setServerWindow)  //Need to know an instance of ChatWindow so we can pass commands along to it in HandleCustomMessages.
        {
            NetPeerConfiguration lidServerConfig = new NetPeerConfiguration("chat");
            lidServerConfig.Port = 2323;
            lidServerConfig.PingInterval = HEARTBEAT_S;
            lidServerConfig.ConnectionTimeout = TIMEOUT_S;
            lidServerConfig.SetMessageTypeEnabled(NetIncomingMessageType.ConnectionApproval, true);

            myLidServer = new NetServer(lidServerConfig);
            myLidServer.Start();
        }

        #endregion Initialization, Connection, Disconnection

        #region Handle Incoming Messages

        /// <summary>
        /// To be called in the program's main loop.
        /// Will check for any newly arrived messages and raise the appropriate event(s).
        /// Branch into HandleCustomMessages for user defined messages.
        /// </summary>
        public void HandleIncomingMessage()
        {
            string text;
            NetIncomingMessage im;
            while ((im = myLidServer.ReadMessage()) != null)
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
                        im.SenderConnection.Approve();
                        ConnectionApprovalReceived(this, new ConnectionApprovedEventArgs(im.SenderConnection));
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();
                        string reason = im.ReadString();
                        StatusChangedReceived(this, new StatusChangedEventArgs(status, reason));

                        if (status == NetConnectionStatus.Connected)        //On connect
                            StatusConnectedReceived(this, EventArgs.Empty);
                        else if (status == NetConnectionStatus.Disconnected)   //On disconnect   
                        {                                                    
                            StatusDisconnectedReceived(this, EventArgs.Empty);
                            BroadcastChat("Server", im.SenderConnection.Nickname + " has left the chat.");
                        }
                        else  //Other disconnects
                        {
                            StatusDisconnectedReceived(this, EventArgs.Empty);
                        }
                        break;

                    case NetIncomingMessageType.Data:
                        HandleCustomMessage(im);            //Branch off into function HandleCustomMessages for all the game-related messages.
                        break;

                    default:
                        OtherReceived(this, new UnhandledMessageEventArgs(im.MessageType, im.LengthBytes));
                        //DispatchChatWindowMessage("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes");
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
                    ChatReceived(this, new ChatEventArgs(chat, im.SenderConnection));
                    BroadcastChat(im.SenderConnection.Nickname, chat);
                    break;

                case CustomMessages.NICKREQ:
                    NickReqReceived(this, EventArgs.Empty);
                    im.SenderConnection.Nickname = im.ReadString();
                    BroadcastChat("Server", im.SenderConnection.Nickname + " has joined the chat session.");
                    break;
            }
        }

        #endregion Handle Incoming

        #region Handle Outgoing Messages

        public  NetOutgoingMessage NewOutgoing
        {
            get
            {
                NetOutgoingMessage om = myLidServer.CreateMessage();
                return om;
            }
        }

         public  void BroadcastChat(string sendername, string text)
         {
            List<NetConnection> all = myLidServer.Connections; //Get a copy of the connection list
            if (all.Count > 0) //if there is anyone in the list
            {
                NetOutgoingMessage om = myLidServer.CreateMessage();
                om.Write((byte)CustomMessages.CHAT);
                om.Write(sendername + " said: " + text);  //Write their ID and message into an outgoing packet
                myLidServer.SendMessage(om, all, NetDeliveryMethod.ReliableOrdered, 0);
            }
         }  

        public void Send(NetOutgoingMessage om, NetConnection recipient, NetDeliveryMethod deliveryMethod)
        {
            myLidServer.SendMessage(om, recipient, deliveryMethod);
            myLidServer.FlushSendQueue();
        }

        #endregion Handle Outgoing

        #endregion Methods


    }
}
