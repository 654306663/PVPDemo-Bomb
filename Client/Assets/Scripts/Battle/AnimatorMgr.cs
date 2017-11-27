using UnityEngine;
using System.Collections;
using System;

public enum AnimatorType
{
    Empty,
    Idle,
    Walk,
    Run,
    IdleThrow,
    RunThrow,
    Death
}

public class AnimatorMgr {

    public static Action<Animator, AnimatorType, AnimatorStateInfo> onStateEnter;
    public static Action<Animator, AnimatorType, AnimatorStateInfo> onStateUpdate;
    public static Action<Animator, AnimatorType, AnimatorStateInfo> onStateExit;

    static string GetAnimatorName(AnimatorType type)
    {
        string name = "";
        switch (type)
        {
            case AnimatorType.Idle:
                name = "Base.Idle";
                break;
            case AnimatorType.Walk:
                name = "Base.Walking";
                break;
            case AnimatorType.Run:
                name = "Base.Running";
                break;
            case AnimatorType.IdleThrow:
                name = "Base.IdleThrow";
                break;
            case AnimatorType.RunThrow:
                name = "Base.RunThrow";
                break;
            case AnimatorType.Death:
                name = "Base.Death";
                break;
            default:
                break;
        }
        return name;
    }

    public static int GetHashByType(AnimatorType type)
    {
        return Animator.StringToHash(GetAnimatorName(type));
    }

    public static AnimatorType GetTypeByHash(int hash)
    {
        AnimatorType animatorType = AnimatorType.Empty;
        foreach (AnimatorType type in Enum.GetValues(typeof(AnimatorType)))
        {
            if (hash == GetHashByType(type))
            {
                animatorType = type;
                break;
            }
        }

        return animatorType;
    }

    public static AnimatorType GetCurrentState(Animator animator)
    {
        AnimatorType animatorType = AnimatorType.Empty;
        foreach (AnimatorType type in Enum.GetValues(typeof(AnimatorType)))
        { 
            if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash == GetHashByType(type))
            {
                animatorType = type;
                break;
            }
        }

        return animatorType;
    }

    public static bool GetIsPlaying(Animator animator, AnimatorType type)
    {
        return animator.GetCurrentAnimatorStateInfo(0).fullPathHash == GetHashByType(type);
    }
}
