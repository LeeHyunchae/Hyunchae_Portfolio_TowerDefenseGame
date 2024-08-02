using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using UnityEngine.Networking;
using System.Text;
using Newtonsoft.Json;

public class TCPClient : MonoBehaviour
{
    private const string serverFullLink = "ec2-52-78-67-151.ap-northeast-2.compute.amazonaws.com:9000";
    private readonly string serverLink = "ec2-52-78-67-151.ap-northeast-2.compute.amazonaws.com";
    private const int serverPort = 9000;

    private TcpClient tcpClient;

    private void Start()
    {
        //StartCoroutine(UnityWebRequestGETTest());

        try
        {
            // TCP 클라이언트 생성
            using (tcpClient = new TcpClient(serverLink, serverPort))
            {
                tcpClient.SendBufferSize = 4096;
                tcpClient.ReceiveBufferSize = 4096;

                Debug.Log("서버에 연결되었습니다.");

                ReqMapData reqMapData = new ReqMapData
                {
                    id = 0
                };

                SendMessage(reqMapData);

                tcpClient.Close();

                // 네트워크 스트림 가져오기
                //NetworkStream stream = tcpClient.GetStream();

                //// 서버로 보낼 메시지
                //string message = "Hello from client!";
                //byte[] data = Encoding.UTF8.GetBytes(message);

                //// 데이터 전송
                //stream.Write(data, 0, data.Length);
                //Debug.Log("데이터를 전송했습니다: "+ message);

                //// 서버로부터 응답 받기
                //data = new byte[256];
                //int bytes = stream.Read(data, 0, data.Length);
                //string response = Encoding.UTF8.GetString(data, 0, bytes);
                //Debug.Log("서버로부터 응답: "+ response);
            }
        }
        catch (SocketException e)
        {
            Debug.Log("소켓 예외 발생: "+ e);
        }
        catch (Exception e)
        {
            Debug.Log("예외 발생: "+ e);
        }
    }

    IEnumerator UnityWebRequestGETTest()
    {
        // UnityWebRequest에 내장되있는 GET 메소드를 사용한다.
        UnityWebRequest www = UnityWebRequest.Get(serverLink);

        yield return www.SendWebRequest();  // 응답이 올때까지 대기한다.

        if (www.error == null)  // 에러가 나지 않으면 동작.
        {
            Debug.Log(www.downloadHandler.text);
        }
        else
        {
            Debug.Log("error");
        }
    }

    private void SendMessage(PacketBase _packet)
    {
        NetworkStream stream = tcpClient.GetStream();

        //string jsonData = JsonConvert.SerializeObject(_packet);

        //byte[] buffer = new byte[4096];

        //byte[] data = Encoding.ASCII.GetBytes(jsonData,0,jsonData.Length,buffer,4);

        //stream.Write(data, 0, data.Length);

        ////data = new byte[256];
        //int bytes = stream.Read(data, 0, data.Length);
        //string response = Encoding.UTF8.GetString(data, 0, bytes);
        //Debug.Log("서버로부터 응답: " + response);
    }
}

public class PacketBase
{
    public string packet = "";
}

public class ReqMapData : PacketBase
{
    public int id;
}

public class ResMapData : PacketBase
{
    public string result;
    public MapData data;
}