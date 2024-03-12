using System;

namespace OGMFramework
{
    public interface IModel
    {
        bool Init(ISignalEngine signalEngine);
        
        bool ReleaseModelData();

        bool IsExistLateUpdate();

        void PushLateUpdate(ModelDataChanged callback);

        void TriggerLateUpdate();

        void ClearLateUpdate();
    }
}