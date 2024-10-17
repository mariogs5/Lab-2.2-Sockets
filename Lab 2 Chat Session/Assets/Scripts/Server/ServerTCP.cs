using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TMPro;
using System.Text;
using System.Collections.Generic;
using UnityEngine.UIElements;
using static ServerTCP;
using UnityEditor.VersionControl;

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
        userName = "Boty";
        serverName = "Server Boty";

        startServer();

        if (!string.IsNullOrEmpty(ServerDataManager.instance.serverName))       // Check if the value is null
        {
            serverName = ServerDataManager.instance.serverName;  // Get the Server User name
        }

        if (!string.IsNullOrEmpty(ServerDataManager.instance.userName))         // Check if the value is null
        {
            userName = ServerDataManager.instance.userName;      // Get the Server name
        }
        chatManager.SendMessageToTCPChat(userName);              // Print the User name as a safety check
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))                         // When the button is pressed, start the message sendig procces
        {
            SendLocalChatMessage(chatInput.text,userName);
            //SendMessageToAllUsers(users, chatInput.text, userName);   // Start the message sending procces
            chatInput.text = "";                                      // Reset the input box text
        }
    }

    #region NEW FUNCIONS
    public void SendLocalChatMessage(string _message = "", string _fromWho = "")
    {
        if (string.IsNullOrEmpty(_message)) return;  // Check if the string have content
        string S = _fromWho + ": " + _message;       // Format the message
        chatManager.SendMessageToTCPChat(S);         // Send Message to the local Server chat
    }

    void SendMessageToAllUsers(List <User> users, string _message, string _fromWho = null)
    {
        if (string.IsNullOrEmpty(_message)) return;  // Check if the string have content
        string S = _fromWho + ": " + _message;       // Format the message
        chatManager.SendMessageToTCPChat(S);         // Send Message to the local Server chat

        byte[] data = Encoding.ASCII.GetBytes(_message);  // Encoding the message

        for (int i = 0; i < users.Count; i++)     // Iterate the Users list in order to send them all the message
        {
            users[i].socket.Send(data);           // Send the message to the specific user          
        }
    }
    #endregion

    public void startServer()
    {
        SendLocalChatMessage("Starting " + serverName + " Server.");

        //TO DO 1
        //Create and bind the socket
        //Any IP that wants to connect to the port 9050 with TCP, will communicate with this socket
        //Don't forget to set the socket in listening mode

        int port = 9050;
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
        socket.Bind(ipep);
        socket.Listen(10);


        //TO DO 3
        //TIme to check for connections, start a thread using CheckNewConnections
        mainThread = new Thread(CheckNewConnections);
        mainThread.Start();
    }

    void CheckNewConnections()
    {
        while (true)
        {
            User newUser = new User();
            
            newUser.name = "";
            //TO DO 3
            //TCP makes it so easy to manage conections, so we are going
            //to put it to use
            //Accept any incoming clients and store them in this user.
            //When accepting, we can now store a copy of our server socket
            //who has established a communication between a
            //local endpoint (server) and the remote endpoint(client)
            //If you want to check their ports and adresses, you can acces
            //the socket's RemoteEndpoint and LocalEndPoint
            //try printing them on the console

            newUser.socket = socket.Accept(); //accept the socket
            IPEndPoint clientep = (IPEndPoint)newUser.socket.RemoteEndPoint;
            SendLocalChatMessage("Connected with " + clientep.Address.ToString() + " at port " + clientep.Port.ToString());

            users.Add(newUser); //Añadir cada Usuario nuevo a una lista de usuarios

            //TO DO 5
            //For every client, we call a new thread to receive their messages. 
            //Here we have to send our user as a parameter so we can use it's socket.
            Thread newConnection = new Thread(() => Receive(newUser));
            newConnection.Start();
        }
        //This users could be stored in the future on a list
        //in case you want to manage your connections

    }

    void Receive(User user)
    {
        //TO DO 5
        //Create an infinite loop to start receiving messages for this user
        //You'll have to use the socket function receive to be able to get them.
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
                SendLocalChatMessage(Encoding.ASCII.GetString(data, 0, recv));
            }

            //TO DO 6
            //We'll send a ping back every time a message is received
            //Start another thread to send a message, same parameters as this one.
            Thread answer = new Thread(() => Send(user, "You have conected to: " + serverName + "Server"));
            answer.Start();
        }
    }

    //TO DO 6
    //Now, we'll use this user socket to send a "ping".
    //Just call the socket's send function and encode the string.
    void Send(User user, string msj)
    {
        byte[] data = Encoding.ASCII.GetBytes(msj);
        user.socket.Send(data);
    }
}
