using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using ExitGames.Client.Photon;

namespace Net
{
    public class HandlerMediat
    {

        public delegate void Act(OperationResponse t);

        static Dictionary<MessageCode, Delegate> messageTable = new Dictionary<MessageCode, Delegate>();

        /// <summary>
        /// 注册监听
        /// </summary>
        /// <param name="type"></param>
        /// <param name="act"></param>
        public static void AddListener(MessageCode type, Act act)
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

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <param name="type"></param>
        /// <param name="act"></param>
        public static void RemoveListener(MessageCode type, Act act)
        {
            if (messageTable.ContainsKey(type))
            {
                Delegate d = messageTable[type];

                if (d == null)
                {
                    //Debug.LogError(string.Format("Attempting to remove listener with for event type \"{0}\" but current listener is null.", type));
                }
                else if (d.GetType() != act.GetType())
                {
                    //Debug.LogError(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", type, d.GetType().Name, act.GetType().Name));
                }
                else
                {
                    messageTable[type] = (Act)messageTable[type] - act;
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

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="param"></param>
        public static void Dispatch(MessageCode type, OperationResponse param)
        {
            Delegate d;
            if (messageTable.TryGetValue(type, out d))
            {
                Act callback = d as Act;

                if (callback != null)
                {
                    callback(param);
                }
                else
                {
                    //Debug.LogError(string.Format("no such event type {0}", type));
                }
            }
        }

        /// <summary>
        /// 移除所有监听
        /// </summary>
        /// <param name="type"></param>
        public static void RemoveAllListener(MessageCode type)
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
    }
}
