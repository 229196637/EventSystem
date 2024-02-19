using System.Reflection;
using EventSystem.Attribute;

namespace EventSystem;

public class EventListenerManager
{
    public List<EventListener> ListenerList;

    public EventListenerManager()
    {
        ListenerList = new List<EventListener>();
    }

    public void AddListener(EventListener eventListener)
    {
        ListenerList.Add(eventListener);
        MethodInfo[] methodInfo = eventListener.GetType().GetMethods();

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
            Event.GetHandlerList.AddEvent(priority,para.ParameterType.Name,method);
        }
    }

    public void RemoveListener(EventListener eventListener)
    {
        ListenerList.Remove(eventListener);
        
        MethodInfo[] methodInfo = eventListener.GetType().GetMethods();

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
