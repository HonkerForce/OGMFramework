using System.Collections.Generic;
using UnityEngine;

namespace OGMFramework
{
    public abstract class ModelBase : IModel
    {
        protected ISignalEngine signalEngine;

        protected HashSet<ModelDataChanged> lateCallbacks = new();

        public abstract bool Init(ISignalEngine signalEngine);
        
        public abstract bool ReleaseModelData();
        
        // public abstract void RegisterDataUpdate(string dataName, ModelDataChanged callback);
        //
        // public abstract void UnRegisterDataUpdate(string dataName, ModelDataChanged callback);
        //
        // public abstract void UpdateData<T>(string dataName, T dataValue, bool isLateUpdate);
        
        public abstract bool IsExistLateUpdate();
        
        public abstract void PushLateUpdate(ModelDataChanged callback);

        public abstract void TriggerLateUpdate();
        
        public abstract void ClearLateUpdate();
    }
}