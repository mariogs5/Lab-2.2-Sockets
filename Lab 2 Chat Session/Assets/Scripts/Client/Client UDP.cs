using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TMPro;
using System.Text;

public class ClientUDP : MonoBehaviour
{
    Socket server;

    [SerializeField] ChatManager chatManager;  // Script to pop messages in the Server chat
    [SerializeField] TMP_InputField chatInput; // Text you want to send to the users

    private string userName;   // Server User name
    private string serverIP;   // Server User name

    void Start()
    {
        userName = "Default Name";
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

    public void StartClient()  // Execute this funcion when the "Connect to Server" button is pressed
    {
        Thread mainThread = new Thread(Send);
        mainThread.Start();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))                         // When the button is pressed, start the message sendig procces
        {
            SendMessageToServer(chatInput.text, userName + ": ");
            chatInput.text = "";                                      // Reset the input box text
        }
    }

    public void SendLocalChatMessage(string _message = "", string _fromWho = "")
    {
        if (string.IsNullOrEmpty(_message)) return;  // Check if the string have content
        //string S = _fromWho + ": " + _message;       // Format the message
        chatManager.SendMessageToUDPChat(_message);         // Send Message to the local Server chat
    }

    void SendMessageToServer(string _message = "", string _fromWho = "")
    {
        if (string.IsNullOrEmpty(_message)) return;  // Check if the string have content
        string S = _fromWho + _message;       // Format the message

        int port = 9050;
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(serverIP), port);
        server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        byte[] data = new byte[1024];
        data = Encoding.UTF8.GetBytes(S);
        server.SendTo(data, data.Length, SocketFlags.None, ipep);
    }

    void Send()
    {
        int port = 9050;
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(serverIP), port);
        server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        byte[] data = new byte[1024];
        string handshake = userName + ": Joined the chat";
        data = Encoding.UTF8.GetBytes(handshake);
        server.SendTo(data, data.Length, SocketFlags.None, ipep);

        Thread receive = new Thread(Receive);
        receive.Start();

    }

    void Receive()
    {
        IPEndPoint sender = new IPEndPoint(IPAddress.Parse(serverIP), 9050);
        EndPoint Remote = (EndPoint)(sender);
        byte[] data = new byte[1024];
        int recv = 0;

        //clientText = ("Message received from {0}: " + Remote.ToString());
        //clientText = clientText += "Message received from {0}: " + Remote.ToString() + "\n" + Encoding.ASCII.GetString(data, 0, recv) + "\n";

        //  Print the recived message to the local console 

        try
        {
            while (true)
            {
                recv = server.ReceiveFrom(data, ref Remote);
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
