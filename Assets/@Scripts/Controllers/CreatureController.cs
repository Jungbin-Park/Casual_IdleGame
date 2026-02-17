using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    Animator animator;

    public double hp;
    public double atk;
    public float atkSpeed;
    public bool isDead = false;

    protected float attackRange = 3.0f;    // ���� ����
    protected float targetRange = 5.0f;    // �߰� ����
    protected bool isAttack = false;

    protected Transform target;

    [SerializeField]
    private Transform bulletTr;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    protected void InitAttack() => isAttack = false;

    protected void AnimatorChange(string temp)
    {
        if(temp == "isAttack")
        {
            animator.SetTrigger("isAttack");
            return;
        }

        animator.SetBool("isIdle", false);
        animator.SetBool("isMove", false);

        animator.SetBool(temp, true);
    }

    protected virtual void Bullet()
    {
        if (target == null) return;

        Managers.Pool.Pop("Bullet").Pop((value) =>
        {
            value.transform.position = bulletTr.position;
            value.GetComponent<BulletController>().Init(target, 10, "CH_01");
        });
    }

    protected void FindClosestTarget<T>(T[] targets) where T : Component
    {
        var monsters = targets;
        Transform closestTarget = null;
        float maxDistance = targetRange;

        foreach(var monster in monsters)
        {
            float targetDist = Vector3.Distance(transform.position, monster.transform.position);

            if(targetDist < maxDistance )
            {
                closestTarget = monster.transform;
                maxDistance = targetDist;
            }
        }

        target = closestTarget;

        if (target != null)
            transform.LookAt(target.position);
    }
}
