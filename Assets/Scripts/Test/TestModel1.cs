namespace OGMFramework
{
    public class TestModel1 : Model
    {
        public ModelRefData<object> refData;

        public override bool InitModelData()
        {
            refData.Init(this);

            return true;
        }

        public override bool ReleaseModelData()
        {

            return true;
        }
    }
}