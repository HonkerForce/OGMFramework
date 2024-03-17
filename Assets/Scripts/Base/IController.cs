using System;

namespace OGMFramework
{
    // public delegate void ViewCreatedCallback(IController controller, IView createdView);

    public interface IController
    {
        bool Init();

        bool Release();

        LoadViewDataProxy ControlView(int viewID, IView view, bool isRoot, string parentPath = "");

        void DropView(int viewID, bool isRoot);

        bool IsExistView(int viewID);

        bool IsViewShowed(int ViewID);

        void ShowView(int viewID);

        void HideView(int viewID);

        void CallbackView(int viewID, Action<IController, IView> callback);

        // void SetModelData<T>(string dataName, T dataValue, bool isLate);

        void LateUpdateData();

    }
}