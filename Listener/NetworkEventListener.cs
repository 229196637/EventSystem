using DefaultNamespace.EventSystem.Events;
using DefaultNamespace.System;
using UnityEngine;

namespace DefaultNamespace.EventSystem.Listener
{
    public class NetworkEventListener : EventListener
    {
        [EventListen()]
        private void PingPong(NetworkPong e)
        {
            NetworkManager.Instance.ResetPingPongTime();
        }

        [EventListen()]
        private void SendNetwork(NetworkSend e)
        {
            Debug.Log("发送成功");
        }

        [EventListen()]
        private void NetworkClose(NetworkClose e)
        {
            Debug.Log("关闭成功");
        }

        [EventListen()]
        void NetworkError(NetworkConnetError e)
        {
            Debug.Log(e.error);
        }
        
        [EventListen()]
        void NetworkSucc(NetworkConnetSucc e)
        {
            Debug.Log("连接成功");
        }
}
}