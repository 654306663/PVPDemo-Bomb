using UnityEngine;
using System.Collections;
using System;
using Net;

public class DeadState : FSMState
{
    private GameObject thisGo;
    private Animator animator;
    private Rigidbody rigidbody;


    public DeadState(GameObject thisGo)
    {
        stateId = FSMStateId.Dead;
        this.thisGo = thisGo;

        animator = thisGo.GetComponent<Animator>();
        rigidbody = thisGo.GetComponent<Rigidbody>();
    }

    public override void DoBeforeEnter(params object[] pars)
    {
        animator.SetTrigger("Dead_t");
    }

    public override void DoUpdate()
    {

    }

    public override void DoBeforeLeave()
    {

    }
}
