using System;

namespace OGMFramework
{
    public interface ICommandEngine
    {
        void RegisterCommand(int command, Func<int, int, System.Object, bool> callback);

        void UnRegisterCommand(int command, Func<int,int, System.Object, bool> callback = null);

        bool ExecuteCommand(int command, int index, System.Object context);
    }
}