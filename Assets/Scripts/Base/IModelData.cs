using System;

namespace YFramework
{
    public delegate void ModelDataChanged(Object value);
    
    public interface IModelData
    {
        IModel model { get; }
        ModelDataChanged callback { get; set; }

        void Init(IModel model);
        
        void UpdateData(Object value, bool isLate);
    }
}