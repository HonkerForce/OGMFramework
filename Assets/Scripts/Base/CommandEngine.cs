using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OGMFramework
{
    public class CommandEngine : ICommandEngine
    {
        protected Dictionary<int, Func<int, int, System.Object, bool>> commandHandlers = new();
        
        public virtual void RegisterCommand(int command, Func<int, int, System.Object, bool> callback)
        {
            if (command <= 0)
            {
                Debug.LogError(string.Format($"注册Command失败，command={command}"));
                return;
            }

            if (commandHandlers.ContainsKey(command))
            {
                commandHandlers[command] += callback;
            }
            else
            {
                commandHandlers.Add(command, callback);
            }
        }

        public virtual void UnRegisterCommand(int command, Func<int, int, System.Object, bool> callback)
        {
            if (command <= 0)
            {
                Debug.LogError(string.Format($"注册Command失败，command={command}"));
                return;
            }

            if (!commandHandlers.ContainsKey(command))
            {
                Debug.LogError(string.Format($"未知信号请求注销，注销信号失败，command={command}"));
                return;
            }

            commandHandlers[command] -= callback;
            if (commandHandlers[command].GetInvocationList().Length == 0)
            {
                commandHandlers.Remove(command);
            }
        }

        public virtual bool ExecuteCommand(int command, int index, System.Object context)
        {
            if (command <= 0 || index < 0)
            {
                return false;
            }

            if (!commandHandlers.ContainsKey(command))
            {
                Debug.LogError(string.Format($"未知信号被触发，command={command}"));
                return false;
            }

            var delegates = commandHandlers[command]?.GetInvocationList();
            if (delegates == null || delegates.Length == 0)
            {
                return false;
            }
            foreach (Func<int, int, System.Object, bool> dele in delegates)
            {
                if (!dele.Invoke(command, index, context))
                {
                    return false;
                }
            }
            return true;
        }
    }
}