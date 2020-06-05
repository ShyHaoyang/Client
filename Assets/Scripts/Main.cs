using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public Dictionary<string, string> Clients = new Dictionary<string, string>();
    private Queue<string> replyQueue = new Queue<string>();

    void Start()
    {
        UDPManager.Instance.SetUp("127.0.0.1", 8080, 9090);
        UDPManager.Instance.Run();
        UDPManager.Instance.onReply = OnReply;

        HeartBeat();
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
}

