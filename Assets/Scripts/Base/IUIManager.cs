using System;

namespace OGMFramework
{
    public interface IUIManager : IManager
    {
        bool InitConfig();

        bool IsExistWindow(WindowModel winModel);

        void ChangeSceneHideViews();

        void AsyncCreateWindow(WindowModel winModel, Action<IController, IView> callback);
    }
}