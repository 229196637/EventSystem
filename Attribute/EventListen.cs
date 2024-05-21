using System;

public class EventListen : Attribute
{  
    EventPriority eventPriority;
    public EventListen(EventPriority eventPriority)
    {
        this.eventPriority = eventPriority;
    }

    public EventListen()
    {
        this.eventPriority = EventPriority.Normal;
    }
    
    public EventPriority EventPriority => eventPriority;
}



public enum EventPriority
{
    Lowest,
    Low,
    Normal,
    High,
    Highest,
}
