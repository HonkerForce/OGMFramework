using System;
using System.Collections.Generic;

namespace OGMFramework
{
    public abstract class Manager : IManager
    {
        protected Dictionary<int, IController> controllers = new();

        protected abstract ICommandEngine commandEngine { get; }

        public abstract bool InitController();
        
        public abstract bool InitCommandEngine();
        
        public abstract bool ReleaseController();
        
        public abstract bool ReleaseCommandEngine();

    }
}