using System;
using rkt.Common;

namespace UI.NewGameFrame
{
    public interface IUIManager : IManager
    {
        bool InitConfig();

        bool IsExistWindow(WindowModel winModel);

        void ChangeSceneHideViews();

        void AsyncCreateWindow(WindowModel winModel, Action<IController, IView> callback);
    }
}