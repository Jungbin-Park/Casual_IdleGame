using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ITEM_OBJ : MonoBehaviour
{
    [SerializeField]
    private Transform itemTextRect;
    [SerializeField]
    private TextMeshProUGUI itemText;

    [SerializeField]
    private float fireAngle = 45.0f;
    [SerializeField]
    private float gravity = 9.8f;

    bool isDropped = false;

    void RarityCheck()
    {
        isDropped = true;

        transform.rotation = Quaternion.identity; // {0, 0, 0}

        itemTextRect.gameObject.SetActive(true);
        itemTextRect.SetParent(UI_BaseCanvas.instance.GetUILayer(2));

        itemText.text = "TEST ITEM";
    }

    private void Update()
    {
        if (isDropped == false) return;

        itemTextRect.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    public void Init(Vector3 pos)
    {
        isDropped = false;
        transform.position = pos;
        Vector3 targetPos = new Vector3(pos.x + (Random.insideUnitSphere.x * 2.0f), 0.5f, pos.z + (Random.insideUnitSphere.z * 2.0f));
        StartCoroutine(CoSimulateProjectile(pos));
    }

    IEnumerator CoSimulateProjectile(Vector3 pos)
    {
        float targetDist = Vector3.Distance(transform.position, pos);

        float projectileVelocity = targetDist / (Mathf.Sin(2 * fireAngle * Mathf.Deg2Rad) / gravity);

        // sqrt : 제곱근
        float Vx = Mathf.Sqrt(projectileVelocity) * Mathf.Cos(fireAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectileVelocity) * Mathf.Sin(fireAngle * Mathf.Deg2Rad);
        float flightDuration = targetDist / Vx;

        // 아이템 드롭 방향이 바라보는 방향으로 되게끔
        transform.rotation = Quaternion.LookRotation(pos - transform.position);

        float time = 0.0f;

        while(time < flightDuration)
        {
            transform.Translate(0, (Vy - (gravity * time)) * Time.deltaTime, Vx * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }
        
        RarityCheck();
    }
}
