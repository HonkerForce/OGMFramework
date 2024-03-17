using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace OGMFramework
{
    public class UIManager : Manager<UIManager>, IUIManager, IEventHandler
    {
        public enum UI_COMMAND
        {
            NONE,
            COMMAND1,
            COMMAND2,
            COMMAND3,
            MAX
        }

        [System.Serializable]
        public class WindowModelConfig
        {
            /// <summary>
            /// 自身Model
            /// </summary>
            public WindowModel winModel;

            public GameObject prefab;
            /// <summary>
            /// 关闭场景是否隐藏
            /// </summary>
            public bool changeSceneHide = true;

            /// <summary>
            /// 子类Model
            /// </summary>
            public List<SonWindowNode> subNode;
        }

        [System.Serializable]
        public class SonWindowNode
        {
            public WindowModel winModel;
            public GameObject prefab;
            public string parentPath;
        }

        protected override ICommandEngine commandEngine { get; } = new CommandEngine();

        private const string uiConfigPath = "Assets/GResources/Config/UI/WindowAsyncCreateConfig.asset";

        private Dictionary<WindowModel, WindowModelConfig> winConfigs = new();
        private GameObject winContainer = null;

        public override bool Init()
        {
            GameManager.RegisterMonoEvent(Update, FixedUpdate, LateUpdate);
            
            return base.Init() & InitConfig();
        }

        public bool InitConfig()
        {
            Resources.LoadAsync(uiConfigPath);
            // AssetManager.LoadAsset<WindowAsyncCreateConfig>(uiConfigPath, modelConfig =>
            // {
            //     if (winConfigs != null)
            //     {
            //         for (int i = 0; i < modelConfig.WindowsModelConfig.Count; i++)
            //         {
            //             WindowModelConfig t = modelConfig.WindowsModelConfig[i];
            //
            //             if (t.winModel != WindowModel.None)
            //             {
            //                 if (winConfigs.ContainsKey(t.winModel))
            //                 {
            //                     TRACE.ErrorLn("UIManager.InitData error key=" + t.winModel);
            //                     continue;
            //                 }
            //
            //                 winConfigs.Add(t.winModel, t);
            //             }
            //
            //             for (int j = 0; j < t.subNode.Count; j++)
            //             {
            //                 WindowModel enSubWinModel = t.subNode[j].winModel;
            //                 if (enSubWinModel != WindowModel.None)
            //                 {
            //                     if (winConfigs.ContainsKey(enSubWinModel))
            //                     {
            //                         TRACE.ErrorLn(string.Format("UIManager.InitData sub error key={0} winModel={1}", enSubWinModel, t.winModel));
            //                         continue;
            //                     }
            //                     winConfigs.Add(enSubWinModel, t);
            //                 }
            //             }
            //         }
            //     }
            //     else
            //     {
            //         TRACE.ErrorLn("==================Can not Find UI UIAsyncConfig===================");
            //     }
            // }, true);

            if (winContainer == null)
            {
                winContainer = GameObject.Find("WinContainer");
            }

            return true;
        }

        protected override bool InitController()
        {
            #region 创建Controller

            controllers?.Add((int)WindowModel.Test, new TestController(commandEngine));
            controllers?.Add((int)WindowModel.Test1, new TestController1(commandEngine));

            #endregion

            #region 初始化Controller

            foreach (var controller in controllers)
            {
                controller.Value.Init();
            }

            #endregion

            return true;
        }

        protected override bool InitCommandEngine()
        {
            commandEngine.RegisterCommand((int)UI_COMMAND.COMMAND1, (i, i1, arg3) => { return true;});
            commandEngine.RegisterCommand((int)UI_COMMAND.COMMAND2, (i, i1, arg3) => { return true;});
            commandEngine.RegisterCommand((int)UI_COMMAND.COMMAND3, (i, i1, arg3) => { return true;});

            return true;
        }

        protected override bool ReleaseController()
        {
            foreach (var controller in controllers)
            {
                controller.Value.Release();
            }

            controllers?.Clear();

            return true;
        }

        protected override bool ReleaseCommandEngine()
        {
            for (UI_COMMAND i = UI_COMMAND.COMMAND1; i < UI_COMMAND.MAX; i++)
            {
                commandEngine.UnRegisterCommand((int)i);
            }
            
            return true;
        }

        public bool IsExistWindow(WindowModel winModel)
        {
            WindowModelConfig config = null;
            if (!winConfigs.TryGetValue(winModel, out config))
            {
                return false;
            }

            IController controller = null;
            if (!controllers.TryGetValue((int)config.winModel, out controller))
            {
                return false;
            }

            return controller.IsExistView((int)winModel);
        }

        public void ChangeSceneHideViews()
        {
            if (winContainer == null || winConfigs == null)
            {
                return;
            }

            List<IView> views = winContainer.GetComponentsInChildren<IView>().ToList();
            foreach (var view in views)
            {
                if (winConfigs.TryGetValue((WindowModel)view.viewID, out var config) && config.changeSceneHide)
                {
                    if (controllers.TryGetValue((int)config.winModel, out var controller))
                    {
                        controller.HideView(0);
                    }
                }
            }
        }

        public void OpenWindow(WindowModel winModel, Action<IController, IView> callback)
        {
            //倒计时五秒退出到选角或者登录，不可以做任何操作/
            // if ((GlobalGame.Instance.GameClient.IsRequestEnterState(GameState.SelectActor) ||
            //      GlobalGame.Instance.GameClient.IsRequestEnterState(GameState.Login))
            //     && winModel != WindowModel.NotificationActorUnder)
            // {
            //     return;
            // }

            WindowModelConfig config = null;
            if (!winConfigs.TryGetValue(winModel, out config))
            {
                return;
            }

            IController controller = null;
            if (!controllers.TryGetValue((int)config.winModel, out controller))
            {
                return;
            }

            if (!controller.IsExistView((int)config.winModel) || !controller.IsExistView((int)winModel))
            {
                // ZSpawnPool.Instance.StartCoroutine(AsyncCreateWindow(config, (int)winModel, callback));
                EventEngine.Instance.StartCoroutine(AsyncCreateWindow(config, (int)winModel, callback));
                return;
            }

            if (winModel != config.winModel)
            {
                if (!controller.IsViewShowed((int)config.winModel))
                {
                    controller.ShowView((int)config.winModel);
                }
            }

            controller.ShowView((int)winModel);
            controller.CallbackView((int)winModel, callback);
        }

        private IEnumerator AsyncCreateWindow(WindowModelConfig config, int showViewID, Action<IController, IView> callback)
        {
            IController controller = null;
            if (!controllers.TryGetValue((int)config.winModel, out controller))
            {
                yield break;
            }

            // AsyncOperationHandle<GameObject> winAssetReq = default;
            // if (!controller.IsExistView((int)config.winModel))
            // {
            //     winAssetReq = AssetManager.LoadAsset<GameObject>(config.prefab.path);
            //     while (!winAssetReq.IsDone)
            //     {
            //         yield return winAssetReq;
            //     }
            //
            //     if (winAssetReq.Result != null)
            //     {
            //         GameObject prefabObj = GameObject.Instantiate(winAssetReq.Result);
            //         UIView view = prefabObj.GetComponent<UIView>();
            //         if (prefabObj != null && winContainer != null && view != null)
            //         {
            //             prefabObj.transform.SetParent(winContainer.transform);
            //             controller.ControlView((int)config.winModel, view, true).UnRegisterViewWhenViewDestroyed();
            //         }
            //
            //         winAssetReq = default;
            //     }
            // }

            if (showViewID == (int)config.winModel)
            {
                controller.CallbackView(showViewID, callback);
                yield break;
            }

            foreach (var node in config.subNode)
            {
                if (controller.IsExistView((int)node.winModel))
                {
                    continue;
                }

                // winAssetReq = AssetManager.LoadAsset<GameObject>(node.prefab.path);
                // while (!winAssetReq.IsDone)
                // {
                //     yield return winAssetReq;
                // }
                //
                // if (winAssetReq.Result != null)
                // {
                //     GameObject subWinObj = GameObject.Instantiate(winAssetReq.Result);
                //     UIView view = subWinObj.GetComponent<UIView>();
                //     if (subWinObj != null && view != null)
                //     {
                //         controller.ControlView((int)node.winModel, view, false, node.parentPath).UnRegisterViewWhenViewDestroyed();
                //         if ((int)node.winModel == showViewID)
                //         {
                //             controller.CallbackView(showViewID, callback);
                //         }
                //     }
                //
                //     winAssetReq = default;
                // }
            }

            controller.LateUpdateData();
        }

        public void CloseWindow(WindowModel winModel)
        {
            if (!winConfigs.TryGetValue(winModel, out var config))
            {
                return;
            }

            if (!controllers.TryGetValue((int)winModel, out var controller))
            {
                return;
            }
            
            controller.DropView((int)winModel, config.winModel == winModel);
        }

        public bool ExecuteEvent(int eventID, int srcType, int srcKey, object args)
        {
            return false;
        }

        private void Update()
        {
            
        }

        private void LateUpdate()
        {
            
        }

        private void FixedUpdate()
        {
            
        }
    }
}