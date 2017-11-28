using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 状态切换条件
public enum FSMTransition
{
    Empty,
    IdleToRun,      // 搜寻到玩家
    RunToIdle,     // 玩家离远了
    IdleToThrow,
    RunToThrow,
    ThrowToIdle,
    IdleToDead,     
    RunToDead,     
    ThrowToDead,
}

// 几种状态
public enum FSMStateId
{
    Empty,
    Idle,     // 
    Run,      // 
    Throw,
    Dead,
}

// 所有状态继承该基类
public abstract class FSMState {

	protected FSMStateId stateId;
    public FSMStateId StateId
    {
        get{ return stateId; }
    }

    protected Dictionary<FSMTransition, FSMStateId> map = new Dictionary<FSMTransition, FSMStateId>();

    public FSMSystem system;

    /// <summary>
    /// 添加切换条件 对应的 下一个状态
    /// </summary>
    /// <param name="transition">Transition.</param>
    /// <param name="stateId">State identifier.</param>
    public void AddTransition(FSMTransition transition, FSMStateId stateId)
    {
        if (transition != FSMTransition.Empty && stateId != FSMStateId.Empty)
        {
            map.Add(transition, stateId);
        }
    }

    /// <summary>
    /// 移除
    /// </summary>
    /// <param name="transition">Transition.</param>
    public void RemoveTransition(FSMTransition transition)
    {
        if (map.ContainsKey(transition))
        {
            map.Remove(transition);
        }
    }

    /// <summary>
    /// 通过切换条件获取状态
    /// </summary>
    /// <returns>The state identifier.</returns>
    /// <param name="transition">Transition.</param>
    public FSMStateId GetStateId(FSMTransition transition)
    {
        if (map.ContainsKey(transition))
        {
            return map[transition];
        }
        return FSMStateId.Empty;
    }

    public virtual void DoBeforeEnter(params object[] pars) { }   // 状态进入前调用
    public virtual void DoBeforeLeave(){ }   // 状态离开前调用

    public virtual void DoUpdate() { }        // 在该状态时调用

    public virtual void DoFixedUpdate() { }        // 在该状态时调用
}
