using System.Collections.Generic;

namespace OGMFramework
{
    public class CallbackItem
    {
        public int id;
        public int index;
        public object args;

        public CallbackItem(int id, int index, object args)
        {
            this.id = id;
            this.index = index;
            this.args = args;
        }
    }
    
    public interface ICallbackHandler
    {
        delegate void CallbackHandle(int id, int index, object args);

        // ICommandEngine commandEngine { get; protected set; }
        
        Dictionary<int, CallbackHandle> handlers { get; set; }
        
        List<CallbackItem> handleCache { get; protected set; }

        void Init();

        void Release();

        void Register(int id, CallbackHandle handler);

        void UnRegister(int id, CallbackHandle handler = null);
    }
}