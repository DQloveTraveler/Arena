using UnityEngine;
using System;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Type t = typeof(T);

                instance = (T)FindObjectOfType(t);
                if (instance == null)
                {
                    var obj = Instantiate(new GameObject(t.ToString())).AddComponent(t);
                }
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        // 他のゲームオブジェクトにアタッチされているか調べる
        // アタッチされている場合は破棄する。
        if (!CheckInstance())
        {
            Destroy(gameObject);
        }
    }

    private bool CheckInstance()
    {
        if (instance == null)
        {
            instance = this as T;
            return true;
        }
        else if (instance == this)
        {
            return true;
        }
        return false;
    }
}