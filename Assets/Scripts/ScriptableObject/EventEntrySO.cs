using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "EventEntry", menuName = "Scriptable Object/EventEntry")]
public class EventEntrySO : ScriptableObject
{
    private Dictionary<EventTriggerType, EventTrigger.Entry> _entries = new();
    private Dictionary<EventTriggerType, UnityEvent<CursorSource>> _cursorActions = new();
    private Dictionary<UIType, UnityEvent<bool>> _uiActions = new();
    private Dictionary<CursorSource, Dictionary<CursorSource, UnityEvent<ItemIdQuantityPair>>> _cursorInteractions = new();
    public void Subscribe(EventTriggerType type, UnityAction<BaseEventData> action)
    {
        if(!_entries.TryGetValue(type, out EventTrigger.Entry entry))
        {
            entry = new() { eventID = type };
            _entries.Add(type, entry);
        }
        entry.callback.AddListener(action);
    }
    public void Subscribe(EventTriggerType type, UnityAction<CursorSource> action)
    {
        if (!_cursorActions.TryGetValue(type, out UnityEvent<CursorSource> entry))
        {
            entry = new();
            _cursorActions.Add(type, entry);
        }
        entry.AddListener(action);
    }
    public void Subscribe(UIType type, UnityAction<bool> action)
    {
        if (!_uiActions.TryGetValue(type, out UnityEvent<bool> entry))
        {
            entry = new UnityEvent<bool>();
            _uiActions.Add(type, entry);
        }
        entry.AddListener(action);
    }
    public void Subscribe(CursorSource iSource, CursorSource fSource, UnityAction<ItemIdQuantityPair> action)
    {
        if (!_cursorInteractions.TryGetValue(iSource, out Dictionary<CursorSource, UnityEvent<ItemIdQuantityPair>> entryDict))
        {
            entryDict = new();
            _cursorInteractions.Add(iSource, entryDict);
        }

        if (!entryDict.TryGetValue(fSource, out UnityEvent<ItemIdQuantityPair> entry))
        {
            entry = new();
            entryDict.Add(fSource, entry);
        }

        entry.AddListener(action);
    }
    public void Unsubscribe(EventTriggerType type, UnityAction<BaseEventData> action)
    {
        if (_entries.TryGetValue(type, out EventTrigger.Entry entry))
        {
            entry.callback.RemoveListener(action);
        }
    }

    public void Unsubscribe(EventTriggerType type, UnityAction<CursorSource> action)
    {
        if (_cursorActions.TryGetValue(type, out UnityEvent<CursorSource> entry))
        {
            _cursorActions.Remove(type);
        }
    }
    public void Unsubscribe(UIType type, UnityAction<bool> action)
    {
        if (_uiActions.TryGetValue(type, out UnityEvent<bool> entry))
        {
            _uiActions.Remove(type);
        }
    }

    public void Publish(EventTriggerType type, BaseEventData data)
    {
        if (_entries.TryGetValue(type, out EventTrigger.Entry entry))
        {
            entry.callback?.Invoke(data);
        }
    }

    public void Publish(EventTriggerType type, CursorSource source)
    {
        if (_cursorActions.TryGetValue(type, out UnityEvent<CursorSource> entry))
        {
            entry.Invoke(source);
        }
    }

    public void Publish(UIType type, bool isOpened)
    {
        if (_uiActions.TryGetValue(type, out UnityEvent<bool> entry))
        {
            entry.Invoke(isOpened);
        }
    }

    public void Publish(CursorSource iSource, CursorSource fSource, ItemIdQuantityPair item)
    {
        if (_cursorInteractions.TryGetValue(iSource, out Dictionary<CursorSource, UnityEvent<ItemIdQuantityPair>> entryDict))
        {
            if(entryDict.TryGetValue(fSource, out UnityEvent <ItemIdQuantityPair> entry)) {
                entry.Invoke(item);
            }
        }
    }
}