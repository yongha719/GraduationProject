using Photon.Pun;
using System;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(T)) as T;

                if (instance == null)
                {
                    var singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<T>();
                    singletonObject.name = typeof(T).ToString();

                    Debug.Assert(false, $"Singleton is Null");
                }
            }

            return instance;
        }
    }


    protected virtual void Awake()
    {
        if (instance == null)
            instance = GetComponent<T>();
        else
        {
            Destroy(this);
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}

public class SingletonPunCallbacks<T> : MonoBehaviourPunCallbacks where T : SingletonPunCallbacks<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectsOfType(typeof(T)) as T;
            }

            return instance;
        }
    }


    protected virtual void Awake()
    {
        instance = (T)this;
    }

    protected virtual void OnDestroy()
    {
        instance = null;
    }
}