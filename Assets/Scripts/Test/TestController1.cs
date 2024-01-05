using System;

namespace YFramework
{
	public class TestController1 : IController
	{
		public TestController1(ISignalEngine engine)
		{
			
		}

		public bool InitSignal()
		{
			throw new NotImplementedException();
		}

		public bool InitModel()
		{
			throw new NotImplementedException();
		}

		public bool InitInteraction()
		{
			throw new NotImplementedException();
		}

		public bool ReleaseSignal()
		{
			throw new NotImplementedException();
		}

		public bool ReleaseModel()
		{
			throw new NotImplementedException();
		}

		public bool ReleaseInteraction()
		{
			throw new NotImplementedException();
		}

		public UnRegisterViewProxy ControlView(int viewID, IView view, bool isRoot, string parentPath = "")
		{
			throw new NotImplementedException();
		}

		public void DropView(int viewID, bool isRoot)
		{
			throw new NotImplementedException();
		}

		public bool IsExistView(int viewID)
		{
			throw new NotImplementedException();
		}

		public bool IsViewShowed(int ViewID)
		{
			throw new NotImplementedException();
		}

		public void ShowView(int viewID)
		{
			throw new NotImplementedException();
		}

		public void HideView(int viewID)
		{
			throw new NotImplementedException();
		}

		public void CallbackView(int viewID, Action<IController, IView> callback)
		{
			throw new NotImplementedException();
		}

		public void LateUpdateData()
		{
			throw new NotImplementedException();
		}
	}
}