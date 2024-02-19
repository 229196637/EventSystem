namespace EventSystem;

public class Event
{
    public string EventName;
    
    public static EventHandler EventHandlerList = new EventHandler();
    public EventHandler GetHandlers => EventHandlerList;
    public static EventHandler GetHandlerList => EventHandlerList;

    public Event()
    {
        EventName = GetType().Name;
    }
    
}
