namespace OGMFramework
{
    public class TestModel1 : Model
    {
        // public ModelRefData<object> refData;
        public ModelValueData<float> fData = new();

        public override bool Init(ISignalEngine signalEngine)
        {
            this.signalEngine = signalEngine;
            // refData.Init(this);
            fData.Init(this).onChanged += () =>
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