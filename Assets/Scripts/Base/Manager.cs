using System;
using System.Collections.Generic;

namespace OGMFramework
{
    public abstract class Manager : IManager
    {
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

        public abstract bool InitController();
        
        public abstract bool InitCommandEngine();
        
        public abstract bool ReleaseController();
        
        public abstract bool ReleaseCommandEngine();

    }
}