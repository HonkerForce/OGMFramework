namespace OGMFramework
{
    public interface IManager
    {
        bool InitController();

        bool InitCommandEngine();

        bool ReleaseController();

        bool ReleaseCommandEngine();
    }
}