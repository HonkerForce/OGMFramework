using System;
using System.Collections.Generic;

namespace UI.NewGameFrame
{
    // public delegate bool SignalCallBack(Object context);

    public interface ISignalEngine
    {
        void RegisterSignal(int signal, int srcType, int srcKey, Func<System.Object, bool> callback);

        void UnRegisterSignal(int signal, int srcType, int srcKey, Func<System.Object, bool> callBack);

        bool TriggerSignal(int signal, int srcType, int srcKey, Object context);
    }
}