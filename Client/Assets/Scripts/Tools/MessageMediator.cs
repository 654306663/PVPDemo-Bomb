/*********************
 * 姓名：王冲
 * 功能：解耦合 消息订阅 分发
 * 日期：2017/9/22
**********************/

using UnityEngine;
using System.Collections.Generic;
using System;

public class MessageMediator
{

    public delegate void Act();
    public delegate void Act<T>(T t);
    public delegate void Act<T1, T2>(T1 t1, T2 t2);
    public delegate void Act<T1, T2, T3>(T1 t1, T2 t2, T3 t3);
    public delegate void Act<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4);
    //public delegate void Act<T1, T2, T3, T4, T5>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5);

    static Dictionary<MessageMediatType, Delegate> messageTable = new Dictionary<MessageMediatType, Delegate>();

    #region 注册监听
    public static void AddListener(MessageMediatType type, Act act)
    {
        if (!messageTable.ContainsKey(type))
        {
            messageTable.Add(type, null);
        }

        Delegate d = messageTable[type];
        if (d != null && d.GetType() != act.GetType())
        {
            //Debug.LogError(string.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", type, d.GetType().Name, act.GetType().Name));
        }
        else
        {
            messageTable[type] = (Act)messageTable[type] + act;
        }
    }
    public static void AddListener<T>(MessageMediatType type, Act<T> act)
    {
        if (!messageTable.ContainsKey(type))
        {
            messageTable.Add(type, null);
        }

        Delegate d = messageTable[type];
        if (d != null && d.GetType() != act.GetType())
        {
            //Debug.LogError(string.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", type, d.GetType().Name, act.GetType().Name));
        }
        else
        {
            messageTable[type] = (Act<T>)messageTable[type] + act;
        }
    }
    public static void AddListener<T1, T2>(MessageMediatType type, Act<T1, T2> act)
    {
        if (!messageTable.ContainsKey(type))
        {
            messageTable.Add(type, null);
        }

        Delegate d = messageTable[type];
        if (d != null && d.GetType() != act.GetType())
        {
            //Debug.LogError(string.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", type, d.GetType().Name, act.GetType().Name));
        }
        else
        {
            messageTable[type] = (Act<T1, T2>)messageTable[type] + act;
        }
    }
    public static void AddListener<T1, T2, T3>(MessageMediatType type, Act<T1, T2, T3> act)
    {
        if (!messageTable.ContainsKey(type))
        {
            messageTable.Add(type, null);
        }

        Delegate d = messageTable[type];
        if (d != null && d.GetType() != act.GetType())
        {
            //Debug.LogError(string.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", type, d.GetType().Name, act.GetType().Name));
        }
        else
        {
            messageTable[type] = (Act<T1, T2, T3>)messageTable[type] + act;
        }
    }
    public static void AddListener<T1, T2, T3, T4>(MessageMediatType type, Act<T1, T2, T3, T4> act)
    {
        if (!messageTable.ContainsKey(type))
        {
            messageTable.Add(type, null);
        }

        Delegate d = messageTable[type];
        if (d != null && d.GetType() != act.GetType())
        {
            //Debug.LogError(string.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", type, d.GetType().Name, act.GetType().Name));
        }
        else
        {
            messageTable[type] = (Act<T1, T2, T3, T4>)messageTable[type] + act;
        }
    }
    #endregion

    #region 移除监听
    public static void RemoveListener(MessageMediatType type, Act listenerBeingRemoved)
    {
        if (messageTable.ContainsKey(type))
        {
            Delegate d = messageTable[type];

            if (d == null)
            {
                //Debug.LogError(string.Format("Attempting to remove listener with for event type \"{0}\" but current listener is null.", type));
            }
            else if (d.GetType() != listenerBeingRemoved.GetType())
            {
                //Debug.LogError(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", type, d.GetType().Name, listenerBeingRemoved.GetType().Name));
            }
            else
            {
                messageTable[type] = (Act)messageTable[type] - listenerBeingRemoved;
                if (d == null)
                {
                    messageTable.Remove(type);
                }
            }
        }
        else
        {
            //Debug.LogError(string.Format("Attempting to remove listener for type \"{0}\" but Messenger doesn't know about this event type.", type));
        }
    }

    public static void RemoveListener<T>(MessageMediatType type, Act<T> listenerBeingRemoved)
    {
        if (messageTable.ContainsKey(type))
        {
            Delegate d = messageTable[type];

            if (d == null)
            {
                //Debug.LogError(string.Format("Attempting to remove listener with for event type \"{0}\" but current listener is null.", type));
            }
            else if (d.GetType() != listenerBeingRemoved.GetType())
            {
                //Debug.LogError(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", type, d.GetType().Name, listenerBeingRemoved.GetType().Name));
            }
            else
            {
                messageTable[type] = (Act<T>)messageTable[type] - listenerBeingRemoved;
                if (d == null)
                {
                    messageTable.Remove(type);
                }
            }
        }
        else
        {
            //Debug.LogError(string.Format("Attempting to remove listener for type \"{0}\" but Messenger doesn't know about this event type.", type));
        }
    }



    public static void RemoveListener<T1, T2>(MessageMediatType type, Act<T1, T2> listenerBeingRemoved)
    {
        if (messageTable.ContainsKey(type))
        {
            Delegate d = messageTable[type];

            if (d == null)
            {
                //Debug.LogError(string.Format("Attempting to remove listener with for event type \"{0}\" but current listener is null.", type));
            }
            else if (d.GetType() != listenerBeingRemoved.GetType())
            {
                //Debug.LogError(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", type, d.GetType().Name, listenerBeingRemoved.GetType().Name));
            }
            else
            {
                messageTable[type] = (Act<T1, T2>)messageTable[type] - listenerBeingRemoved;
                if (d == null)
                {
                    messageTable.Remove(type);
                }
            }
        }
        else
        {
            //Debug.LogError(string.Format("Attempting to remove listener for type \"{0}\" but Messenger doesn't know about this event type.", type));
        }
    }
    public static void RemoveListener<T1, T2, T3>(MessageMediatType type, Act<T1, T2, T3> listenerBeingRemoved)
    {
        if (messageTable.ContainsKey(type))
        {
            Delegate d = messageTable[type];

            if (d == null)
            {
                //Debug.LogError(string.Format("Attempting to remove listener with for event type \"{0}\" but current listener is null.", type));
            }
            else if (d.GetType() != listenerBeingRemoved.GetType())
            {
                //Debug.LogError(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", type, d.GetType().Name, listenerBeingRemoved.GetType().Name));
            }
            else
            {
                messageTable[type] = (Act<T1, T2, T3>)messageTable[type] - listenerBeingRemoved;
                if (d == null)
                {
                    messageTable.Remove(type);
                }
            }
        }
        else
        {
            //Debug.LogError(string.Format("Attempting to remove listener for type \"{0}\" but Messenger doesn't know about this event type.", type));
        }
    }
    public static void RemoveListener<T1, T2, T3, T4>(MessageMediatType type, Act<T1, T2, T3, T4> listenerBeingRemoved)
    {
        if (messageTable.ContainsKey(type))
        {
            Delegate d = messageTable[type];

            if (d == null)
            {
                //Debug.LogError(string.Format("Attempting to remove listener with for event type \"{0}\" but current listener is null.", type));
            }
            else if (d.GetType() != listenerBeingRemoved.GetType())
            {
                //Debug.LogError(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", type, d.GetType().Name, listenerBeingRemoved.GetType().Name));
            }
            else
            {
                messageTable[type] = (Act<T1, T2, T3, T4>)messageTable[type] - listenerBeingRemoved;
                if (d == null)
                {
                    messageTable.Remove(type);
                }
            }
        }
        else
        {
            //Debug.LogError(string.Format("Attempting to remove listener for type \"{0}\" but Messenger doesn't know about this event type.", type));
        }
    }

    public static void RemoveAllListener(MessageMediatType type)
    {
        if (messageTable.ContainsKey(type))
        {
            messageTable[type] = null;
            messageTable.Remove(type);
        }
        else
        {
            //Debug.LogError(string.Format("Attempting to remove listener for type \"{0}\" but Messenger doesn't know about this event type.", type));
        }
    }
    #endregion

    #region 分配消息
    public static void Dispatch(MessageMediatType type)
    {
        if (!messageTable.ContainsKey(type))
            return;

        Delegate d;
        if (messageTable.TryGetValue(type, out d))
        {
            Act callback = d as Act;

            if (callback != null)
            {
                callback();
            }
            else
            {
                //Debug.LogError(string.Format("no such event type {0}", type));
            }
        }
    }
    public static void Dispatch<T>(MessageMediatType type, T param1)
    {
        if (!messageTable.ContainsKey(type))
            return;

        Delegate d;
        if (messageTable.TryGetValue(type, out d))
        {
            Act<T> callback = d as Act<T>;

            if (callback != null)
            {
                callback(param1);
            }
            else
            {
                //Debug.LogError(string.Format("no such event type {0}", type));
            }
        }
    }
    public static void Dispatch<T1, T2>(MessageMediatType type, T1 param1, T2 param2)
    {
        if (!messageTable.ContainsKey(type))
            return;

        Delegate d;
        if (messageTable.TryGetValue(type, out d))
        {
            Act<T1, T2> callback = d as Act<T1, T2>;

            if (callback != null)
            {
                callback(param1, param2);
            }
            else
            {
                //Debug.LogError(string.Format("no such event type {0}", type));
            }
        }
    }
    public static void Dispatch<T1, T2, T3>(MessageMediatType type, T1 param1, T2 param2, T3 param3)
    {
        if (!messageTable.ContainsKey(type))
            return;

        Delegate d;
        if (messageTable.TryGetValue(type, out d))
        {
            Act<T1, T2, T3> callback = d as Act<T1, T2, T3>;

            if (callback != null)
            {
                callback(param1, param2, param3);
            }
            else
            {
                //Debug.LogError(string.Format("no such event type {0}", type));
            }
        }
    }
    public static void Dispatch<T1, T2, T3, T4>(MessageMediatType type, T1 param1, T2 param2, T3 param3, T4 param4)
    {
        if (!messageTable.ContainsKey(type))
            return;

        Delegate d;
        if (messageTable.TryGetValue(type, out d))
        {
            Act<T1, T2, T3, T4> callback = d as Act<T1, T2, T3, T4>;

            if (callback != null)
            {
                callback(param1, param2, param3, param4);
            }
            else
            {
                //Debug.LogError(string.Format("no such event type {0}", type));
            }
        }
    }
    #endregion
}