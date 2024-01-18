using System;

namespace OGMFramework
{
    public interface IEventEngine
    {
        void RegisterEvent(int eventID, int srcType, int srcKey, Func<System.Object, bool> callback);

        void RegisterEvent(int eventID, int srcType, int srcKey, IEventHandler handler);

        void UnRegisterEvent(int eventID, int srcType, int srcKey, Func<System.Object, bool> callBack);

        void UnRegisterEvent(int eventID, int srcType, int srcKey, IEventHandler handler);

        bool TriggerEvent(int eventID, int srcType, int srcKey, System.Object context);

        void TriggerDelayEvent(int eventID, int srcType, int srcKey, int delay, System.Object context);
    }
}