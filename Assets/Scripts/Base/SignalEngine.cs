using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

namespace YFramework
{
    public class SignalEngine : ISignalEngine
    {
        public enum SIGNAL_SRC_TYPE
        {
            SRC_TYPE_NONE = 0,
            SRC_TYPE_SYSTEM,
            SRC_TYPE_MAX
        }
        
        protected Dictionary<UInt64, Func<System.Object, bool>> signalDelegates = new();

        protected struct SignalNode
        {
            public UInt64 key;
            public int delay;
            public int time;
            public object[] args;
        }

        protected List<SignalNode> delaySignals = new();
        
        public virtual void RegisterSignal(int signal, int srcType, int srcKey, Func<System.Object, bool> callback)
        {
            UInt64 key = GenericSignalKey(signal, srcType, srcKey);
            if (key == 0)
            {
                Debug.LogError(string.Format($"生成唯一key={key}，注册信号失败，signal={signal} srcType={srcType} srcKey={srcKey}"));
                return;
            }

            if (signalDelegates.ContainsKey(key))
            {
                signalDelegates[key] += callback;
            }
            else
            {
                signalDelegates.Add(key, callback);
            }
        }

        public virtual void UnRegisterSignal(int signal, int srcType, int srcKey, Func<System.Object, bool> callBack)
        {
            UInt64 key = GenericSignalKey(signal, srcType, srcKey);
            if (key == 0)
            {
                Debug.LogError(string.Format($"生成唯一key={key}，注销信号失败，signal={signal} srcType={srcType} srcKey={srcKey}"));
                return;
            }

            if (!signalDelegates.ContainsKey(key))
            {
                Debug.LogError(string.Format($"未知信号请求注销，注销信号失败，signal={signal} srcType={srcType} srcKey={srcKey}"));
                return;
            }

            signalDelegates[key] -= callBack;
            if (signalDelegates[key].GetInvocationList().Length == 0)
            {
                signalDelegates.Remove(key);
            }
        }

        public virtual bool TriggerSignal(int signal, int srcType, int srcKey, Object context)
        {
            if (signal <= 0 || srcType < 0 || srcKey < 0)
            {
                return false;
            }

            if (srcType == (int)SIGNAL_SRC_TYPE.SRC_TYPE_NONE)
            {
                for (int i = (int)SIGNAL_SRC_TYPE.SRC_TYPE_SYSTEM; i < (int)SIGNAL_SRC_TYPE.SRC_TYPE_MAX; ++i)
                {
                    UInt64 key = GenericSignalKey(signal, i, srcKey);
                    if (key == 0)
                    {
                        continue;
                    }
                    
                    if (!signalDelegates.ContainsKey(key))
                    {
                        continue;
                    }

                    if (!TriggerSignal(key, context))
                    {
                        return false;
                    }
                }
            }
            else
            {
                UInt64 key = GenericSignalKey(signal, srcType, srcKey);
                if (key == 0)
                {
                    return false;
                }

                if (!signalDelegates.ContainsKey(key))
                {
                    Debug.LogError(string.Format($"未知信号被触发，signal={signal} srcType={srcType} srcKey={srcKey}"));
                    return false;
                }

                return TriggerSignal(key, context);
            }

            return true;
        }

        public virtual bool TriggerDelaySignal(int signal, int srcType, int srcKey, int delay, object context)
        {
            if (signal <= 0 || srcType < 0 || srcKey < 0)
            {
                return false;
            }

            if (srcType == (int)SIGNAL_SRC_TYPE.SRC_TYPE_NONE)
            {
                for (int i = (int)SIGNAL_SRC_TYPE.SRC_TYPE_SYSTEM; i < (int)SIGNAL_SRC_TYPE.SRC_TYPE_MAX; ++i)
                {
                    UInt64 key = GenericSignalKey(signal, i, srcKey);
                    if (key == 0)
                    {
                        continue;
                    }
                    
                    if (!signalDelegates.ContainsKey(key))
                    {
                        continue;
                    }

                    SignalNode node = new()
                    {
                        key = key,
                        delay = delay,
                        time = (int)(Time.realtimeSinceStartup * 1000),
                        args = new[] { context }
                    };
                    
                    delaySignals?.Add(node);
                }
            }
            else
            {
                UInt64 key = GenericSignalKey(signal, srcType, srcKey);
                if (key == 0)
                {
                    return false;
                }

                SignalNode node = new()
                {
                    key = key,
                    delay = delay,
                    time = (int)(Time.realtimeSinceStartup * 1000),
                    args = new[] { context }
                };
                    
                delaySignals?.Add(node);
            }

            return true;
        }

        protected bool TriggerSignal(UInt64 key, Object context)
        {
            var delegates = signalDelegates[key]?.GetInvocationList();
            foreach (Func<System.Object, bool> dele in delegates)
            {
                if (!dele.Invoke(context))
                {
                    return false;
                }
            }

            return true;
        }

        protected virtual UInt64 GenericSignalKey(int signal, int srcType, int srcKey)
        {
            UInt64 key = 0;

            if (signal <= 0 || srcType < 0 || srcKey < 0)
            {
                return key;
            }

            if (srcType >= (1 << 16) || srcKey >= (1 << 16))
            {
                return key;
            }

            key = (uint)signal << 32;
            key |= (uint)srcType << 16;
            key |= (uint)srcKey;

            return key;
        }
    }
}