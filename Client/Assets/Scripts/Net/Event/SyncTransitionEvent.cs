using UnityEngine;
using System.Collections;
using System;
using ExitGames.Client.Photon;
using Tools;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using ProtoData;

namespace Net
{
    public class SyncTransitionEvent : HandlerBase
    {

        public override void AddListener()
        {
            EventMediat.AddListener(MessageCode.SyncTransition, OnSyncTransitionReceived);
        }

        public override void RemoveListener()
        {
            EventMediat.RemoveListener(MessageCode.SyncTransition, OnSyncTransitionReceived);
        }


        void OnSyncTransitionReceived(EventData eventData)
        {
            byte[] bytes = (byte[])DictTool.GetValue<byte, object>(eventData.Parameters, 1);
            object[] objs= (object[])DictTool.GetValue<byte, object>(eventData.Parameters, 2);

            SyncTransitionS2C syncTransitionS2C = BinSerializer.DeSerialize<SyncTransitionS2C>(bytes);

            BattleSyncMgr.Instance.OnSyncTransitionEvent(syncTransitionS2C.username, (FSMTransition)syncTransitionS2C.targetTransition, objs);
        }
    }
}
