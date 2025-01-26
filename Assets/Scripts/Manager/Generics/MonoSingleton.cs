using UnityEngine;

namespace Manager.Generics
{
    public class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject gameObject = new GameObject(typeof(T).Name);
                    instance = gameObject.AddComponent<T>();
                }

                return instance;
            }
        }

        public virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if (instance == null)
                instance = this as T;
            else
            {
                if (instance != this)
                    Destroy(gameObject);
            }
        }
    }
}