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

		public IManager[] managers = new IManager[(int)MANAGER_TYPE.MAX]
		{
			new UIManager(),			// MANAGER_TYPE.UI
		};
		
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
			
		}

		void Update()
		{
			
		}

		void LateUpdate()
		{
			
		}

		void OnDisable()
		{
			
		}

		void OnDestroy()
		{
			
		}
	}
}