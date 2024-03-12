using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OGMFramework
{
    public abstract class Model : ModelBase
    {
        // public sealed override void RegisterDataUpdate(string dataName, ModelDataChanged callback)
        // {
        //     if (modelDatas.ContainsKey(dataName))
        //     {
        //         modelDatas[dataName].callback += callback;
        //     }
        // }
        //
        // public sealed override void UnRegisterDataUpdate(string dataName, ModelDataChanged callback)
        // {
        //     if (modelDatas.ContainsKey(dataName))
        //     {
        //         modelDatas[dataName].callback -= callback;
        //     }
        // }
        //
        // public sealed override void UpdateData<T>(string dataName, T dataValue, bool isLateUpdate)
        // {
        //     IModelData modelData = null;
        //     if (modelDatas.TryGetValue(dataName, out modelData))
        //     {
        //         modelData.TriggerUpdate(dataValue, isLateUpdate);
        //         
        //         if (isLateUpdate)
        //         {
        //             LateUpdateInfo info;
        //             info.callback = modelData.callback;
        //             info.paramObj = dataValue;
        //
        //             lateCallbacks?.Add(info);
        //         }
        //     }
        // }

        public sealed override void PushLateUpdate(ModelDataChanged callback)
        {
            lateCallbacks?.Add(callback);
        }

        public sealed override bool IsExistLateUpdate()
        {
            if (lateCallbacks == null)
            {
                return false;
            }

            return lateCallbacks.Count > 0;
        }

        public sealed override void TriggerLateUpdate()
        {
            foreach (var callback in lateCallbacks)
            {
                callback?.Invoke();
            }

            lateCallbacks.Clear();
        }
        
        public sealed override void ClearLateUpdate()
        {
            lateCallbacks.Clear();
        }
    }
}