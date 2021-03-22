using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHandler : MonoBehaviour
{
    protected static CoroutineHandler instance;
    public static CoroutineHandler Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("CoroutineHandler");
                DontDestroyOnLoad(obj);
                instance = obj.AddComponent<CoroutineHandler>();
            }

            return instance;
        }
    }

    public void OnDisable()
    {
        if (instance)
            Destroy(instance.gameObject);
    }

    public static Coroutine StartStaticCoroutine(IEnumerator coroutine)
    {
        return Instance.StartCoroutine(coroutine);
    }

    public static void StopStaticCoroutine(Coroutine coroutine)
    {
        Instance.StopCoroutine(coroutine);
    }

}