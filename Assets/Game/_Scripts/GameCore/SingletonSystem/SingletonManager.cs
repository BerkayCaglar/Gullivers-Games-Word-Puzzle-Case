using UnityEngine;

namespace GameCore.SingletonSystem
{
    /// <summary>
    /// This class is used to create a singleton MonoBehaviour.
    /// </summary>
    /// <typeparam name="T"> The type of the MonoBehaviour. </typeparam>
    public class AutoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance;

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
            else
            {
                Debug.LogError($"There is already an instance of {typeof(T)} in the scene!");
                Debug.LogError($"Name of the object: {Instance.gameObject.name}");
            }
        }
    }

    /// <summary>
    /// This class is used to create a singleton MonoBehaviour that persists between scenes.
    /// </summary>
    /// <typeparam name="T"> The type of the MonoBehaviour. </typeparam>
    public class PersistentSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance;

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.LogError($"There is already an instance of {typeof(T)} in the scene!");
            }
        }
    }

    public class ScriptableResourceSingleton<T> : ScriptableObject where T : ScriptableObject
    {
        const string PATH = "ScriptableSingletons/";
        private static T instance = null;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<T>(PATH + typeof(T).Name);
                }
                return instance;
            }
        }

        public virtual void Init() { }
    }
}
