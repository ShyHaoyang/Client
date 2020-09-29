using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using UnityEngine;
using LitJson;


public class Message
{
    public class Head
    {
        public string id { get; set; }
        public string type { get; set; }
    }

    public class Body
    {
        public string vlaue { get; set; }
    }

    public Head head = new Head();
    public Body body = new Body();
}


public class Main : MonoBehaviour
{
    struct IPInfo
    {
        public string ip;
        public int remotPort;
        public int port;
    }


    public Dictionary<string, string> Clients = new Dictionary<string, string>();
    private Queue<string> replyQueue = new Queue<string>();

    

    void Start()
    {
        UDPManager.Instance.SetUp("127.0.0.1", 8080, 9090);
        UDPManager.Instance.Run();
        UDPManager.Instance.onReply = OnReply;

        HeartBeat();

        Debug.Log(CreatPingJson());
    }

    void Update()
    {
        if (replyQueue.Count != 0)
        {
            Debug.Log(replyQueue.Dequeue());
        }
    }

    private void OnDestroy()
    {
        UDPManager.Instance.Stop();
    }

    void OnReply(byte[] _reply, string _address, int _port)
    {
        string reply = System.Text.Encoding.UTF8.GetString(_reply);
        replyQueue.Enqueue(reply);
    }

    private void HeartBeat()
    {
        StartCoroutine(heartBeat());
    }

    IEnumerator heartBeat()
    {
        while (true)
        {
            UDPManager.Instance.Report("ping:127.0.0.1:8080");
            yield return new WaitForSeconds(1f);
        }
    }

    private string GetIP()
    {
        NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface adater in adapters)
        {
            if (adater.Supports(NetworkInterfaceComponent.IPv4))
            {
                UnicastIPAddressInformationCollection UniCast = adater.GetIPProperties().UnicastAddresses;
                if (UniCast.Count > 0)
                {
                    foreach (UnicastIPAddressInformation uni in UniCast)
                    {
                        if (uni.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            Debug.Log(uni.Address.ToString());
                            return uni.Address.ToString();
                        }
                    }
                }
            }
        }
        return null;
    }

    private string CreatPingJson()
    {
        Message info = new Message();
        info.head.id = GetIP();
        info.head.type = "ping";

        return JsonMapper.ToJson(info);
    }
}

