using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CreatureController  
{
    Vector3 startPos;
    Quaternion rot;

    protected override void Start()
    {
        base.Start();

        startPos = transform.position;
        rot = transform.rotation;
    }

    private void Update()
    {
        // 타겟이 없으면 시작 지점으로 이동
        if(target == null)
        {
            FindClosestTarget(SpawningPool.monsters.ToArray());

            float targetPos = Vector3.Distance(transform.position, startPos);
            if(targetPos > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPos, Time.deltaTime);
                transform.LookAt(startPos);
                AnimatorChange("isMove");
            }
            else
            {
                transform.rotation = rot;
                AnimatorChange("isIdle");
            }

            return;
        }

        float targetDistance = Vector3.Distance(transform.position, target.position);
        // 현재 타겟이 추적 범위 안에 있지만 공격 범위 밖인 경우
        if(targetDistance <= targetRange && targetDistance > attackRange)
        {
            AnimatorChange("isMove");
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime);
        }
        // 타겟이 공격 범위 안에 들어오면 공격
        else if(targetDistance <= attackRange)
        {
            AnimatorChange("isAttack");
        }
    }
}
