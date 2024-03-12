namespace OGMFramework
{
    public interface IView
    {
        int viewID { get; set; }
        
        bool InitView(IHelper helper);

        bool IsCreateSuc();
        
        bool IsActive();

        void Show();

        void Hide();

        void Destroy();
    }
}