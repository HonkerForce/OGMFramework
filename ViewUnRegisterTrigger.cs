using UnityEngine;

namespace UI.NewGameFrame
{
    public class ViewUnRegisterTrigger : MonoBehaviour
    {
        private UnRegisterViewProxy unRegisterProxy;

        public void SetUnRegisterProxy(UnRegisterViewProxy proxy)
        {
            unRegisterProxy = proxy;
        }

        private void UnRegister()
        {
            unRegisterProxy?.UnRegister();
        }

        void OnDestroy()
        {
            unRegisterProxy?.UnRegister();
        }
    }
}