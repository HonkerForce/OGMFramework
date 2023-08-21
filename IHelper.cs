using System;

namespace UI.NewGameFrame
{
    public interface IHelper
    {
        void RegisterInteraction(int actionID, Action<System.Object> callback);

        void UnRegisterInteraction(int actionID, Action<System.Object> callback);

        void TriggerInteraction(int actionID, System.Object paramObj);
    }
}