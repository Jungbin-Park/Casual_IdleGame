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
        // 텍스트 위치 랜덤성 부여
        pos.x += Random.Range(-0.3f, 0.3f);
        pos.z += Random.Range(-0.3f, 0.3f);

        // 텍스트 설정
        target = pos;
        hitText.text = damage.ToString();
        transform.SetParent(UI_BaseCanvas.instance.transform);
        upRange = 0.0f;

        // 크리티컬이면
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
