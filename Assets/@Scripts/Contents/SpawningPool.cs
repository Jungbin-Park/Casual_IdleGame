using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{
    // 여러 마리가 몇 초마다 수시로 스폰
    [SerializeField]
    GameObject monsterPrefab;

    [SerializeField]
    int m_Count;
    [SerializeField]
    float m_SpawnTime;

    private void Start()
    {
        StartCoroutine(CoSpawn());
    }

    IEnumerator CoSpawn()
    {
        Vector3 pos;

        for(int i = 0; i < m_Count; i++)
        {
            pos = Vector3.zero + Random.insideUnitSphere * 5.0f;
            pos.y = 0.0f;

            while(Vector3.Distance(pos, Vector3.zero) <= 3.0f)
            {
                pos = Vector3.zero + Random.insideUnitSphere * 5.0f;
                pos.y = 0.0f;
            }

            var go = Managers.Pool.Pop("Monster").Pop((value) =>
            {
                // 몬스터가 생성되면 Init 함수 실행
                value.GetComponent<MonsterController>().Init();
                value.transform.position = pos;
                value.transform.LookAt(Vector3.zero);
            });

            StartCoroutine(PushCoroutine(go));
        }

        yield return new WaitForSeconds(m_SpawnTime);

        StartCoroutine(CoSpawn());
    }

    IEnumerator PushCoroutine(GameObject obj)
    {
        yield return new WaitForSeconds(1.0f);
        Managers.Pool.pools["Monster"].Push(obj);
    }
}
