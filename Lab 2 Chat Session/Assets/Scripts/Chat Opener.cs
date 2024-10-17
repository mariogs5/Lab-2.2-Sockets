using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatOpener : MonoBehaviour
{
    [SerializeField] vThirdPersonInput characterInput;

    public GameObject openServerTCPChat;  // Abrir / Cerrar Server UDP Chat
    public GameObject openServerUDPChat;  // Abrir / Cerrar Server TCP Chat

    public GameObject openClientTCPChat;  // Abrir / Cerrar Client TCP Chat
    public GameObject openClientUDPChat;  // Abrir / Cerrar Client UDP Chat

    private bool nearTCPActive = false;
    private bool nearUDPActive = false;

    private void Update()
    {
        if(nearTCPActive && Input.GetKeyDown(KeyCode.Return))
        {
            characterInput.chatOpen = true;
            openServerTCPChat.SetActive(true);
        }

        if (nearUDPActive && Input.GetKeyDown(KeyCode.Return))
        {
            characterInput.chatOpen = true;
            openServerUDPChat.SetActive(true);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "serverTCP")
        {
            nearTCPActive = true;
        }

        if (other.gameObject.tag == "serverUDP")
        {
            nearUDPActive = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "serverTCP")
        {
            characterInput.chatOpen = false;
            openServerTCPChat.SetActive(false);
        }

        if (other.gameObject.tag == "serverUDP")
        {
            characterInput.chatOpen = false;
            openServerUDPChat.SetActive(false);
        }
    }
}
