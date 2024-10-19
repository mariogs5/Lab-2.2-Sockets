using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;

public class IPScript : MonoBehaviour
{
    public TextMeshProUGUI ipText;

    void Start()
    {
        if (ipText != null)
        {
            ipText.text = "IP: " + GetLocalIPAddress();
        }
        else
        {
            Debug.LogError("Cant Get the Text component");
        }
    }

    // Función para obtener la dirección IP local
    string GetLocalIPAddress()
    {
        string localIP = "No IP Found";
        try
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error getting the IP: " + e.Message);
        }
        return localIP;
    }
}
