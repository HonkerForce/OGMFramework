using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.NewGameFrame
{
    
    public abstract class Controller<ModelClass> : IController where ModelClass : IModel
    {
        protected Dictionary<int, IView> allViews = new Dictionary<int, IView>();

        protected int rootViewID = 0;

        protected ISignalEngine signalEngine;

        protected abstract IHelper interactionHelper { get; }

        protected abstract ModelClass model { get; }

        public Controller(ISignalEngine signalEngine)
        {
            this.signalEngine = signalEngine;
        }

        public abstract bool InitSignal();
        
        public abstract bool InitModel();
        
        public abstract bool InitInteraction();
        
        public abstract bool ReleaseSignal();
        
        public abstract bool ReleaseModel();
        
        public abstract bool ReleaseInteraction();

        public virtual UnRegisterViewProxy ControlView(int viewID, IView view, bool isRoot, string parentPath = "")
        {
            if (viewID <= 0 || view == null)
            {
                return null;
            }

            view.viewID = viewID;
            view.InitHelper(interactionHelper);
            

            if (!allViews.ContainsKey(viewID))
            {
                ((View)view).gameObject.SetActive(false);
                allViews.Add(viewID, view);

                if (isRoot)
                {
                    rootViewID = viewID;
                }
                else if (rootViewID > 0)
                {
                    View rootView = allViews[rootViewID] as View;
                    View curView = view as View;
                    if (!String.IsNullOrEmpty(parentPath))
                    {
                        Transform parentTrans = rootView.transform.Find(parentPath);
                        if (parentTrans != null)
                        {
                            curView.transform.SetParent(parentTrans);
                        }
                        else
                        {
                            Debug.LogError("View父节点名称配置有误，未找到该父节点，viewID:" + viewID);
                        }
                    }
                    else
                    {
                        curView.transform.SetParent(rootView.transform);
                    }
                }
                else
                {
                    Debug.LogError("在父节点未创建前开始创建子节点，viewID:" + viewID);
                }
            }
            else
            {
                Debug.LogError("重复添加控制窗口，viewID：" + viewID);
            }
            
            // 重置view排序层和特效显示
            (view as UIView)?.ResetCanvasSorting(allViews.Count);
            (view as UIView)?.ResetOrderLayerEventEx();

            return new UnRegisterViewProxy(this as IController, view as View, isRoot);
        }

        public virtual void DropView(int viewID, bool isRoot)
        {
            if (viewID <= 0)
            {
                return;
            }

            if (isRoot)
            {
                foreach (var view in allViews)
                {
                    view.Value.Destroy();
                }

                allViews.Clear();
                rootViewID = 0;
                return;
            }
            
            if (allViews.ContainsKey(viewID))
            {
                allViews[viewID].Destroy();
                allViews.Remove(viewID);
            }
        }

        public virtual bool IsExistView(int viewID)
        {
            return allViews.ContainsKey(viewID);
        }

        public virtual bool IsViewShowed(int ViewID)
        {
            if (!allViews.ContainsKey(ViewID))
            {
                return false;
            }

            return allViews[ViewID].IsActive();
        }

        public virtual void ShowView(int viewID)
        {
            allViews[rootViewID]?.Show();
            if (viewID != rootViewID)
            {
                allViews[viewID]?.Show();
            }
        }

        public virtual void HideView(int viewID)
        {
            if (viewID == 0 || viewID == rootViewID)
            {
                foreach (var view in allViews)
                {
                    if (!view.Value.IsActive())
                    {
                        continue;
                    }
                    
                    view.Value.Hide();
                }
            }
            else
            {
                if (allViews.TryGetValue(viewID, out var view) && view.IsActive())
                {
                    allViews[viewID].Hide();
                }
            }
        }

        public virtual void CallbackView(int viewID, Action<IController, IView> callback)
        {
            IView view = null;
            if (!allViews.TryGetValue(viewID, out view))
            {
                return;
            }
            
            callback?.Invoke(this, view);
        }
        
        // public virtual void SetModelData<T>(string dataName, T dataValue, bool isLate)
        // {
        //     model.UpdateData<T>(dataName, dataValue, isLate);
        // }

        public virtual void LateUpdateData()
        {
            if (model.IsExistLateUpdate())
            {
                model.TriggerLateUpdate();
            }
        }
    }
}