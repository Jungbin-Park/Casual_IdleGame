using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;
using UnityEngine.UIElements;

public class COIN_PARENT : MonoBehaviour
{
    Vector3 target;
    Camera cam;
    RectTransform[] childs = new RectTransform[5];

    [Range(0.0f, 500.0f)]
    [SerializeField]
    float distanceRange, speed;

    private void Awake()
    {
        cam = Camera.main;
        for(int i = 0; i < childs.Length; i++)
        {
            childs[i] = transform.GetChild(i).GetComponent<RectTransform>();
        }
    }

    public void Init(Vector3 pos)
    {
        target = pos;

        transform.position = cam.WorldToScreenPoint(pos); 

        for(int i = 0; i < 5; i++)
        {
            childs[i].anchoredPosition = Vector2.zero;
        }

        transform.SetParent(UI_BaseCanvas.instance.GetUILayer(0));

        StartCoroutine(CoCoinEffect());
    }

    IEnumerator CoCoinEffect()
    {
        Vector2[] randPos = new Vector2[childs.Length];
        for(int i = 0; i <childs.Length; i++)
        {
            randPos[i] = new Vector2(target.x, target.y) + Random.insideUnitCircle * Random.Range(-distanceRange, distanceRange);
        }

        // ÄÚÀÎ ¶³±¸±â (Èð»Ñ¸®±â)
        while(true)
        {
            for(int i = 0; i < randPos.Length; i++)
            {
                RectTransform rect = childs[i];

                rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, randPos[i], Time.deltaTime * speed);
            }

            if (IsArriveLocal(randPos, 0.5f)) break;

            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        // ÄÚÀÎÀÌ Èð»Ñ·ÁÁø ÈÄ ui·Î ÀÚµ¿ È¹µæÃ³¸®
        while(true)
        { 
            for(int i = 0; i <  childs.Length; i++)
            {
                RectTransform rect = childs[i];
                rect.position = Vector2.MoveTowards(rect.position, UI_BaseCanvas.instance.ui_coin.position, Time.deltaTime * (speed * 20.0f));
            }

            if(IsArriveWorld(0.5f))
            {
                Managers.Pool.pools["COIN_PARENT"].Push(this.gameObject);
                break;
            }
            yield return null;
        }

        
    }

    private bool IsArriveLocal(Vector2[] end, float range)
    {
        for(int i = 0; i < childs.Length; i++)
        {
            float distance = Vector2.Distance(childs[i].anchoredPosition, end[i]);
            if(distance > range)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsArriveWorld(float Range)
    {
        for(int i = 0; i < childs.Length; i++)
        {
            float distance = Vector2.Distance(childs[i].position, UI_BaseCanvas.instance.ui_coin.position);
            if (distance > Range)
                return false;
        }

        return true;
    }
}
