using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    public GameObject createTCP;
    public GameObject createUDP;

    public GameObject joinTCP;
    public GameObject joinUDP;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "serverTCP")
        {
            createTCP.SetActive(true);
        }

        if (other.gameObject.tag == "serverUDP")
        {
            createUDP.SetActive(true);
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
