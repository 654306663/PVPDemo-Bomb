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
    public class SyncPlayerEvent : EventBase
    {
        public override void AddListener()
        {
            EventMediat.AddListener(MessageCode.AddPlayer, OnSyncAddPlayerReceived);
            EventMediat.AddListener(MessageCode.RemovePlayer, OnSyncRemovePlayerReceived);
        }

        public override void RemoveListener()
        {
            EventMediat.RemoveListener(MessageCode.AddPlayer, OnSyncAddPlayerReceived);
            EventMediat.RemoveListener(MessageCode.RemovePlayer, OnSyncRemovePlayerReceived);
        }

        void OnSyncAddPlayerReceived(EventData eventData)
        {
            byte[] bytes = (byte[])DictTool.GetValue<byte, object>(eventData.Parameters, 1);
            PlayerS2CEvt playerS2CEvt = BinSerializer.DeSerialize<PlayerS2CEvt>(bytes);

            Debug.LogError("新玩家进入：" + playerS2CEvt.username);

            BattleSyncMgr.Instance.OnAddPlayerEvent(playerS2CEvt.username, playerS2CEvt.modelName);
        }

        void OnSyncRemovePlayerReceived(EventData eventData)
        {
            string username = (string)DictTool.GetValue<byte, object>(eventData.Parameters, 1);

            Debug.LogError("离开玩家：" + username);

            BattleSyncMgr.Instance.OnRemovePlayerEvent(username);
        }
    }

}
