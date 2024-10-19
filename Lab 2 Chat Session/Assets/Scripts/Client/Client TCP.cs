using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TMPro;
using System.Text;
using static ServerTCP;
using System.Collections.Generic;

public class ClientTCP : MonoBehaviour
{
    string clientText;
    Socket server;

    [SerializeField] ChatManager chatManager;  // Script to pop messages in the Server chat
    [SerializeField] TMP_InputField chatInput; // Text you want to send to the users

    private string userName;   // Server User name
    private string serverIP;   // Server User name

    void Start()
    {
        userName = "Boty";
        serverIP = "127.0.0.1";

        if (!string.IsNullOrEmpty(ClientDataManager.instance.userName))       // Check if the value is null
        {
            userName = ClientDataManager.instance.userName;  // Get the Server User name
        }

        if (!string.IsNullOrEmpty(ClientDataManager.instance.serverIP))       // Check if the value is null
        {
            serverIP = ClientDataManager.instance.serverIP;  // Get the Server User name
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))                         // When the button is pressed, start the message sendig procces
        {
            //SendLocalChatMessage(chatInput.text, userName);
            SendMessageToServer(chatInput.text, userName + ": ");
            //SendMessageToAllUsers(users, chatInput.text, userName);   // Start the message sending procces
            chatInput.text = "";                                      // Reset the input box text
        }
    }

    public void SendLocalChatMessage(string _message = "", string _fromWho = "")
    {
        if (string.IsNullOrEmpty(_message)) return;  // Check if the string have content
        //string S = _fromWho + ": " + _message;       // Format the message
        chatManager.SendMessageToTCPChat(_message);         // Send Message to the local Server chat
    }

    void SendMessageToServer(string _message = "", string _fromWho = "")
    {
        if (string.IsNullOrEmpty(_message)) return;  // Check if the string have content
        string S = _fromWho + _message;       // Format the message
  
        byte[] data = Encoding.ASCII.GetBytes(S);  // Encoding the message

        server.Send(data);
    }

    public void StartClient()
    {
        Thread connect = new Thread(Connect);  // Start the Thread with the conect funcion 
        connect.Start();
    }

    void Connect()
    {
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(serverIP), 9050);                     // Create IPEndPoint With the chosen IP and Port
        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  // Create TCP Socket 
        server.Connect(ipep);

        Thread sendThread = new Thread(Send);        // Start the Thread with the Send funcion
        sendThread.Start();

        Thread receiveThread = new Thread(Receive);  // Start the Thread with the Recive funcion
        receiveThread.Start();
    }

    void Send()
    {
        //byte[] data = Encoding.ASCII.GetBytes(chatInput.text);
        //server.Send(data);
        SendMessageToServer(userName + ": joined the chat.");
    }

    void Receive()
    {
        byte[] data = new byte[1024];
        int recv = 0;

        try
        {
            while (true)
            {
                recv = server.Receive(data);
                if (recv > 0)
                {
                    string message = Encoding.ASCII.GetString(data, 0, recv);
                    SendLocalChatMessage(message);
                }
                else
                {
                    break;
                }
            }
        }
        catch (SocketException ex)
        {
            Debug.Log("Error in Receive: " + ex.Message);
        }
    } 
}
