using System;
using System.Collections.Generic;
using rkt;

namespace UI.NewGameFrame
{
    public abstract class Manager : IManager
    {
        protected Dictionary<int, IController> controllers = new();

        protected abstract ISignalEngine signalEngine { get; }

        public abstract bool InitController();
        
        public abstract bool InitSignalEngine();
        
        public abstract bool ReleaseController();
        
        public abstract bool ReleaseSignalEngine();
        
        public abstract void UpdateProcess();
        
        public abstract void FixedUpdateProcess();
        
        public abstract void LateUpdateProcess();
        
    }
}