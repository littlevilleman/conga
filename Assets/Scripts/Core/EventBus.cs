using System.Collections.Generic;
using System;
using System.Linq;

public abstract class EventContext { }

public delegate void EventListener(EventContext e);

public static class EventBus
{
    private static Dictionary<Type, Dictionary<int, EventListener>> EventListeners;

    public static void Register<T>(Action<T> listener) where T : EventContext
    {
        Type eventType = typeof(T);

        if (EventListeners == null)
            EventListeners = new Dictionary<Type, Dictionary<int, EventListener>>();

        if (!EventListeners.ContainsKey(eventType) || EventListeners[eventType] == null)
        {
            EventListeners[eventType] = new Dictionary<int, EventListener>();
        }

        // Wrap a type converstion around the event listener
        void Wrapper(EventContext aEvent)
        {
            listener((T)aEvent);
        }

        //Register
        EventListeners[eventType].Add(listener.GetHashCode(), Wrapper);
    }

    public static bool Unregister<T>(Action<T> listener) where T : EventContext
    {
        Type eventType = typeof(T);

        if (EventListeners == null || !EventListeners.ContainsKey(eventType) || EventListeners[eventType] == null)
            return false;

        return EventListeners[eventType].Remove(listener.GetHashCode());
    }

    public static void Send(EventContext eventContext)
    {
        Type trueEventContextClass = eventContext.GetType();
        if (EventListeners == null || !EventListeners.ContainsKey(trueEventContextClass))
            return;

        int[] keys = EventListeners[trueEventContextClass].Keys.ToArray();
        foreach (int key in keys)
            EventListeners[trueEventContextClass][key](eventContext);
    }
}
public class EventStartGame : EventContext
{

}
public class EventExitGame : EventContext
{

}
public class EventRestartGame : EventContext
{

}
public class EventBackToMenu : EventContext
{

}

public class EventChangeDifficulty : EventContext
{
    public int difficulty;
}

public class EventMuteMusic : EventContext
{
    public bool mute;
}

public class EventMuteSound: EventContext
{
    public bool mute;
}