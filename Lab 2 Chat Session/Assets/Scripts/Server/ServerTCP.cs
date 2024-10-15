using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TMPro;
using System.Text;
using System.Collections.Generic;
using UnityEngine.UIElements;
using static ServerTCP;

public class ServerTCP : MonoBehaviour
{
    Socket socket;
    Thread mainThread = null;

    //public GameObject UItextObj;
    //TextMeshProUGUI UItext;
    //string serverText;

    public struct User
    {
        public string name;
        public Socket socket;
    }

    #region NEW VARS
    public static ServerTCP Singleton;

    [SerializeField] ChatMessage chatMessagePrefab;
    [SerializeField] CanvasGroup chatContent;
    [SerializeField] TMP_InputField chatInput;

    private string userName;

    private List <User> users;
    #endregion

    void Awake()
    {
        Singleton = this;
    }


    void Start()
    {
        userName = ServerDataManager.instance.userName;
        AddMessage(userName);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendChatMessage(chatInput.text, userName);
            chatInput.text = "";
        }
    }

    #region NEW FUNCIONS
    public void SendChatMessage(string _message, string _fromWho = null)
    {
        if (string.IsNullOrEmpty(_message)) return;

        string S = _fromWho + ": " + _message;
        //Send message
        SendMessageToAllUsers(users, S);
    }

    void SendMessageToAllUsers(List <User> users, string message)
    {
        byte[] data = Encoding.ASCII.GetBytes(message);
        for (int i = 0; i < users.Count; i++)
        {
            users[i].socket.Send(data);
        }
    }

    void AddMessage(string msg)
    {
        ChatMessage CM = Instantiate(chatMessagePrefab, chatContent.transform);
        CM.SetText(msg);
    }
    #endregion

    public void startServer()
    {
        AddMessage("Starting " + ServerDataManager.instance.serverName + " Server.");

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
            AddMessage("Connected with " + clientep.Address.ToString() + " at port " + clientep.Port.ToString());

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
                AddMessage(Encoding.ASCII.GetString(data, 0, recv));
            }

            //TO DO 6
            //We'll send a ping back every time a message is received
            //Start another thread to send a message, same parameters as this one.
            Thread answer = new Thread(() => Send(user, "You have conected to: " + ServerDataManager.instance.serverName + "Server"));
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
