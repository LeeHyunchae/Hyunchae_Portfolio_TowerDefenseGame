using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class TCPServerTest : MonoBehaviour
{
    public Button CreateServerButton;

    public TMP_InputField ipInput;
    public TMP_InputField portInput;

    private List<ServerClient> clients;
    private List<ServerClient> disconnectList;

    private TcpListener server;
    private bool isServerStarted = false;

    private void Awake()
    {
        CreateServerButton.onClick.AddListener(CreateServer);
    }

    public void CreateServer()
    {
        clients = new List<ServerClient>();
        disconnectList = new List<ServerClient>();

        try
        {
            int port = portInput.text == "" ? 50202 : int.Parse(portInput.text);

            IPAddress ipAddress = IPAddress.Parse("0.0.0.0"); // 모든 IP에서 접근 가능
            server = new TcpListener(ipAddress, port);

            //server = new TcpListener(ipInput.text == "" ? IPAddress.Any: IPAddress.Parse(ipInput.text), port);

            server.Start();

            StartListening();
            isServerStarted = true;
            TCPChat.instance.ShowMessage("서버가 " + port + " 에서 시작되었습니다");
        }
        catch(Exception e)
        {
            TCPChat.instance.ShowMessage("Socket Error : " + e.Message);
        }
    }

    private void Update()
    {
        if (!isServerStarted) return;

        foreach (ServerClient c in clients)
        {
            if(!GetIsConnected(c.tcp))
            {
                c.tcp.Close();
                disconnectList.Add(c);
                continue;
            }
            else
            {
                NetworkStream stream = c.tcp.GetStream();
                if(stream.DataAvailable)
                {
                    string data = new StreamReader(stream, true).ReadLine();
                    if(data != null)
                    {
                        OnIncomingData(c, data);
                    }
                }
            }
        }

        for(int i = 0; i < disconnectList.Count -1; i++)
        {
            BroadCast(disconnectList[i].clientName + " 연결이 끊어졌습니다", clients);
            clients.Remove(disconnectList[i]);
            disconnectList.RemoveAt(i);
        }
    }

    public bool GetIsConnected(TcpClient _client)
    {
        try
        {
            if(_client != null && _client.Client != null && _client.Client.Connected)
            {
                if(_client.Client.Poll(0,SelectMode.SelectRead))
                {
                    return !(_client.Client.Receive(new byte[1], SocketFlags.Peek) == 0);
                }

                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    public void StartListening()
    {
        server.BeginAcceptTcpClient(AcceptTcpClient, server);
    }

    public void AcceptTcpClient(IAsyncResult _ar)
    {
        TcpListener listener = (TcpListener)_ar.AsyncState;
        clients.Add(new ServerClient(listener.EndAcceptTcpClient(_ar)));
        StartListening();

        BroadCast("%NAME", new List<ServerClient>() { clients[clients.Count - 1] });
    }

    public void OnIncomingData(ServerClient _serverClient, string _data)
    {
        if(_data.Contains("&NAME"))
        {
            _serverClient.clientName = _data.Split('|')[1];
            BroadCast(_serverClient.clientName + "이 연결되었습니다", clients);

            return;
        }

        BroadCast(_serverClient.clientName + " : " + _data, clients);
    }

    public void BroadCast(string _data, List<ServerClient> _serverClients)
    {
        foreach(var c in _serverClients)
        {
            try
            {
                StreamWriter writer = new StreamWriter(c.tcp.GetStream());
                writer.WriteLine(_data);
                writer.Flush();
            }
            catch(Exception e)
            {
                TCPChat.instance.ShowMessage("쓰기 에러 : " + e.Message + "를 클라이언트에게 " + c.clientName);
            }
        }
    }
}

public class ServerClient
{
    public TcpClient tcp;
    public string clientName;

    public ServerClient(TcpClient clientSocket)
    {
        clientName = "Guest";
        tcp = clientSocket;
    }
}