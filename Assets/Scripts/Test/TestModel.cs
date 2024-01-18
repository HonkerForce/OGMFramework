namespace OGMFramework
{
    public class TestModel : Model
    {
        public ModelRefData<object> refData;
        public ModelValueData<int> nData;
        
        public override bool InitModelData()
        {
            refData.Init(this);
            nData.Init(this);

            return true;
        }

        public override bool ReleaseModelData()
        {
            return true;
        }
    }
}