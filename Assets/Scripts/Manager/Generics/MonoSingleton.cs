using UnityEngine;

namespace Manager.Generics
{
    public class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        protected virtual bool DontDestroy => true;
        
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject gameObject = new GameObject(typeof(T).Name);
                    _instance = gameObject.AddComponent<T>();
                }

                return _instance;
            }
        }

        public virtual void Awake()
        {
            if (DontDestroy)
                DontDestroyOnLoad(gameObject);

            if (_instance == null)
                _instance = this as T;
            else
            {
                if (_instance != this)
                    Destroy(gameObject);
            }
        }
    }
}