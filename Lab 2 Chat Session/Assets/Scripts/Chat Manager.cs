using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatManager : MonoBehaviour
{
    public static ChatManager Singleton;

    [SerializeField] ChatMessage chatMessagePrefab;
    [SerializeField] CanvasGroup chatContent;
    [SerializeField] TMP_InputField chatInput;

    private string userName;

    void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        userName = ServerDataManager.instance.userName;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            SendChatMessage(chatInput.text, userName);
            chatInput.text = "";
        }
    }

    public void CreateTCPServer()
    {

    }
    public void CreateUDPServer()
    {

    }

    public void SendChatMessage(string _message, string _fromWho = null)
    {
        if (string.IsNullOrEmpty(_message)) return;

        string S = _fromWho + ": " + _message;
        //Send message
    }

    void AddMessage(string msg)
    {
        ChatMessage CM = Instantiate(chatMessagePrefab, chatContent.transform);
        CM.SetText(msg);
    }
}
