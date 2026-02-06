using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class MonsterController : CreatureController
{
    [SerializeField]
    float m_Speed;

    bool isSpawn = false;

    protected override void Start()
    {
        base.Start();
        hp = 5;

    }


    public void Init()
    {
        StartCoroutine(CoSpawnStart());
    }

    IEnumerator CoSpawnStart()
    {
        float current = 0.0f;
        float percent = 0.0f;
        float start = 0.0f;
        float end = transform.localScale.x;
        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / 0.2f;
            float lerpPos = Mathf.Lerp(start, end, percent);    // 선형보간 : start 부터 end까지 특정 시간속도로 이동
            transform.localScale = new Vector3(lerpPos, lerpPos, lerpPos);
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);
        isSpawn = true;
    }

    public void GetDamage(double damage)
    {
        hp -= damage;

        if(hp <= 0)
        {
            isDead = true;
            SpawningPool.monsters.Remove(this);

            var smokeObj = Managers.Pool.Pop("Smoke").Pop((value) =>
            {
                value.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
                Managers.s_instance.ReturnPool(value.GetComponent<ParticleSystem>().duration, value, "Smoke");
            });

            Managers.Pool.pools["Monster"].Push(this.gameObject);
        }
    }

    private void Update()
    {
        transform.LookAt(Vector3.zero);

        if (!isSpawn) return;

        float targetDistance = Vector3.Distance(transform.position, Vector3.zero);
        if ((targetDistance <= 0.5f))
        {
            AnimatorChange("isIdle");
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, Time.deltaTime * m_Speed);
            AnimatorChange("isMove");
        }

    }

    IEnumerator CoReturnSmoke(float timer, GameObject gameObject, string path)
    {
        yield return new WaitForSeconds(timer);
        Managers.Pool.pools[path].Push(gameObject);
    }
}
