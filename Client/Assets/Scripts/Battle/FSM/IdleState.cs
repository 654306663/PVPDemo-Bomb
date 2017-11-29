using UnityEngine;
using System.Collections;
using System;

public class IdleState : FSMState
{
    private GameObject thisGo;
    private Animator animator;

    public IdleState(GameObject thisGo)
    {
        stateId = FSMStateId.Idle;
        this.thisGo = thisGo;

        animator = thisGo.GetComponent<Animator>();
    }

    public override void DoBeforeEnter(params object[] pars)
    {
        animator.SetFloat("Speed_f", 0);
    }

    public override void DoUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");


        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("Plane")))
            {
                system.PerformTransition(FSMTransition.IdleToThrow, hit.point.x, hit.point.y, hit.point.z);
                return;
            }
        }

        if (h != 0 || v != 0)
        {
            system.PerformTransition(FSMTransition.IdleToRun);
        }
    }
}
