using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class RoomSelectScoket
{
    private Socket socket;
    private int port;
    private int roomId;

    public int Port { get { return port; } }
    public int RoomId { get { return roomId; } }

    public RoomSelectScoket(string address, int port)
    {
        socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        socket.NoDelay = true;
        socket.Connect(address, port);
    }

    public bool ConnectAsHost()
    {
        bool isSuccess = false;
        byte[] buf;
        byte[] receivebuf = new byte[25];
        buf = System.Text.Encoding.UTF8.GetBytes("create");
        socket.Send(buf);

        socket.Receive(receivebuf, 20, SocketFlags.None);
        string receiveString = System.Text.Encoding.UTF8.GetString(receivebuf);
        Debug.Log(receiveString);
        Debug.Log(buf);
        if(receiveString.StartsWith("noEmpty"))
        {
            isSuccess = false;
        }
        else
        {
            roomId = int.Parse(receiveString.Split(",")[0]);
            port = roomId + 12346;
            isSuccess = true;
        }

        disconnectSocket();
        return isSuccess;
    }

    public bool ConnectAsClient(int roomId)
    {
        bool isSuccess = false;
        byte[] buf;
        byte[] receivebuf = new byte[25];
        buf = System.Text.Encoding.UTF8.GetBytes(roomId.ToString() + "\n\0");
        socket.Send(buf);

        socket.Receive(receivebuf, 20, SocketFlags.None);
        string receiveString = System.Text.Encoding.UTF8.GetString(receivebuf);
        Debug.Log(receiveString);
        if (receiveString.StartsWith("noRoom"))
        {
            isSuccess= false;
        }
        else
        {
            port = int.Parse(receiveString.Split('\n')[0]);
            isSuccess= true;
;       }

        disconnectSocket();
        return isSuccess;
    }

    private void disconnectSocket()
    {
        socket.Shutdown(SocketShutdown.Both);
        socket.Close();
    }
}
