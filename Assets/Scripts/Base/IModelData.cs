using System;

namespace OGMFramework
{
    public delegate void ModelDataChanged();
    
    public interface IModelData
    {
        IModel model { get; }
        
        ModelDataChanged onChanged { get; set; }

        IModelData Init(IModel model);
        
        // void UpdateData(Object value, bool isLate); // 注释原因：值类型与引用类型修改方式不一致，无法兼容使用同一接口
    }
}