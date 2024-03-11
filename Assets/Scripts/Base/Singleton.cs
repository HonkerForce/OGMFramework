using UnityEngine;

namespace OGMFramework
{
    public interface ISingleton<T>
    {
        static T Instance { get; }
    }

    public class Singleton<T> : ISingleton<T>
        where T : class, new()
    {
        private static T instance = null;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }

                return instance;
            }
        }
    }

    public class SingleMonoBehaviour<T> : MonoBehaviour, ISingleton<T> 
        where T : MonoBehaviour
    {
        private static T instance = null;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    T obj = FindObjectOfType<T>();
                    if (obj == null)
                    {
                        var go = new GameObject(nameof(T));
                        obj = go.AddComponent<T>();
                        instance = obj;
                        
                        DontDestroyOnLoad(go);
                    }
                    else
                    {
                        obj.SendMessage("Awake");
                    }
                }

                return instance;
            }
        }

        public void Awake()
        {
            instance ??= this as T;
            
            DontDestroyOnLoad(gameObject);
        }
    }
}