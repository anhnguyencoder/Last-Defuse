using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _lock = new object();
    private static bool applicationIsQuitting = false;

    public static bool DontDestroyOnLoadEnabled { get; set; } = true;

    public static T Instance
    {
        get
        {
            if (applicationIsQuitting) return null;

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<T>();

                    if (FindObjectsByType<T>(FindObjectsSortMode.None).Length > 1)
                    {
                        Debug.LogError("[Singleton] More than one instance of " + typeof(T) + " found!");
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject("[Singleton] " + typeof(T));
                        _instance = singleton.AddComponent<T>();

                        if (DontDestroyOnLoadEnabled)
                            DontDestroyOnLoad(singleton);
                    }
                }
            }

            return _instance;
        }
    }

    public virtual void KeepAlive(bool alive)
    {
        DontDestroyOnLoadEnabled = alive;
    }

    protected virtual void Awake()
    {
        lock (_lock)
        {
            if (_instance == null)
            {
                _instance = this as T;

                if (DontDestroyOnLoadEnabled)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
            else if (_instance != this)
            {
                Debug.LogWarning("[Singleton] Duplicate instance of " + typeof(T) +
                                 " detected. Destroying new instance.");
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }

    private void OnDisable()
    {
        if (!Application.isPlaying)
            _instance = null;
    }

    private void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }
}
