using System;
using System.Collections.Generic;

namespace OGMFramework
{
    public class InteractionHelper : IHelper
    {
        protected Dictionary<int, Action<System.Object>> interactions = new();

        public void RegisterInteraction(int actionID, Action<System.Object> callback)
        {
            if (actionID <= 0)
            {
                return;
            }

            if (!interactions.ContainsKey(actionID))
            {
                interactions.Add(actionID, callback);
            }
            else
            {
                interactions[actionID] += callback;
            }
        }

        public void UnRegisterInteraction(int actionID, Action<System.Object> callback)
        {
            if (interactions.ContainsKey(actionID))
            {
                interactions[actionID] -= callback;
                if (interactions[actionID].GetInvocationList().Length == 0)
                {
                    interactions.Remove(actionID);
                }
            }
        }

        public void TriggerInteraction(int actionID, Object paramObj)
        {
            if (interactions == null || actionID <= 0)
            {
                return;
            }
            
            interactions[actionID]?.Invoke(paramObj);
        }
    }
}