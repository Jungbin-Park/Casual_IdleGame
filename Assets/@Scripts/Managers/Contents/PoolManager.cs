using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPool
{
    Transform Root { get; set; }
    Queue<GameObject> pool { get; set;} // queue - 선입선출

    GameObject Pop(Action<GameObject> action = null);

    void Push (GameObject obj, Action<GameObject> action = null);
}

class Pool : IPool
{
    public Queue<GameObject> pool { get; set;} = new Queue<GameObject>();

    public Transform Root { get; set; }

    public GameObject Pop(Action<GameObject> action = null)
    {
        GameObject obj = pool.Dequeue(); // Dequeue = pop
        obj.SetActive(true);
        if(action != null)
        {
            action?.Invoke(obj);
        }
        return obj;
    }

    public void Push(GameObject obj, Action<GameObject> action = null)
    {
        pool.Enqueue(obj); // Enqueue = push
        obj.transform.parent = Root;
        obj.SetActive(false);
        if(action != null)
        {
            action?.Invoke(obj);
        }
    }
}

public class PoolManager
{
    public Dictionary<string, IPool> pools = new Dictionary<string, IPool>();

    Transform baseObj = null;

    public void Init(Transform t)
    {
        baseObj = t;
    }

    public IPool Pop(string path)
    {
        if(pools.ContainsKey(path) == false)
        {
            CreatePool(path);
        }

        // 큐 안에 아무것도 없다면
        if (pools[path].pool.Count <= 0) Push(path);

        return pools[path];
    }

    private GameObject CreatePool(string path)
    {
        GameObject go = new GameObject(path + " Root");
        go.transform.SetParent(baseObj);
        Pool pool = new Pool();

        pools.Add(path, pool);

        pool.Root = go.transform;
        return go;
    }

    private void Push(string path)
    {
        var go = Managers.s_instance.InstantiatePath(path);
        go.transform.parent = pools[path].Root;

        pools[path].Push(go);
    }
}
