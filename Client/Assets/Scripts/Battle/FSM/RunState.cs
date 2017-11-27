using UnityEngine;
using System.Collections;
using System;

public class RunState : FSMState
{
    private GameObject thisGo;
    private Animator animator;
    private Rigidbody rigidbody;
    private Transform camera;


    public RunState(GameObject thisGo)
    {
        stateId = FSMStateId.Run;
        this.thisGo = thisGo;

        animator = thisGo.GetComponent<Animator>();
        rigidbody = thisGo.GetComponent<Rigidbody>();
        camera = Camera.main.transform;
    }

    public override void DoBeforeEnter(params object[] pars)
    {
        animator.SetFloat("Speed_f", 1);
    }


    float h, v;
    Vector3 moveVec;

    public override void DoUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1 << LayerMask.NameToLayer("Plane")))
            {
                system.PerformTransition(FSMTransition.RunToThrow, hit.point.x, hit.point.y, hit.point.z);
            }
        }
    }

    public override void DoFixedUpdate()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        if (h == 0 && v == 0)
        {
            system.PerformTransition(FSMTransition.RunToIdle);
            return;
        }

        // 根据摄像机方向 进行移动
        moveVec = new Vector3(h, 0, v);
        moveVec = Quaternion.Euler(0, camera.eulerAngles.y, 0) * moveVec;
        rigidbody.AddForce(moveVec * 200);
        RotatePlayer();
    }

    private void RotatePlayer()
    {
        Vector3 vec = Quaternion.Euler(0, 0, 0) * moveVec;
        Quaternion qua = Quaternion.LookRotation(vec);
        thisGo.transform.rotation = Quaternion.Lerp(thisGo.transform.rotation, qua, Time.deltaTime * 100);
    }
}
