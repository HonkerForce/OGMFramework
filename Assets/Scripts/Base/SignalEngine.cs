using System;
using System.Collections.Generic;
using UnityEngine;

namespace OGMFramework
{
    public class SignalEngine : ISignalEngine
    {
        public enum SIGNAL_SRC_TYPE
        {
            SRC_TYPE_NONE = 0,
            SRC_TYPE_SYSTEM,
            SRC_TYPE_MAX
        }
        
        protected Dictionary<int, Func<System.Object, bool>> signalDelegates = new();

        protected struct SignalNode
        {
            public UInt64 key;
            public int delay;
            public int time;
            public object[] args;
        }

        protected List<SignalNode> delaySignals = new();
        
        public virtual void RegisterSignal(int signal, Func<System.Object, bool> callback)
        {
            if (signalDelegates.ContainsKey(signal))
            {
                signalDelegates[signal] += callback;
            }
            else
            {
                signalDelegates.Add(signal, callback);
            }
        }

        public virtual void UnRegisterSignal(int signal, Func<System.Object, bool> callBack)
        {
            if (!signalDelegates.ContainsKey(signal))
            {
                Debug.LogError(string.Format($"未知信号请求注销，注销信号失败，signal={signal}"));
                return;
            }

            signalDelegates[signal] -= callBack;
            if (signalDelegates[signal].GetInvocationList().Length == 0)
            {
                signalDelegates.Remove(signal);
            }
        }

        public virtual bool TriggerSignal(int signal, System.Object context)
        {
            if (signal <= 0)
            {
                return false;
            }

            if (!signalDelegates.ContainsKey(signal))
            {
                Debug.LogError(string.Format($"未知信号被触发，signal={signal}"));
                return false;
            }

            var delegates = signalDelegates[signal]?.GetInvocationList();
            foreach (Func<System.Object, bool> dele in delegates)
            {
                if (!dele.Invoke(context))
                {
                    return false;
                }
            }

            return true;
        }
    }
}