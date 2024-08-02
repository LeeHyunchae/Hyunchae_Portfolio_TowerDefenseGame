using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TCPClientTest : MonoBehaviour
{
    public TMP_InputField ipInput;
    public TMP_InputField portInput;
    public TMP_InputField nicknameInput;
    public TMP_InputField sendInput;
    public Button sendButton;
    public Button connectButton;
    private string clientName;

    private bool isSocketReady = false;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    private void Awake()
    {
        sendButton.onClick.AddListener(OnSendButton);
        connectButton.onClick.AddListener(ConnectedToServer);
    }

    public void ConnectedToServer()
    {
        if(isSocketReady)
        {
            return;
        }

        string ip = ipInput.text == "" ? "127.0.0.1" : ipInput.text;
        int port = portInput.text == "" ? 50202 : int.Parse(portInput.text);

        try
        {
            socket = new TcpClient(ip, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            isSocketReady = true;

        }
        catch(Exception e)
        {
            TCPChat.instance.ShowMessage("소켓 에러 : " + e.Message);
        }
    }

    private void Update()
    {
        if(isSocketReady && stream.DataAvailable)
        {
            string data = reader.ReadLine();
            if(data != null)
            {
                OnIncomingData(data);
            }
        }
    }

    private void OnIncomingData(string _data)
    {
        if(_data == "%NAME")
        {
            clientName = nicknameInput.text == "" ? "Guest" + UnityEngine.Random.Range(0, 10000) : nicknameInput.text;
            Send("&NAME|" + clientName);
            return;
        }

        TCPChat.instance.ShowMessage(_data);
    }

    private void Send(string _data)
    {
        if(!isSocketReady)
        {
            return;
        }

        writer.WriteLine(_data);
        writer.Flush();
    }

    public void OnSendButton()
    {
        if(sendInput.text.Trim() == "")
        {
            return;
        }

        string message = sendInput.text;
        sendInput.text = "";
        Send(message);
    }

    private void OnApplicationQuit()
    {
        CloseSocket();
    }

    private void CloseSocket()
    {
        if(!isSocketReady)
        {
            return;
        }

        writer.Close();
        reader.Close();
        socket.Close();
        isSocketReady = false;
    }
}
