using System;
using UnityEngine;

namespace UI.NewGameFrame
{
    public class ReadOnlyData<T> where T : class
    {
        public readonly System.Object data;

        public ReadOnlyData(T obj)
        {
            data = obj;
        }

        public static implicit operator ReadOnlyData<T>(T obj)
        {
            return new ReadOnlyData<T>(obj);
        }
    }

    public class ModelRefData<T> : IModelData where T : class
    {
        protected T value;
        public ReadOnlyData<T> Value => value;
        public IModel model { get; protected set; }

        public ModelRefData()
        {
            value = default;
        }

        public ModelRefData(T defValue)
        {
            value = defValue;
        }

        protected ModelDataChanged valueChangedCallback = null;
        public ModelDataChanged callback
        {
            get => valueChangedCallback;
            set => valueChangedCallback = value;
        }

        public virtual void Init(IModel model)
        {
            this.model = model;
        }

        public virtual void UpdateData(System.Object value, bool isLate)
        {
            if (!value.Equals(this.value))
            {
                this.value = (T)value;
            }

            if (!isLate)
            {
                callback?.Invoke(value);
            }
            else
            {
                model?.PushLateUpdate(callback, null);
            }
        }
    }

    public class ModelValueData<T> : IModelData where T : struct
    {
        public T Value { get; protected set; }
        
        public IModel model { get; protected set; }
        protected ModelDataChanged valueChangedCallback = null;

        public ModelDataChanged callback
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

        public virtual void Init(IModel model)
        {
            this.model = model;
        }

        public virtual void UpdateData(System.Object value, bool isLate)
        {
            Value = (T)value;

            if (!isLate)
            {
                callback?.Invoke(value);
            }
            else
            {
                model?.PushLateUpdate(callback, null);
            }
        }
    }
}