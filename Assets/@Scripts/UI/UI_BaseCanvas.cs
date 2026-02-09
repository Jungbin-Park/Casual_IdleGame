using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BaseCanvas : MonoBehaviour
{
    public static UI_BaseCanvas instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }

    public Transform ui_coin;
    [SerializeField]
    Transform layer;

    public Transform GetUILayer(int index)
    {
        return layer.GetChild(index);
    }
}
