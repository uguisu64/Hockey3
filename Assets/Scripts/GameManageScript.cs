using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManageScript : MonoBehaviour
{
    private SocketReceiver socketReceiver;

    public GameObject playerBar;
    public GameObject enemyBar;
    public GameObject ball;

    public Text scoreText;
    public Text finishText;

    public Button nextButton;
    public Button retryButton;
    // Start is called before the first frame update
    void Start()
    {
        socketReceiver = new SocketReceiver(TitleSceneScript.ipaddress, TitleSceneScript.port);
    }

    // Update is called once per frame
    void Update()
    {
        if(socketReceiver.GameState == GameState.FINISHED_WIN || socketReceiver.GameState == GameState.FINISHED_LOSE)
        {
            return;
        }
        socketReceiver.ReceiveSocketData();
        switch(socketReceiver.GameState)
        {
            case GameState.ISGAME:
                Vector3 enemyBarPos = enemyBar.transform.position;
                enemyBar.transform.position = new Vector3(enemyBarPos.x, socketReceiver.EnemyPosition, enemyBarPos.z);

                socketReceiver.SendPositionSocket(playerBar.transform.position.y);
                ball.transform.position = socketReceiver.BallPosition;

                scoreText.text = socketReceiver.MyScore.ToString() + ":" + socketReceiver.EnemyScore.ToString();
                break;

            case GameState.WAITING:
                break;

            case GameState.FINISHED_WIN:
                finishText.text = "WIN";
                nextButton.gameObject.SetActive(true);
                retryButton.gameObject.SetActive(true);
                break;

            case GameState.FINISHED_LOSE:
                finishText.text = "LOSE";
                nextButton.gameObject.SetActive(true);
                retryButton.gameObject.SetActive(true);
                break;
        }
    }

    public void onRetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void onNextButton()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
