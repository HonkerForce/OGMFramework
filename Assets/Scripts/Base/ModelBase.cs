using System.Collections.Generic;
using UnityEngine;

namespace YFramework
{
    public abstract class ModelBase : IModel
    {
        protected struct LateUpdateInfo
        {
            public ModelDataChanged callback;
            public System.Object paramObj;
        }
        
        // protected Dictionary<string, IModelData> modelDatas = new Dictionary<string, IModelData>();

        protected HashSet<LateUpdateInfo> lateCallbacks = new();

        public abstract bool InitModelData();
        
        public abstract bool ReleaseModelData();
        
        // public abstract void RegisterDataUpdate(string dataName, ModelDataChanged callback);
        //
        // public abstract void UnRegisterDataUpdate(string dataName, ModelDataChanged callback);
        //
        // public abstract void UpdateData<T>(string dataName, T dataValue, bool isLateUpdate);
        
        public abstract bool IsExistLateUpdate();
        
        public abstract void PushLateUpdate(ModelDataChanged callback, object paramObj);

        public abstract void TriggerLateUpdate();
        
        public abstract void ClearLateUpdate();
    }
}