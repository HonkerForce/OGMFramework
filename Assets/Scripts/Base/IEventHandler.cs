namespace OGMFramework
{
    public interface IEventHandler
    {
        bool ExecuteEvent(int eventID, int srcType, int srcKey, object args);
    }
}