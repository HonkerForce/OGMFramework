using System;
using System.Collections.Generic;
using UnityEngine;

namespace OGMFramework
{
	public class GameManager : MonoBehaviour
	{
		public enum MANAGER_TYPE
		{
			UI,
			MAX
		}

		private IManager[] managers = new IManager[(int)MANAGER_TYPE.MAX]
		{
			UIManager.Instance,			// MANAGER_TYPE.UI
		};

		private static Action update;
		private static Action fixedUpdate;
		private static Action lateUpdate;
		
		private void InitGame()
		{
			for (MANAGER_TYPE i = MANAGER_TYPE.UI; i < MANAGER_TYPE.MAX; i++)
			{
				var ret = managers[(int)i].Init();
				if (!ret)
				{
					print(Enum.GetName(typeof(MANAGER_TYPE), i) + "(Manager) CreateInit Fail!");
				}
			}
		}

		public static void RegisterMonoEvent(Action update, Action fixedUpdate, Action lateUpdate)
		{
			if (update != null)
			{
				GameManager.update += update;
			}

			if (fixedUpdate != null)
			{
				GameManager.fixedUpdate += fixedUpdate;
			}

			if (lateUpdate != null)
			{
				GameManager.lateUpdate += lateUpdate;
			}
		}

		void Awake()
		{
			InitGame();
		}

		void OnEnable()
		{
			
		}

		void Start()
		{
			
		}

		void FixedUpdate()
		{
			GameManager.fixedUpdate();
		}

		void Update()
		{
			GameManager.update();
		}

		void LateUpdate()
		{
			GameManager.lateUpdate();
		}

		void OnDisable()
		{
			
		}

		void OnDestroy()
		{
			
		}
	}
}