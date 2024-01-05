namespace YFramework
{
    public interface IManager
    {
        bool InitController();

        bool InitSignalEngine();

        bool ReleaseController();

        bool ReleaseSignalEngine();

        void UpdateProcess();

        void FixedUpdateProcess();

        void LateUpdateProcess();
    }
}