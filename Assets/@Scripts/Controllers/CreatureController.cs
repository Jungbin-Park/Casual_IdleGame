using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    Animator animator;



    public double hp;
    public double atk;
    public float atkSpeed;

    protected float attackRange;    // 공격 범위
    protected float targetRange;    // 추격 범위

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    protected void AnimatorChange(string temp)
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isMove", false);

        animator.SetBool(temp, true);
    }
}
