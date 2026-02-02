using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    Animator animator;

    public double hp;
    public double atk;
    public float atkSpeed;

    protected float attackRange = 1.0f;    // 공격 범위
    protected float targetRange = 3.0f;    // 추격 범위
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
        Debug.Log("Bullet Event");

        Managers.Pool.Pop("Bullet").Pop((value) =>
        {

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
