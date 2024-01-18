using System.Collections.Generic;
using Unity.VisualScripting;

namespace OGMFramework
{
    public class CommandHandler : ICallbackHandler
    {
        Dictionary<int, ICallbackHandler.CallbackHandle> ICallbackHandler.handlers { get; set; } = new();

        List<CallbackItem> ICallbackHandler.handleCache { get; set; } = new();

        public void Init()
        {
            // Register(int id, ICallbackHandler.CallbackHandle handler)
        }

        public void Release()
        {
            var baseThis = this as ICallbackHandler;
            baseThis?.handleCache.Clear();

            var pair = baseThis.handlers.GetEnumerator();
            while (pair.MoveNext())
            {
                UnRegister(pair.Current.Key);
            }
            baseThis.handlers.Clear();
        }

        public void Register(int id, ICallbackHandler.CallbackHandle handler)
        {
            throw new System.NotImplementedException();
        }

        public void UnRegister(int id, ICallbackHandler.CallbackHandle handler = null)
        {
            throw new System.NotImplementedException();
        }
    }
}