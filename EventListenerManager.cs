using System.Collections.Generic;
using System.Reflection;
using DefaultNamespace.EventSystem.Events;
using Unity.VisualScripting;
using UnityEngine;

public class EventListenerManager
{
    public List<EventListener> ListenerList;
    
    private List<NetworkEvent> networkEvents;
    int msgCount =0;
    readonly int MAX_MESSAGE_FIRE = 10;
    
    public void AddMessage(NetworkEvent msgBase)
    {
        lock (networkEvents)
        {
            networkEvents.Add(msgBase);
        }
        msgCount ++;
    }

    public void NetworkEventUpdate()
    {
        //初步判断
        if (msgCount ==0) return;
        //重复处理消息
        for (int i = 0; i < MAX_MESSAGE_FIRE; i++)
        {
            //获取第一条消息
            NetworkEvent msgBase = null;

            lock (networkEvents)
            {
                if (networkEvents.Count > 0)
                {
                    msgBase = networkEvents[0];
                    networkEvents.RemoveAt(0);
                    msgCount--;
                }
            }

            if (msgBase != null)
            {
                FireListenerEvent(msgBase);
            }
            else
            {
                break;
            }
        }
    }

    public EventListenerManager()
    {
        ListenerList = new List<EventListener>();
        networkEvents = new List<NetworkEvent>();
    }

    public void AddListener(EventListener eventListener)
    {
        ListenerList.Add(eventListener);
        MethodInfo[] methodInfo = eventListener.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        Debug.Log(eventListener.GetType());
        foreach (var method in methodInfo)
        {
            EventListen? listen = method.GetCustomAttribute<EventListen>();
            Debug.Log(method.Name);
            
            if (listen ==null) continue;
            //检测参数是否符合规定
            var paras = method.GetParameters();
            if(paras.Length != 1) continue;
            
            var para = paras[0];
            if(!typeof(Event).IsAssignableFrom(para.ParameterType)) continue;
            
            
            //转换方法为函数指针;
            //获取优先级
            EventPriority priority = listen.EventPriority;
            Event.GetHandlerList.AddEvent(priority,para.ParameterType.Name,method);
        }
        
    }

    public void RemoveListener(EventListener eventListener)
    {
        ListenerList.Remove(eventListener);
        
        MethodInfo[] methodInfo = eventListener.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var method in methodInfo)
        {
            EventListen? listen = method.GetCustomAttribute<EventListen>();

            if (listen ==null) continue;
            
            //检测参数是否符合规定
            var paras = method.GetParameters();
            if(paras.Length != 1) continue;
            
            var para = paras[0];
            if(!typeof(Event).IsAssignableFrom(para.ParameterType)) continue;
            
            //转换方法为函数指针;
            //获取优先级
            EventPriority priority = listen.EventPriority;
            Event.GetHandlerList.RemoveEvent(priority,para.ParameterType.Name,method);
        }
    }

    public void FireListenerEvent(Event e)
    {
        foreach (var eventListener in ListenerList)
        {
            Event.GetHandlerList.FireMessageEvent(eventListener,e.GetType().Name,e);
        }
    }
}
