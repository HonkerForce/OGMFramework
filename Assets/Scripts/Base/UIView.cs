using System;
using System.Collections.Generic;
using UnityEngine;

namespace OGMFramework
{
    public abstract class UIView : View
    {
        protected IHelper helper;

        protected WindowModel curWinID = WindowModel.None;

        protected Canvas m_Canvas = null;

        // public abstract Window_Sorting WinSorting { get; set; }
        //特效附加层
        protected const int m_effectAppendOrder = 4;

        public sealed override int viewID
        {
            get => (int)curWinID;
            set
            {
                WindowModel.TryParse(value.ToString(), out curWinID);
            }
        }

        public override bool InitView(IHelper helper)
        {
            if (helper == null)
            {
                return false;
            }

            this.helper = helper;
            return true;
        }

        public override bool IsCreateSuc()
        {
            return base.IsCreateSuc() &&
                   helper != null &&
                   curWinID != WindowModel.None; //&& 
            // WinSorting != Window_Sorting.None;
        }

        public sealed override void Show()
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
        }

        public sealed override void Hide()
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
        }

        public sealed override void Destroy()
        {
            Destroy(gameObject);
        }

        #region 重置 canvas的sorting值 
        public void ResetCanvasSorting(int idx)
        {
            //只有定义为window层才需要自动排序
            // if (WinSorting != Window_Sorting.Window) return;
    
            if (m_Canvas == null)
            {
                m_Canvas = gameObject.GetComponent<Canvas>();
            }
    
            if (m_Canvas == null) return;
    
            if (m_Canvas.overrideSorting)
            {
                // m_Canvas.sortingOrder = (int)Window_Sorting.Window + (40 * idx);
                //Debug.Log(gameObject.name + "   " + m_Canvas.sortingOrder);
            }

            ResetOrderLayerEvent();
        }
    
        public void ResetOrderLayerEvent()
        {
            if (m_Canvas == null)
            {
                m_Canvas = gameObject.GetComponent<Canvas>();
            }
    
            if (m_Canvas == null) return;
    
            #region 重置当前已经加载的粒子层级
            Canvas[] r = GetComponentsInChildren<Canvas>(true);
    
            Canvas tmp = null;
    
            for (int i = 0; i < r.Length; i++)
            {
                tmp = r[i];
                //特殊处理：新手引导手指，需要加入到CommonWindow层，而手指又需要超过Guider层，固如果设置了超过一定层将不重新设置/
                if(tmp.sortingOrder < 1000)
                    tmp.sortingOrder = m_Canvas.sortingOrder + (m_effectAppendOrder * i + 1);
            }
    
            // IOrderInLayerListener[] tmpOrderInLayer = GetComponentsInChildren<IOrderInLayerListener>(true);
            //
            // IOrderInLayerListener tmpO = null;
    
            // for (int i = 0; i < tmpOrderInLayer.Length; i++)
            // {
            //     tmpO = tmpOrderInLayer[i];
            //
            //     tmpO.OrderInLayerChange();
            // }
    
            #endregion
        }
        #endregion
        
        #region  子类异步创建完成处理

        public void ResetOrderLayerEventEx()
        {
            Canvas parent = transform.parent.GetComponentInParent<Canvas>();
            if (parent == null)
            {
                return;
            }

            #region 重置当前已经加载的粒子层级
            Canvas[] r = GetComponentsInChildren<Canvas>(true);

            Canvas tmp = null;

            for (int i = 0; i < r.Length; i++)
            {
                tmp = r[i];
                //特殊处理：新手引导手指，需要加入到CommonWindow层，而手指又需要超过Guider层，固如果设置了超过一定层将不重新设置/
                if (tmp.sortingOrder < 1000)
                    tmp.sortingOrder = parent.sortingOrder + (m_effectAppendOrder * i + m_effectAppendOrder);
            }

            // IOrderInLayerListener[] tmpOrderInLayer = GetComponentsInChildren<IOrderInLayerListener>(true);
            //
            // IOrderInLayerListener tmpO = null;
            //
            // for (int i = 0; i < tmpOrderInLayer.Length; i++)
            // {
            //     tmpO = tmpOrderInLayer[i];
            //
            //     tmpO.OrderInLayerChange();
            // }

            #endregion
        }

        #endregion
    }
}