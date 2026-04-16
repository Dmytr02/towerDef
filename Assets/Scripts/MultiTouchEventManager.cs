using System;
using System.Collections.Generic;
using UnityEngine;

public class MultiTouchEventManager : MonoBehaviour
{
    private static List<MultiTouchEventTrigger> eventTriggers = new List<MultiTouchEventTrigger>();

    public static void AddEvent(MultiTouchEventTrigger trigger)
    {
        eventTriggers.AddSorted(trigger);
    }

    public static void RemoveEvent(MultiTouchEventTrigger trigger)
    {
        eventTriggers.Remove(trigger);
    }
    private void Update()
    {
        foreach (var i in eventTriggers)
        {
            i.OrderedUpdate();
        }
    }
}
