using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatOpener : MonoBehaviour
{
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
            createTCP.SetActive(true);
        }

        if (udpActive && Input.GetKeyDown(KeyCode.Return))
        {
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
            createTCP.SetActive(false);
        }

        if (other.gameObject.tag == "serverUDP")
        {
            createUDP.SetActive(false);
        }
    }
}
