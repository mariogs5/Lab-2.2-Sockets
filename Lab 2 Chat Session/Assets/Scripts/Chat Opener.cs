using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatOpener : MonoBehaviour
{
    [SerializeField] vThirdPersonInput characterInput;

    public GameObject createTCP;
    public GameObject createUDP;

    public GameObject joinTCP;
    public GameObject joinUDP;

    private bool tcpActive = false;
    private bool udpActive = false;

    private void Update()
    {
        if(tcpActive && Input.GetKeyDown(KeyCode.Return))
        {
            characterInput.chatOpen = true;
            createTCP.SetActive(true);
        }

        if (udpActive && Input.GetKeyDown(KeyCode.Return))
        {
            characterInput.chatOpen = true;
            createUDP.SetActive(true);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "serverTCP")
        {
            tcpActive = true;
        }

        if (other.gameObject.tag == "serverUDP")
        {
            udpActive = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "serverTCP")
        {
            characterInput.chatOpen = false;
            createTCP.SetActive(false);
        }

        if (other.gameObject.tag == "serverUDP")
        {
            characterInput.chatOpen = false;
            createUDP.SetActive(false);
        }
    }
}
