using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public GameObject tcpChatPanel;
    public GameObject udpChatPanel;
    public GameObject textObject;

    Message tcpmessageIncoming;
    bool tcpMessage;

    Message udpmessageIncoming;
    bool udpMessage;

    [SerializeField]
    List<Message> messageList = new List<Message>();

    void Start()
    {
        
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space) && tcpChatPanel.activeInHierarchy)
        //{
        //    SendMessageToTCPChat("You pressed the space");
        //}

        if (tcpMessage)
        {
            CreateTCPMessage();
            tcpMessage = false;
        }

        if (udpMessage)
        {
            CreateUDPMessage();
            udpMessage = false;
        }
    }

    void CreateTCPMessage()
    {
        GameObject newText = Instantiate(textObject, tcpChatPanel.transform);
        tcpmessageIncoming.textObject = newText.GetComponent<TextMeshProUGUI>();
        tcpmessageIncoming.textObject.text = tcpmessageIncoming.text;
    }

    void CreateUDPMessage()
    {
        GameObject newText = Instantiate(textObject, udpChatPanel.transform);
        udpmessageIncoming.textObject = newText.GetComponent<TextMeshProUGUI>();
        udpmessageIncoming.textObject.text = udpmessageIncoming.text;
    }

    public void SendMessageToTCPChat(string text)
    {
        Message newMessage = new Message();
        newMessage.text = text;

        tcpmessageIncoming = newMessage;
        tcpMessage = true;

        //GameObject newText = Instantiate(textObject, tcpChatPanel.transform);
        //newMessage.textObject = newText.GetComponent<TextMeshProUGUI>();
        //newMessage.textObject.text = newMessage.text;

        messageList.Add(newMessage);
    }

    public void SendMessageToUDPChat(string text)
    {
        Message newMessage = new Message();
        newMessage.text = text;

        udpmessageIncoming = newMessage;
        udpMessage = true;

        //GameObject newText = Instantiate(textObject, udpChatPanel.transform);
        //newMessage.textObject = newText.GetComponent<TextMeshProUGUI>();
        //newMessage.textObject.text = newMessage.text;

        messageList.Add(newMessage);
    }
}

[System.Serializable]
public class Message
{
    public string text;
    public TextMeshProUGUI textObject;
}
