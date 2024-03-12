using UnityEngine;

namespace OGMFramework
{
    public class ViewUnRegisterTrigger : MonoBehaviour
    {
        private LoadViewDataProxy _loadDataProxy;

        public void SetLoadViewDataProxy(LoadViewDataProxy dataProxy)
        {
            _loadDataProxy = dataProxy;
        }

        void OnEnable()
        {
            _loadDataProxy?.LoadViewData();
        }
    }
}