namespace OGMFramework
{
    public class TestModel : Model
    {
        // public ModelRefData<object> refData;
        public ModelValueData<int> nData = new();
        
        public override bool Init(ISignalEngine signalEngine)
        {
            this.signalEngine = signalEngine;
            // refData.Init(this);
            nData.Init(this).onChanged += () =>
            {
                
            };

            return true;
        }

        public override bool ReleaseModelData()
        {
            return true;
        }
    }
}