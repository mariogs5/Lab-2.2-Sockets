using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatOpener : MonoBehaviour
{
    [SerializeField] vThirdPersonInput characterInput;

    public GameObject openServerTCPChat;  // Open / Close Server TCP Chat
    public GameObject openServerUDPChat;  // Open / Close Server UDP Chat

    private void Update()
    {
        if(!characterInput.chatOpen && !openServerTCPChat.activeInHierarchy && Input.GetKeyDown(KeyCode.T))
        {
            characterInput.chatOpen = true;
            openServerTCPChat.SetActive(true);
        }

        if (!characterInput.chatOpen && !openServerUDPChat.activeInHierarchy && Input.GetKeyDown(KeyCode.U))
        {
            characterInput.chatOpen = true;
            openServerUDPChat.SetActive(true);
        }

        if (openServerTCPChat.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
        {
            openServerTCPChat.SetActive(false);
            characterInput.chatOpen = false;
        }

        if (openServerUDPChat.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
        {
            openServerUDPChat.SetActive(false);
            characterInput.chatOpen = false;
        }
    }
}
