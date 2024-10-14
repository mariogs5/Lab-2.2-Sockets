using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClientDataManager : MonoBehaviour
{
    public static ClientDataManager instance; // Patrón Singleton para acceder a la instancia

    public string serverIP; // Información que quieres pasar entre escenas
    public string userName; // Información que quieres pasar entre escenas

    public TMP_InputField serverInput;
    public TMP_InputField userInput;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Evita que se destruya al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Asegura que solo exista una instancia
        }
    }

    public void SetServerName()
    {
        serverIP = serverInput.text;
    }

    public void SetUserName()
    {
        userName = userInput.text;
    }
}
