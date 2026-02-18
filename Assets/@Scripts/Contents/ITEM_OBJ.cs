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
    private GameObject[] itemRarities;
    [SerializeField]
    private ParticleSystem particleLoot;
    [SerializeField]
    private float fireAngle = 45.0f;
    [SerializeField]
    private float gravity = 9.8f;

    Rarity rarity;
    bool isDropped = false;

    void RarityCheck()
    {
        isDropped = true;

        transform.rotation = Quaternion.identity; // {0, 0, 0}

        itemRarities[(int)rarity].SetActive(true);

        itemTextRect.gameObject.SetActive(true);
        itemTextRect.SetParent(UI_BaseCanvas.instance.GetUILayer(2));

        itemText.text = Utils.String_Color_Rarity(rarity) + "TEST ITEM" + "</color>";

        // <color=#FFFFFF>TEST ITEM</color>
        // <size=35>TEST ITEM</size>

        StartCoroutine(CoLootItem());
    }
    
    IEnumerator CoLootItem()
    {
        yield return new WaitForSeconds(Random.Range(1.0f, 1.5f));

        // 루팅이 진행될 때, 이펙트 끄기
        for(int i = 0; i < itemRarities.Length; i++) itemRarities[i].SetActive(false);

        itemTextRect.transform.SetParent(this.transform);
        itemTextRect.gameObject.SetActive(false);

        particleLoot.Play();

        yield return new WaitForSeconds(0.5f);

        Managers.Pool.pools["Item_OBJ"].Push(this.gameObject);
    }

    private void Update()
    {
        if (isDropped == false) return;

        itemTextRect.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    public void Init(Vector3 pos)
    {
        rarity = (Rarity)Random.Range(0, 4);
        isDropped = false;
        // 위치는 몬스터가 사망한 위치
        transform.position = pos;
        Vector3 targetPos = new Vector3(pos.x + (Random.insideUnitSphere.x * 2.0f), 0.5f, pos.z + (Random.insideUnitSphere.z * 2.0f));
        StartCoroutine(CoSimulateProjectile(targetPos));
    }

    IEnumerator CoSimulateProjectile(Vector3 pos)
    {
        float targetDist = Vector3.Distance(transform.position, pos);

        float projectileVelocity = targetDist / (Mathf.Sin(2 * fireAngle * Mathf.Deg2Rad) / gravity);

        // sqrt : 제곱근근
        float Vx = Mathf.Sqrt(projectileVelocity) * Mathf.Cos(fireAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectileVelocity) * Mathf.Sin(fireAngle * Mathf.Deg2Rad);
        float flightDuration = targetDist / Vx;

        // 발사 방향으로 오브젝트를 회전시켜 파티클이 올바르게 보이도록 함
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
