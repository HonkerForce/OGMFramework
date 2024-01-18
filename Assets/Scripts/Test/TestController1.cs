using System;

namespace OGMFramework
{
	public class TestController1 : Controller<TestModel1>
	{
		public TestController1(ICommandEngine engine) : base(engine)
		{
			
		}

		public override bool InitSignal()
		{
			throw new NotImplementedException();
		}

		public override bool InitModel()
		{
			throw new NotImplementedException();
		}

		public override bool InitInteraction()
		{
			throw new NotImplementedException();
		}

		public override bool ReleaseSignal()
		{
			throw new NotImplementedException();
		}

		public override bool ReleaseModel()
		{
			throw new NotImplementedException();
		}

		public override bool ReleaseInteraction()
		{
			throw new NotImplementedException();
		}

		public override UnRegisterViewProxy ControlView(int viewID, IView view, bool isRoot, string parentPath = "")
		{
			throw new NotImplementedException();
		}

		public override void DropView(int viewID, bool isRoot)
		{
			throw new NotImplementedException();
		}

		public override bool IsExistView(int viewID)
		{
			throw new NotImplementedException();
		}

		public override bool IsViewShowed(int ViewID)
		{
			throw new NotImplementedException();
		}

		public override void ShowView(int viewID)
		{
			throw new NotImplementedException();
		}

		public override void HideView(int viewID)
		{
			throw new NotImplementedException();
		}

		public override void CallbackView(int viewID, Action<IController, IView> callback)
		{
			throw new NotImplementedException();
		}

		public override void LateUpdateData()
		{
			throw new NotImplementedException();
		}
	}
}