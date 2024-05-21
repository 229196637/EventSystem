using System.Collections.Generic;
using System.Reflection;
using DefaultNamespace.EventSystem.Events;
using UnityEngine;

public class EventHandler
{
    //这里实现方法有很多，懒得新建类了，就使用这一种了
    private Dictionary<int,Dictionary<string,List<MethodInfo>>> eveDic;
    

    public EventHandler()
    {
        eveDic = new Dictionary<int, Dictionary<string, List<MethodInfo>>>();

        for (int i = 0; i <= (int)EventPriority.Highest; i++)
        {
            Dictionary<string, List<MethodInfo>> dic = new Dictionary<string, List<MethodInfo>>();
            eveDic.Add(i,dic);
        }
        
    }
    

    public void AddEvent(EventPriority eventPriority,string eventName, MethodInfo method)
    {
        if (eveDic[(int)eventPriority].ContainsKey(eventName))
        {
            eveDic[(int)eventPriority][eventName].Add(method);
        }
        else
        {
            List<MethodInfo> methodInfos = new List<MethodInfo>();
            methodInfos.Add(method);
            Dictionary<string,List<MethodInfo>> dicMethod = eveDic[(int)eventPriority];
            dicMethod.Add(eventName,methodInfos);
            eveDic[(int)eventPriority] = dicMethod;
        }
    }

    public void RemoveEvent(EventPriority eventPriority,string eventName, MethodInfo method)
    {
        if(!eveDic[(int)eventPriority].ContainsKey(eventName)) return;
        List<MethodInfo> methodlist = eveDic[(int)eventPriority][eventName];
            
        if(!methodlist.Contains(method)) return;
            
        methodlist.Remove(method);
        eveDic[(int)eventPriority][eventName] = methodlist;
       
    }
    
    public void FireMessageEvent(EventListener listen, string eventName, Event e)
    {
        /*if (eveDic.ContainsKey(eventName))
        {
            foreach (var methodInfo in eveDic[eventName])
            {
                object[] para = {e};
                methodInfo?.Invoke(listen,para);
            }
        }*/
        
        for (int i =(int)EventPriority.Highest;i >=0;i--)
        {
            if(!eveDic[i].ContainsKey(eventName)) continue;
            
            foreach (var methodInfo in eveDic[i][eventName])
            {
                if(methodInfo.DeclaringType != listen.GetType()) continue;
                
                object[] para = {e};
                methodInfo?.Invoke(listen,para);
            }
        }
    }
    
}
