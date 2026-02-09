using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{
    [SerializeField]
    int m_Count;
    [SerializeField]
    float m_SpawnTime;

    public static List<MonsterController> monsters = new List<MonsterController>();
    public static List<PlayerController> players = new List<PlayerController>();

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
                monsters.Add(value.GetComponent<MonsterController>());
            });
        }

        yield return new WaitForSeconds(m_SpawnTime);

        StartCoroutine(CoSpawn());
    }

}
