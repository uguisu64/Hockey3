using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.ComponentModel;

public class TitleSceneScript : MonoBehaviour
{
    public InputField ipField;
    public InputField roomField;

    public static string ipaddress;
    public static int roomId;
    public static int port;

    private RoomSelectScoket rss;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ToMainSceneHost()
    {
        ipaddress = ipField.text;

        rss = new RoomSelectScoket(ipaddress, 12345);
        if (!rss.ConnectAsHost()) return;
        port = rss.Port;
        roomId = rss.RoomId;

        SceneManager.LoadScene("MainScene");
    }

    public void ToMainSceneClient()
    {
        ipaddress = ipField.text;
        roomId = int.Parse(roomField.text);

        rss = new RoomSelectScoket(ipaddress, 12345);
        if (!rss.ConnectAsClient(roomId)) return;
        port = rss.Port;

        SceneManager.LoadScene("MainScene");
    }
}
