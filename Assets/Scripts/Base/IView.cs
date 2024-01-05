namespace YFramework
{
    public interface IView
    {
        int viewID { get; set; }
        
        bool InitHelper(IHelper helper);

        bool IsCreateSuc();
        
        bool IsActive();

        void Show();

        void Hide();

        void Destroy();
    }
}