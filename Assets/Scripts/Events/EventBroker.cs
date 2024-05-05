using System;
using Data;
using UnityEngine.Events;

/// <summary>
/// Class for Observer Pattern implmentation
/// </summary>
public static class EventBroker
{
    /*Personally I would prefer to use an Action instead of UnityEvents 
     since it is more performant.*/
    public static UnityEvent<ObjectData> DataPropagation = new UnityEvent<ObjectData>();
    public static Action<int> ObjectDetroyed;
    public static Action<string> ObjectClick;

    public static void CallDataPropagation(ObjectData data)
    {
        DataPropagation?.Invoke(data);
    }

    public static void CallObjectDestroyed(int id)
    {
        ObjectDetroyed?.Invoke(id);
    }

    public static void CallObjectClick(string data)
    {
        ObjectClick?.Invoke(data);
    }
}
