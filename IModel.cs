using System;

namespace UI.NewGameFrame
{
    public interface IModel
    {
        bool InitModelData();
        
        bool ReleaseModelData();

        bool IsExistLateUpdate();

        void PushLateUpdate(ModelDataChanged callback, System.Object paramObj);

        void TriggerLateUpdate();

        void ClearLateUpdate();
    }
}