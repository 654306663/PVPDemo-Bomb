using UnityEngine;
using System.Collections;
using Net;

public class FSMController : MonoBehaviour
{
    public FSMSystem system = new FSMSystem();

    // Use this for initialization
    void Start()
    {
        InitFSM();
    }

    void InitFSM()
    {
        IdleState idleState = new IdleState(gameObject);
        RunState runState = new RunState(gameObject);
        ThrowState throwState = new ThrowState(gameObject);

        // 注册切换条件 对应的 状态
        idleState.AddTransition(FSMTransition.IdleToRun, FSMStateId.Run);
        runState.AddTransition(FSMTransition.RunToIdle, FSMStateId.Idle);

        idleState.AddTransition(FSMTransition.IdleToThrow, FSMStateId.Throw);
        runState.AddTransition(FSMTransition.RunToThrow, FSMStateId.Throw);

        throwState.AddTransition(FSMTransition.ThrowToIdle, FSMStateId.Idle);

        system.AddState(idleState);
        system.AddState(runState);
        system.AddState(throwState);

        system.Start(FSMStateId.Idle);
    }

    void Update()
    {
        if (system.playerType == PlayerType.Self)
        {
            system.CurrentState.DoUpdate();
        }
    }

    void FixedUpdate()
    {
        if (system.playerType == PlayerType.Self)
        {
            system.CurrentState.DoFixedUpdate();
        }
    }
}
