using System;

namespace OGMFramework
{
    // public delegate bool SignalCallBack(Object context);

    public interface ISignalEngine
    {
        void RegisterSignal(int signal, Func<System.Object, bool> callback);

        void UnRegisterSignal(int signal, Func<System.Object, bool> callBack);

        bool TriggerSignal(int signal, System.Object context);
    }
}