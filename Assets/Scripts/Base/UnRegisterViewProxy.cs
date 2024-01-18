namespace OGMFramework
{
    public class UnRegisterViewProxy
    {
        private IController controller;
        private View view;
        private bool isRoot;

        public UnRegisterViewProxy(IController controller, View view, bool isRoot)
        {
            this.controller = controller;
            this.view = view;
            this.isRoot = isRoot;
        }

        public void UnRegister()
        {
            controller?.DropView(view.viewID, isRoot);
        }

        public void UnRegisterViewWhenViewDestroyed()
        {
            if (view == null)
            {
                return;
            }

            ViewUnRegisterTrigger trigger = view.gameObject.GetComponent<ViewUnRegisterTrigger>();
            if (trigger == null)
            {
                trigger = view.gameObject?.AddComponent<ViewUnRegisterTrigger>();
            }
            trigger?.SetUnRegisterProxy(this);
        }
    }
}