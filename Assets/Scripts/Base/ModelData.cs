using System;
using UnityEngine;

namespace OGMFramework
{
    public class ReadOnlyData<T> where T : class
    {
        public readonly T data;

        public ReadOnlyData(T obj)
        {
            data = obj;
        }

        public static implicit operator ReadOnlyData<T>(T obj)
        {
            return new ReadOnlyData<T>(obj);
        }
    }

    // public class ModelRefData<T> : IModelData where T : class, new()
    // {
    //     protected T value;
    //     public T Value => value;
    //     public IModel model { get; protected set; }
    //
    //     public ModelRefData()
    //     {
    //         value = default;
    //     }
    //
    //     public ModelRefData(in T defValue)
    //     {
    //         value = defValue;
    //     }
    //
    //     protected ModelDataChanged valueChangedCallback = null;
    //     public ModelDataChanged callback
    //     {
    //         get => valueChangedCallback;
    //         set => valueChangedCallback = value;
    //     }
    //
    //     public virtual void Init(IModel model)
    //     {
    //         this.model = model;
    //     }
    //
    //     public virtual void UpdateData(System.Object value, bool isLate)
    //     {
    //         if (!value.Equals(this.value))
    //         {
    //             this.value = (T)value;
    //         }
    //     
    //         if (!isLate)
    //         {
    //             callback?.Invoke();
    //         }
    //         else
    //         {
    //             model?.PushLateUpdate(callback);
    //         }
    //     }
    // }

    public class ModelValueData<T> : IModelData where T : struct
    {
        public T Value { get; protected set; }
        
        public IModel model { get; protected set; }
        protected ModelDataChanged valueChangedCallback = null;

        public ModelDataChanged onChanged
        {
            get => valueChangedCallback;
            set => valueChangedCallback = value;
        }

        public ModelValueData()
        {
            Value = default;
        }

        public ModelValueData(T value)
        {
            Value = value;
        }

        public virtual IModelData Init(IModel model)
        {
            this.model = model;
            return this;
        }

        public virtual void UpdateData(System.Object value, bool isLate)
        {
            Value = (T)value;

            if (!isLate)
            {
                onChanged?.Invoke();
            }
            else
            {
                model?.PushLateUpdate(onChanged);
            }
        }
    }
}