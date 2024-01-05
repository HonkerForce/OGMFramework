using System.Collections.Generic;
using UnityEngine;

namespace YFramework
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

		void Awake()
		{
			
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