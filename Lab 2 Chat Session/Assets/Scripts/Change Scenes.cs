using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
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
