using System;
using System.Collections.Generic;

namespace OGMFramework
{
    public abstract class Manager<T> : IManager
        where T : IManager, new()
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }
        
                return instance;
            }
        }
        
        protected Manager() { }
        
        protected Dictionary<int, IController> controllers = new();

        protected abstract ICommandEngine commandEngine { get; }

        public virtual bool Init()
        {
            return InitController() & InitCommandEngine();
        }

        public virtual bool Release()
        {
            return ReleaseController() & ReleaseCommandEngine();
        }

        protected abstract bool InitController();
        
        protected abstract bool InitCommandEngine();
        
        protected abstract bool ReleaseController();
        
        protected abstract bool ReleaseCommandEngine();
    }
}