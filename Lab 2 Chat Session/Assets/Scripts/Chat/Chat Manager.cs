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
    }

    public void SendMessageToTCPChat(string text)
    {
        Message newMessage = new Message();
        newMessage.text = text;

        GameObject newText = Instantiate(textObject, tcpChatPanel.transform);
        newMessage.textObject = newText.GetComponent<TextMeshProUGUI>();
        newMessage.textObject.text = newMessage.text;

        messageList.Add(newMessage);
    }

    public void SendMessageToUDPChat(string text)
    {
        Message newMessage = new Message();
        newMessage.text = text;

        GameObject newText = Instantiate(textObject, udpChatPanel.transform);
        newMessage.textObject = newText.GetComponent<TextMeshProUGUI>();
        newMessage.textObject.text = newMessage.text;

        messageList.Add(newMessage);
    }
}

[System.Serializable]
public class Message
{
    public string text;
    public TextMeshProUGUI textObject;
}
