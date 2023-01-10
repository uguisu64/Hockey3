using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleSceneScript : MonoBehaviour
{
    public InputField ipField;
    public InputField portField;

    public static string ipaddress;
    public static int port;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ToMainScene()
    {
        ipaddress = ipField.text;
        port = int.Parse(portField.text);

        SceneManager.LoadScene("MainScene");
    }
}
