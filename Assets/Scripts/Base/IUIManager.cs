using System;

namespace OGMFramework
{
    public interface IUIManager : IManager
    {
        bool InitConfig();

        bool IsExistWindow(WindowModel winModel);

        void ChangeSceneHideViews();

        void OpenWindow(WindowModel winModel, Action<IController, IView> callback);

        void CloseWindow(WindowModel winModel);
    }
}