using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class SocketReceiver
{
    private Socket socket;
    private float enemyPosition;
    private GameState gameState;
    private Vector3 ballPostion;
    private int myScore;
    private int enemyScore;

    public float EnemyPosition { get { return enemyPosition; } }
    public GameState GameState { get { return gameState; } }
    public Vector3 BallPosition { get { return ballPostion; } }
    public int MyScore { get { return myScore; } }
    public int EnemyScore { get { return enemyScore; } }

    public SocketReceiver(string address, int port)
    {
        socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        socket.NoDelay = true;
        Debug.Log(address + port);
        socket.Connect(address, port);
        enemyPosition = 0;
        gameState = GameState.WAITING;
        ballPostion = Vector2.zero;
        myScore = 0;
        enemyScore = 0;
    }

    public void ReceiveSocketData()
    {
        byte[] responseBytes = new byte[25];
        int count;
        bool isData = false;
        string receiveString = "";
        while(socket.Available > 0)
        {
            count = socket.Receive(responseBytes, 20, SocketFlags.None);
            receiveString = System.Text.Encoding.UTF8.GetString(responseBytes);
            isData = false;
            if (receiveString.Length <= 0) continue;
            if (receiveString.StartsWith("waiting"))
            {
                gameState = GameState.WAITING;
            }
            else if (receiveString.StartsWith("start"))
            {
                gameState = GameState.ISGAME;
            }
            else if (receiveString.StartsWith("win"))
            {
                gameState = GameState.FINISHED_WIN;
                disconnectSocket();
                break;
            }
            else if (receiveString.StartsWith("lose"))
            {
                gameState = GameState.FINISHED_LOSE;
                disconnectSocket();
                break;
            }
            else if (receiveString.StartsWith("score"))
            {
                string[] scores = receiveString.Split(',');
                myScore = int.Parse(scores[1]);
                enemyScore = int.Parse(scores[2].Split('\n')[0]);
            }
            else
            {
                isData = true;
            }
        }
        if(isData)
        {
            string[] datas = receiveString.Split(',');
            enemyPosition = myParse(datas[0]);
            float ballX = myParse(datas[1]);
            float ballY = myParse(datas[2].Substring(0,4));
            ballPostion = new Vector3(ballX, ballY);
        }
    }

    public void SendPositionSocket(float myPosition)
    {
        byte[] sendBytes;
        string sendData = myPosition.ToString("F2") + "\n";
        sendBytes = System.Text.Encoding.UTF8.GetBytes(sendData);

        socket.SendAsync(sendBytes, SocketFlags.None);
    }

    private float myParse(string s)
    {
        if(s.StartsWith("0.00"))
        {
            return 0.0f;
        }
        else
        {
            return float.Parse(s);
        }
    }

    private void disconnectSocket()
    {
        socket.Shutdown(SocketShutdown.Both);
        socket.Close();
    }
}

public enum GameState
{
    WAITING,
    ISGAME,
    FINISHED_WIN,
    FINISHED_LOSE
}
