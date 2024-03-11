using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace OGMFramework
{
	public enum WindowModel
	{
		None = -1,
		Test,
		Test1,
		Max
	}

	public static class CreateWindowConfigTool
	{
		private static WindowConfig config;
		
		[MenuItem("Tools/Create UI Configs File")]
		public static void CreateConfig()
		{
			config = ScriptableObject.CreateInstance<WindowConfig>();
			if (AssetDatabase.IsValidFolder("Assets/Configs") == false)
			{
				AssetDatabase.CreateFolder("Assets", "Configs");
			}
			AssetDatabase.CreateAsset(config, "Assets/Configs/NewUIWinConfigs.asset");
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
	}

	[Serializable]
	[CreateAssetMenu(fileName = "NewUIWinConfigs", menuName = "UIWindowConfigs", order = 1)]
	public class WindowConfig : ScriptableObject
	{
		public WindowModel winModel;
		public AssetReferenceGameObject prefabRef;
		public bool isHideWhenSceneChanged;
		public List<SubWindowConfig> subWinConfigs;
	}

	[Serializable]
	public class SubWindowConfig
	{
		public WindowModel winModel;
		public AssetReferenceGameObject prefabRef;
		public string parentRoot;
	}
}