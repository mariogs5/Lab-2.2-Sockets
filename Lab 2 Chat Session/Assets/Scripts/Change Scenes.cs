using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    public GameObject menu;
    public GameObject server;
    public GameObject client;

    public void GoBackToMenu()
    {
        server.SetActive(false);
        client.SetActive(false);
        menu.SetActive(true);
    }
    public void OnServerClicked()
    {
        server.SetActive(true);
        menu.SetActive(false);
    }

    public void OnClientClicked()
    {
        client.SetActive(true);
        menu.SetActive(false);
    }

    public void ChangeToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ChangeToServer()
    {
        SceneManager.LoadScene(1);
    }

    public void ChangeToClient()
    {
        SceneManager.LoadScene(2);
    }
}
