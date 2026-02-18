using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_HitText : MonoBehaviour
{
    Vector3 target;
    Camera cam;
    public TextMeshProUGUI hitText;

    [SerializeField]
    private GameObject criticalObj;

    float upRange = 0.0f;

    private void Start()
    {
        cam = Camera.main;
    }

    public void Init(Vector3 pos, double damage, bool critical = false)
    {
        // ÅØ½ºÆ® À§Ä¡ ·£´ý¼º ºÎ¿©
        pos.x += Random.Range(-0.3f, 0.3f);
        pos.z += Random.Range(-0.3f, 0.3f);

        // ÅØ½ºÆ® ¼³Á¤
        target = pos;
        hitText.text = damage.ToString();
        transform.SetParent(UI_BaseCanvas.instance.GetUILayer(1));
        upRange = 0.0f;

        // Å©¸®Æ¼ÄÃÀÌ¸é
        criticalObj.SetActive(critical);
        //hitText.colorGradient = 

        Managers.s_instance.ReturnPool(2.0f, this.gameObject, "UI_HitText");
    }

    private void Update()
    {
        Vector3 targetPos = new Vector3(target.x, target.y + upRange, target.z);
        transform.position = cam.WorldToScreenPoint(targetPos);

        if(upRange <= 0.3f)
        {
            upRange += Time.deltaTime;
        }
    }

    private void ReturnText()
    {
        Managers.Pool.pools["UI_HitText"].Push(this.gameObject);
    }
}
