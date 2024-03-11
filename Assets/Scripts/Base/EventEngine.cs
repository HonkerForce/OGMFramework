using System;
using System.Collections.Generic;
using UnityEngine;

namespace OGMFramework
{
    public sealed class EventEngine : SingleMonoBehaviour<EventEngine>, IEventEngine
    {
        public enum EVENT_SRC_TYPE
        {
            NONE = 0,
            SYSTEM,
            MAX
        }
        
        protected Dictionary<UInt64, Func<System.Object, bool>> eventDelegates = new();

        protected Dictionary<UInt64, List<IEventHandler>> eventHandlers = new();

        protected struct EventNode
        {
            public int eventID;
            public int srcType;
            public int srcKey;
            public int delay;
            public int time;
            public object args;
        }

        protected List<EventNode> delayEvents = new();

        void Update()
        {
            var realtime = Time.realtimeSinceStartup;
            var nodes = delayEvents.GetEnumerator();
            int i = 0;
            while (nodes.MoveNext())
            {
                var node = nodes.Current;
                if (node.time + node.delay >= realtime)
                {
                    var key = GenericEventKey(node.eventID, node.srcType, node.srcKey);
                    if (eventDelegates.ContainsKey(key))
                    {
                        eventDelegates[key].Invoke(node.args);
                    }
                    else if (eventHandlers.ContainsKey(key))
                    {
                        foreach (var handler in eventHandlers[key])
                        {
                            handler.ExecuteEvent(node.eventID, node.srcType, node.srcKey, node.args);
                        }
                    }

                    delayEvents.RemoveAt(i);
                }

                i++;
            }
        }
        
        public void RegisterEvent(int eventID, int srcType, int srcKey, Func<System.Object, bool> callback)
        {
            UInt64 key = GenericEventKey(eventID, srcType, srcKey);
            if (key == 0)
            {
                Debug.LogError(string.Format($"生成唯一key={key}，注册信号失败，signal={eventID} srcType={srcType} srcKey={srcKey}"));
                return;
            }

            if (eventDelegates.ContainsKey(key))
            {
                eventDelegates[key] += callback;
            }
            else
            {
                eventDelegates.Add(key, callback);
            }
        }

        public void RegisterEvent(int eventID, int srcType, int srcKey, IEventHandler handler)
        {
            UInt64 key = GenericEventKey(eventID, srcType, srcKey);
            if (key == 0)
            {
                Debug.LogError(string.Format($"生成唯一key={key}，注册信号失败，signal={eventID} srcType={srcType} srcKey={srcKey}"));
                return;
            }

            if (eventHandlers.ContainsKey(key))
            {
                eventHandlers[key].Add(handler);
            }
            else
            {
                eventHandlers.Add(key, new List<IEventHandler>() { handler });
            }
        }

        public void UnRegisterEvent(int eventID, int srcType, int srcKey, Func<System.Object, bool> callBack)
        {
            UInt64 key = GenericEventKey(eventID, srcType, srcKey);
            if (key == 0)
            {
                Debug.LogError(string.Format($"生成唯一key={key}，注销信号失败，signal={eventID} srcType={srcType} srcKey={srcKey}"));
                return;
            }

            if (!eventDelegates.ContainsKey(key))
            {
                Debug.LogError(string.Format($"未知信号请求注销，注销信号失败，signal={eventID} srcType={srcType} srcKey={srcKey}"));
                return;
            }

            eventDelegates[key] -= callBack;
            if (eventDelegates[key].GetInvocationList().Length == 0)
            {
                eventDelegates.Remove(key);
            }
        }

        public void UnRegisterEvent(int eventID, int srcType, int srcKey, IEventHandler handler)
        {
            UInt64 key = GenericEventKey(eventID, srcType, srcKey);
            if (key == 0)
            {
                Debug.LogError(string.Format($"生成唯一key={key}，注销信号失败，signal={eventID} srcType={srcType} srcKey={srcKey}"));
                return;
            }

            if (!eventHandlers.ContainsKey(key))
            {
                Debug.LogError(string.Format($"未知信号请求注销，注销信号失败，signal={eventID} srcType={srcType} srcKey={srcKey}"));
                return;
            }

            eventHandlers[key].Remove(handler);
            if (eventHandlers[key].Count == 0)
            {
                eventHandlers.Remove(key);
            }
        }

        public bool TriggerEvent(int eventID, int srcType, int srcKey, System.Object context)
        {
            if (eventID <= 0 || srcType < 0 || srcKey < 0)
            {
                return false;
            }

            if (srcType == (int)EVENT_SRC_TYPE.NONE)
            {
                for (int i = (int)EVENT_SRC_TYPE.SYSTEM; i < (int)EVENT_SRC_TYPE.MAX; ++i)
                {
                    UInt64 key = GenericEventKey(eventID, i, srcKey);
                    if (key == 0)
                    {
                        continue;
                    }
                    
                    if (eventDelegates.ContainsKey(key))
                    {
                        if (!TriggerDelegate(key, context))
                        {
                            return false;
                        }
                    }
                    else if (eventHandlers.ContainsKey(key))
                    {
                        if (!TriggerHandler(eventID, srcType, srcKey, context))
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                UInt64 key = GenericEventKey(eventID, srcType, srcKey);
                if (key == 0)
                {
                    return false;
                }

                if (eventDelegates.ContainsKey(key))
                {
                    if (!TriggerDelegate(key, context))
                    {
                        return false;
                    }
                }
                else if (eventHandlers.ContainsKey(key))
                {
                    if (!TriggerHandler(eventID, srcType, srcKey, context))
                    {
                        return false;
                    }
                }
                else
                {
                    Debug.LogError(string.Format($"未知信号被触发，signal={eventID} srcType={srcType} srcKey={srcKey}"));
                    return false;
                }
            }

            return true;
        }

        public void TriggerDelayEvent(int eventID, int srcType, int srcKey, int delay, object context)
        {
            if (eventID <= 0 || srcType < 0 || srcKey < 0)
            {
                return;
            }

            if (srcType == (int)EVENT_SRC_TYPE.NONE)
            {
                for (int i = (int)EVENT_SRC_TYPE.SYSTEM; i < (int)EVENT_SRC_TYPE.MAX; ++i)
                {
                    UInt64 key = GenericEventKey(eventID, i, srcKey);
                    if (key == 0)
                    {
                        continue;
                    }

                    EventNode node = new()
                    {
                        eventID = eventID,
                        srcType = srcType,
                        srcKey = srcKey,
                        delay = delay,
                        time = (int)(Time.realtimeSinceStartup * 1000),
                        args = context
                    };
                    
                    delayEvents?.Add(node);
                }
            }
            else
            {
                UInt64 key = GenericEventKey(eventID, srcType, srcKey);
                if (key == 0)
                {
                    return;
                }

                EventNode node = new()
                {
                    eventID = eventID,
                    srcType = srcType,
                    srcKey = srcKey,
                    delay = delay,
                    time = (int)(Time.realtimeSinceStartup * 1000),
                    args = new[] { context }
                };
                    
                delayEvents?.Add(node);
            }
        }

        protected bool TriggerDelegate(UInt64 key, System.Object context)
        {
            var delegates = eventDelegates[key]?.GetInvocationList();
            foreach (Func<System.Object, bool> dele in delegates)
            {
                if (!dele.Invoke(context))
                {
                    return false;
                }
            }

            return true;
        }

        protected bool TriggerHandler(int eventID, int srcType, int srcKey, System.Object context)
        {
            UInt64 key = GenericEventKey(eventID, srcType, srcKey);
            if (key == 0)
            {
                return false;
            }

            if (!eventHandlers.TryGetValue(key, out var handlers))
            {
                return false;
            }
            var handler = handlers.GetEnumerator();
            while (handler.MoveNext())
            {
                if (handler.Current == null)
                {
                    continue;
                }
                if (!handler.Current.ExecuteEvent(eventID, srcType, srcKey, context))
                {
                    return false;
                }
            }

            return true;
        }

        protected UInt64 GenericEventKey(int eventID, int srcType, int srcKey)
        {
            UInt64 key = 0;

            if (eventID <= 0 || srcType < 0 || srcKey < 0)
            {
                return key;
            }

            if (srcType >= (1 << 16) || srcKey >= (1 << 16))
            {
                return key;
            }

            key = (uint)eventID << 32;
            key |= (uint)srcType << 16;
            key |= (uint)srcKey;

            return key;
        }
    }
}