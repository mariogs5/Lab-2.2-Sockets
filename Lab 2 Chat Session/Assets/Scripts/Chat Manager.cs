using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatManager : MonoBehaviour
{
    [SerializeField] GameObject tcpChat;
    [SerializeField] GameObject udpChat;

    public void CreateTCPServer()
    {
        tcpChat.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void CreateUDPServer()
    {
        udpChat.SetActive(true);
    }
}
