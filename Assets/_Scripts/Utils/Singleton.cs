using UnityEngine;

/// <summary>
/// Instead of destroying any new instances, it overrides the current instance. Handy for resetting state 
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour{
    public static T Instance {get; private set;}
    protected virtual void Awake() => Instance = this as T;
    protected virtual void OnApplicationQuit(){
        Instance = null;
        Destroy(gameObject);
    }
}

/// <summary>
/// This transforms the static instance into a basic Singleton. This will destroy any new
/// versions created, leaving the original intact.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
{
    protected override void Awake(){
        if (Instance != null){
            Destroy(gameObject);
            return;
        }
        base.Awake();
    }

}

/// <summary>
///  Finally we have a persistent version of singleton. lThis will survivfe through scene loads. 
///  Perfect for system classes which require stateful, persistent data. Or audio sources where
///  music plays through loading scenes, etc.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}
