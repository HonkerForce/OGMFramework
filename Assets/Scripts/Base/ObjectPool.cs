using System;
using System.Collections.Generic;
using UnityEngine;

namespace OGMFramework
{
    public class PoolData<T> where T : new()
    {
        public T obj = new();
        public bool inPool = true;
    }
    
    public class ObjectPool<T> where T : new()
    {
        public delegate void ObjectPoolCallback(PoolData<T> obj);
        
        private List<PoolData<T>> pool;
        private int poolSize = 0;
        public int Capacity => poolSize;
        private ObjectPoolCallback getHandler = null;
        public ObjectPoolCallback OnGet => getHandler;
        private ObjectPoolCallback createHandler = null;
        public ObjectPoolCallback OnCreate => createHandler;
        private ObjectPoolCallback recycleHandler = null;
        public ObjectPoolCallback OnRecycle => recycleHandler;
        private ObjectPoolCallback deleteHandler = null;
        public ObjectPoolCallback OnDelete => deleteHandler;

        public ObjectPool(int initSize, ObjectPoolCallback initCreate = null, ObjectPoolCallback initGet = null,
            ObjectPoolCallback initRecycle = null, ObjectPoolCallback initDelete = null)
        {
            poolSize = initSize;
            pool ??= new List<PoolData<T>>(poolSize);

            if (initCreate != null)
            {
                createHandler += initCreate;
            }

            if (initGet != null)
            {
                getHandler += initGet;
            }

            if (initRecycle != null)
            {
                recycleHandler += initRecycle;
            }

            if (initDelete != null)
            {
                deleteHandler += initDelete;
            }
        }

        public T Get()
        {
            var retData = pool.Find(data => data.inPool == true);
            if (retData == null)
            {
                retData = new();
                createHandler(retData);
                pool.Add(retData);
                retData.inPool = false;
                return retData.obj;
            }
            getHandler(retData);
            retData.inPool = false;
            return retData.obj;
        }

        public bool Recycle(T obj)
        {
            var retData = pool.Find(data => data.obj.Equals(obj));
            if (retData == null)
            {
                return false;
            }

            recycleHandler(retData);
            retData.inPool = true;
            return true;
        }

        public bool ResetSize(int size)
        {
            if (size >= poolSize)
            {
                return false;
            }

            int num = poolSize - size;
            for (int i = pool.Count - 1; i >= 0 && num > 0; i--)
            {
                if (pool[i].inPool)
                {
                    deleteHandler(pool[i]);
                    pool.RemoveAt(i);
                    num--;
                }
            }

            Debug.Assert(num > 0, $"重置对象池容量错误，池中有{poolSize}个对象，还需要删除{num}个对象！");
            return true;
        }
    }
}