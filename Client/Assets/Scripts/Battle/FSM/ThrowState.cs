using UnityEngine;
using System.Collections;
using System;
using Net;

public class ThrowState : FSMState
{
    private GameObject thisGo;
    private Animator animator;
    private Rigidbody rigidbody;


    public ThrowState(GameObject thisGo)
    {
        stateId = FSMStateId.Throw;
        this.thisGo = thisGo;

        animator = thisGo.GetComponent<Animator>();
        rigidbody = thisGo.GetComponent<Rigidbody>();
    }

    Vector3 hitPosition;
    public override void DoBeforeEnter(params object[] pars)
    {
        AnimatorMgr.onStateExit += OnThrowAnimatorExit;

        animator.SetTrigger("Throw_t");

        hitPosition = new Vector3((float)pars[0], (float)pars[1], (float)pars[2]);
        thisGo.transform.LookAt(new Vector3(hitPosition.x, thisGo.transform.position.y, hitPosition.z));

        if (system.playerType == PlayerType.Self)
        {
            SyncTransformRequest.Instance.SendSyncTransformRequest(thisGo.transform.position, thisGo.transform.localEulerAngles.y);
            CoroutineUtil.Instance.WaitTime(0.4f, true, SendThrowBomb);
        }
    }

    void OnThrowAnimatorExit(Animator ani, AnimatorType type, AnimatorStateInfo stateInfo)
    {
        if (ani == animator)
        {
            if (type == AnimatorType.IdleThrow || type == AnimatorType.RunThrow)
            {
                system.PerformTransition(FSMTransition.ThrowToIdle);
            }
        }
    }

    public override void DoUpdate()
    {

    }

    public override void DoBeforeLeave()
    {
        AnimatorMgr.onStateExit -= OnThrowAnimatorExit;
    }

    void SendThrowBomb()
    {
        BombData bombData = new BombData();
        if(AnimatorMgr.GetIsPlaying(animator, AnimatorType.IdleThrow))
        {
            bombData.startPos = thisGo.transform.position + thisGo.transform.forward + new Vector3(0, 1f, 0);

            if (Vector3.Distance(thisGo.transform.position, new Vector3(hitPosition.x, thisGo.transform.position.y, hitPosition.z)) > 12)
            {
                hitPosition = thisGo.transform.position + (new Vector3(hitPosition.x, thisGo.transform.position.y, hitPosition.z) - thisGo.transform.position).normalized * 12;
            }
        }
        else if (AnimatorMgr.GetIsPlaying(animator, AnimatorType.RunThrow))
        {
            bombData.startPos = thisGo.transform.position + thisGo.transform.forward + new Vector3(0, 2f, 0);

            if (Vector3.Distance(thisGo.transform.position, new Vector3(hitPosition.x, thisGo.transform.position.y, hitPosition.z)) > 20)
            {
                hitPosition = thisGo.transform.position + (new Vector3(hitPosition.x, thisGo.transform.position.y, hitPosition.z) - thisGo.transform.position).normalized * 20;
            }
        }


        bombData.endPos = hitPosition;
        bombData.durationTime = 2;
        bombData.damageRange = 10;
        BombRequest.Instance.SendAddBombRequest(bombData);
    }
}
