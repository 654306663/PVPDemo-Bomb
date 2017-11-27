using UnityEngine;
using ExitGames.Client.Photon;
using Tools;
using System;

namespace Net
{
    public class SyncTransformEvent : EventBase
    {
        public override void AddListener()
        {
            EventMediat.AddListener(MessageCode.SyncTransform, OnSyncPositionReceived);
        }

        public override void RemoveListener()
        {
            EventMediat.RemoveListener(MessageCode.SyncTransform, OnSyncPositionReceived);
        }

        
        void OnSyncPositionReceived(EventData eventData)
        {
            byte[] bytes = (byte[])DictTool.GetValue<byte, object>(eventData.Parameters, 1);

            ProtoData.SyncTransformEvtS2C syncTransformEvtS2C = BinSerializer.DeSerialize<ProtoData.SyncTransformEvtS2C>(bytes);

            BattleSyncMgr.Instance.OnSyncTransformEvent(syncTransformEvtS2C.dataList);

            //Debug.LogWarning(Time.time);
        }
    }
}
