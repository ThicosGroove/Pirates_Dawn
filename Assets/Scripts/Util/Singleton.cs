// http://www.unitygeek.com/unity_c_singleton/
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // Debug.Log("Trying to find singleton " + typeof(T).Name + "...");
                instance = FindObjectOfType<T>();

                if (instance == null)
                {
                    // Debug.Log("Singleton " + typeof(T).Name + " not found; Creating new singleton...");
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            // Debug.Log("Singleton " + typeof(T).Name + " successfully created on Awake.");
            instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
            // Debug.Log("Tried to override singleton " + typeof(T).Name + " on awake!");
        }
    }
}