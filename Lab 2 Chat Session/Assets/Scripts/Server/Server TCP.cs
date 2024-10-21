using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TMPro;
using System.Text;
using System.Collections.Generic;

public class ServerTCP : MonoBehaviour
{
    Socket socket;
    Thread mainThread = null;

    public struct User
    {
        public string name;
        public Socket socket;
    }

    #region NEW VARS
    public static ServerTCP Singleton;

    [SerializeField] ChatManager chatManager;  // Script to pop messages in the Server chat
    [SerializeField] TMP_InputField chatInput; // Text you want to send to the users

    private string userName;   // Server User name
    private string serverName; // Server name

    private List <User> users; // List of Users Connected to the Server
    #endregion

    void Awake()
    {
        Singleton = this;
    }


    void Start()
    {
        users = new List<User>();

        userName = "Default Name";
        serverName = "Default Server";

        if (!string.IsNullOrEmpty(ServerDataManager.instance.serverName))       // Check if the value is null
        {
            serverName = ServerDataManager.instance.serverName;  // Get the Server User name
        }

        if (!string.IsNullOrEmpty(ServerDataManager.instance.userName))         // Check if the value is null
        {
            userName = ServerDataManager.instance.userName;      // Get the Server name
        }
        //chatManager.SendMessageToTCPChat(userName);              // Print the User name as a safety check

        startServer();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))                         // When the button is pressed, start the message sendig procces
        {
            //SendLocalChatMessage(chatInput.text,userName);
            SendMessageToAllUsers(users, chatInput.text, userName + ": ");   // Start the message sending procces
            chatInput.text = "";                                      // Reset the input box text
        }
    }

    #region NEW FUNCIONS
    public void SendLocalChatMessage(string _message = "", string _fromWho = "")
    {
        if (string.IsNullOrEmpty(_message)) return;         // Check if the string have content
        string S = _fromWho + _message;                     // Format the message
        chatManager.SendMessageToTCPChat(_message);         // Send Message to the local Server chat
    }

    void SendMessageToAllUsers(List <User> users, string _message = "", string _fromWho = "")
    {
        if (string.IsNullOrEmpty(_message)) return;  // Check if the string have content
        string S = _fromWho + _message;       // Format the message
        chatManager.SendMessageToTCPChat(S);         // Send Message to the local Server chat

        byte[] data = Encoding.ASCII.GetBytes(S);  // Encoding the message

        for (int i = 0; i < users.Count; i++)     // Iterate the Users list in order to send them all the message
        {
            users[i].socket.Send(data);           // Send the message to the specific user          
        }
    }
    #endregion

    public void startServer()
    {
        SendLocalChatMessage("Starting " + serverName + " TCP Server...");

        int port = 9050;
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
        socket.Bind(ipep);
        socket.Listen(10);

        mainThread = new Thread(CheckNewConnections);
        mainThread.Start();
    }

    void CheckNewConnections()
    {
        while (true)
        {
            User newUser = new User();
            
            newUser.name = " ";

            newUser.socket = socket.Accept(); //accept the socket
            IPEndPoint clientep = (IPEndPoint)newUser.socket.RemoteEndPoint;
            SendLocalChatMessage("Connected with " + clientep.Address.ToString() + " at port " + clientep.Port.ToString());

            users.Add(newUser); //Añadir cada Usuario nuevo a una lista de usuarios

            Thread newConnection = new Thread(() => Receive(newUser));
            newConnection.Start();
        }
    }

    void Receive(User user)
    {
        byte[] data = new byte[1024];
        int recv = 0;

        while (true)
        {
            data = new byte[1024];
            recv = user.socket.Receive(data);
            if (recv == 0)
                break;
            else
            {
                SendMessageToAllUsers(users, Encoding.ASCII.GetString(data, 0, recv));
            }
        }

        Thread answer = new Thread(() => Send(user, "You have conected to: " + serverName + "Server"));
        answer.Start();
    }
    void Send(User user, string msj)
    {
        byte[] data = Encoding.ASCII.GetBytes(msj);
        user.socket.Send(data);
    }
}
