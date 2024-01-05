using UnityEngine;

namespace YFramework
{
    public abstract class View : MonoBehaviour, IView
    {
        public abstract int viewID { get; set; }

        public virtual bool InitHelper(IHelper helper)
        {
            return true;
        }
        
        public virtual bool IsCreateSuc()
        {
            return true;
        }

        public virtual bool IsActive()
        {
            return gameObject.activeInHierarchy;
        }
        
        public abstract void Show();

        public abstract void Hide();

        public abstract void Destroy();
        
        
        protected abstract void Awake();

        protected virtual void Start()
        {
            
        }

        protected abstract void OnEnable();

        protected virtual void FixedUpdate()
        {
            
        }

        protected virtual void Update()
        {
            
        }

        protected virtual void LateUpdate()
        {
            
        }

        protected abstract void OnDisable();

        protected abstract void OnDestroy();
    }
}