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
            EventMediat.AddListener(MessageCode.SyncTransform, OnSyncTransformReceived);
        }

        public override void RemoveListener()
        {
            EventMediat.RemoveListener(MessageCode.SyncTransform, OnSyncTransformReceived);
        }

        /// <summary>
        /// 收到位置等消息
        /// </summary>
        /// <param name="eventData"></param>
        void OnSyncTransformReceived(EventData eventData)
        {
            byte[] bytes = (byte[])DictTool.GetValue<byte, object>(eventData.Parameters, 1);

            ProtoData.SyncTransformEvtS2C syncTransformEvtS2C = BinSerializer.DeSerialize<ProtoData.SyncTransformEvtS2C>(bytes);

            BattleSyncMgr.Instance.OnSyncTransformEvent(syncTransformEvtS2C.dataList);

            //Debug.LogWarning(Time.time);
        }
    }
}
