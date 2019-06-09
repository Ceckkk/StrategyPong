using System;
using System.Collections.Generic;

public static class EventAggregator<T> where T : class
{
    private static List<Action<T>> _listeners = new List<Action<T>>();

    public static void Subscribe(Action<T> listener)
    {
        _listeners.Add(listener);
    }

    public static void Publish(T message)
    {
        foreach (var action in _listeners)
        {
            action.Invoke(message);
        }
    }

    public static void Unsubscribe(Action<T> action)
    {
        _listeners.Remove(action);
    }

    public static void UnsubscribeAll()
    {
        _listeners.Clear();
    }
}