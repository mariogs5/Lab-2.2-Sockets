using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TMPro;
using System.Text;
using System.Collections.Generic;

public class ServerUDP : MonoBehaviour
{
    Socket socket;

    public static ServerUDP Singleton;

    [SerializeField] ChatManager chatManager;  // Script to pop messages in the Server chat
    [SerializeField] TMP_InputField chatInput; // Text you want to send to the users

    private string userName;   // Server User name
    private string serverName; // Server name

    private List<EndPoint> users; // List of Users Connected to the Server

    void Awake()
    {
        Singleton = this;
    }

    void Start()
    {
        users = new List<EndPoint>();

        if (!string.IsNullOrEmpty(ServerDataManager.instance.serverName))       // Check if the value is null
        {
            serverName = ServerDataManager.instance.serverName;  // Get the Server User name
        }

        if (!string.IsNullOrEmpty(ServerDataManager.instance.userName))         // Check if the value is null
        {
            userName = ServerDataManager.instance.userName;      // Get the Server name
        }

        startServer();
    }

    public void SendLocalChatMessage(string _message = "", string _fromWho = "")
    {
        if (string.IsNullOrEmpty(_message)) return;         // Check if the string have content
        string S = _fromWho + _message;                     // Format the message
        chatManager.SendMessageToUDPChat(_message);         // Send Message to the local Server chat
    }

    void SendMessageToAllUsers(List<EndPoint> users, string _message = "", string _fromWho = "")
    {
        if (string.IsNullOrEmpty(_message)) return;  // Check if the string have content
        string S = _fromWho + _message;              // Format the message
        chatManager.SendMessageToUDPChat(S);         // Send Message to the local Server chat

        byte[] data = Encoding.ASCII.GetBytes(S);    // Encoding the message

        for (int i = 0; i < users.Count; i++)        // Iterate the Users list in order to send them all the message
        {
            socket.SendTo(data, data.Length, SocketFlags.None, users[i]);  // Send the message to the specific user               
        }
    }

    public void startServer()
    {
        SendLocalChatMessage("Starting " + serverName + " UDP Server...");

        int port = 9050;
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
        socket.Bind(ipep);

        Thread newConnection = new Thread(Receive);
        newConnection.Start();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))                               // When the button is pressed, start the message sendig procces
        {
            //SendLocalChatMessage(chatInput.text,userName);
            SendMessageToAllUsers(users, chatInput.text, userName + ": ");  // Start the message sending procces
            chatInput.text = "";                                            // Reset the input box text
        }
    }

    void Receive()
    {
        int recv;
        byte[] data = new byte[1024];

        SendLocalChatMessage("Waiting for new Client...");

        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint Remote = (EndPoint)(sender);

        //while (true)
        //{
        //    recv = socket.ReceiveFrom(data, ref Remote);

        //    serverText = serverText + "\n" + "Message received from {0}:" + Remote.ToString();
        //    serverText = serverText + "\n" + Encoding.ASCII.GetString(data, 0, recv);

        //    Thread answer = new Thread(() => Send(Remote));
        //    answer.Start();
        //}

        while (true)
        {
            recv = socket.ReceiveFrom(data, ref Remote);

            users.Add(Remote);

            if (recv == 0)
                break;
            else
            {
                SendMessageToAllUsers(users, Encoding.ASCII.GetString(data, 0, recv));
            }
        }

    }

    void Send(EndPoint Remote)
    {
        byte[] data = new byte[1024];
        string welcome = "Ping";

        data = Encoding.ASCII.GetBytes(welcome);
        socket.SendTo(data, data.Length, SocketFlags.None, Remote);
    }
}
