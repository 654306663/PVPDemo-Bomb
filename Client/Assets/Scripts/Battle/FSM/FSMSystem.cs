using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Net;

public enum PlayerType
{
    Self,
    Other,
}

public class FSMSystem {

    public PlayerType playerType;

    private Dictionary<FSMStateId, FSMState> stateDict;     // 存储每个状态对象

    private FSMState currentState;      // 当前状态
    public FSMState CurrentState
    {
        get{ return currentState; }
    }

    public FSMSystem()
    {
        stateDict = new Dictionary<FSMStateId, FSMState>();
    }

    /// <summary>
    /// 添加一个状态
    /// </summary>
    /// <param name="state">State.</param>
    public void AddState(FSMState state)
    {
        if (state != null && !stateDict.ContainsKey(state.StateId))
        {
            state.system = this;
            stateDict.Add(state.StateId, state);
        }
    }

    /// <summary>
    /// 移除一个状态
    /// </summary>
    /// <param name="stateId">State identifier.</param>
    public void RemoveState(FSMStateId stateId)
    {
        if (stateDict.ContainsKey(stateId)) 
        {
            stateDict.Remove(stateId);
        }
    }

    /// <summary>
    /// 执行切换下一个状态
    /// </summary>
    /// <param name="transition">Transition.</param>
    public void PerformTransition(FSMTransition transition, params object[] pars)
    {
        if (transition != null && currentState != null)
        {
            FSMStateId nextStateId = currentState.GetStateId(transition);
            if (nextStateId != FSMStateId.Empty)
            {
                FSMState state;
                stateDict.TryGetValue(nextStateId, out state);
                currentState.DoBeforeLeave();
                currentState = state;
                state.DoBeforeEnter(pars);
                if(playerType == PlayerType.Self)
                    SyncTransitionRequest.Instance.SendSyncTransitionRequest(transition, pars);
            }
        }
    }

    /// <summary>
    /// 默认执行的状态
    /// </summary>
    /// <param name="stateId">State identifier.</param>
    public void Start(FSMStateId stateId)
    {
        FSMState state;
        if (stateDict.TryGetValue(stateId, out state))
        {
            state.DoBeforeEnter();
            currentState = state;
        }
    }
}
