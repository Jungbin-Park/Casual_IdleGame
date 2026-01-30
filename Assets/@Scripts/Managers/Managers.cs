using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

// 모든 매니저 스크립트의 집합체

public class Managers : MonoBehaviour
{
    public static Managers s_instance = null;

    private static PoolManager pool = new PoolManager();
    public static PoolManager Pool { get { return pool; } }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if(s_instance == null)
        {
            s_instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public GameObject InstantiatePath(string path)
    {
        return Instantiate(Resources.Load<GameObject>(path));
    }
}
