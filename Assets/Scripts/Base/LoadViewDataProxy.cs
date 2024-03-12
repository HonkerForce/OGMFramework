namespace OGMFramework
{
    public class LoadViewDataProxy
    {
        private IHelper helper;
        private View view;

        public LoadViewDataProxy(IHelper helper, View view)
        {
            this.helper = helper;
            this.view = view;
        }

        public void LoadViewData()
        {
            helper.TriggerInteraction((int)Common_Interaction.LOAD_VIEW_DATA, view.viewID);
        }

        public void LoadViewDataWhenViewActive()
        {
            if (view == null)
            {
                return;
            }

            ViewUnRegisterTrigger trigger = view.gameObject.GetComponent<ViewUnRegisterTrigger>();
            if (trigger == null)
            {
                trigger = view.gameObject?.AddComponent<ViewUnRegisterTrigger>();
                trigger?.SetLoadViewDataProxy(this);
            }
        }
    }
}