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
            EventMediat.AddListener(MessageCode.PlayerDead, OnSyncPlayerDeadReceived);
        }

        public override void RemoveListener()
        {
            EventMediat.RemoveListener(MessageCode.AddPlayer, OnSyncAddPlayerReceived);
            EventMediat.RemoveListener(MessageCode.RemovePlayer, OnSyncRemovePlayerReceived);
            EventMediat.RemoveListener(MessageCode.PlayerDead, OnSyncPlayerDeadReceived);
        }

        void OnSyncAddPlayerReceived(EventData eventData)
        {
            byte[] bytes = (byte[])DictTool.GetValue<byte, object>(eventData.Parameters, 1);
            AddPlayerS2CEvt playerS2CEvt = BinSerializer.DeSerialize<AddPlayerS2CEvt>(bytes);

            Debug.LogError("新玩家进入：" + playerS2CEvt.username);

            BattleSyncMgr.Instance.OnAddPlayerEvent(playerS2CEvt.username, playerS2CEvt.modelName, playerS2CEvt.nickName, playerS2CEvt.hp, playerS2CEvt.killCount);
        }

        void OnSyncRemovePlayerReceived(EventData eventData)
        {
            string username = (string)DictTool.GetValue<byte, object>(eventData.Parameters, 1);

            Debug.LogError("离开玩家：" + username);

            BattleSyncMgr.Instance.OnRemovePlayerEvent(username);
        }

        void OnSyncPlayerDeadReceived(EventData eventData)
        {
            byte[] bytes = (byte[])DictTool.GetValue<byte, object>(eventData.Parameters, 1);
            PlayerDeadS2CEvt playerDeadS2CEvt = BinSerializer.DeSerialize<PlayerDeadS2CEvt>(bytes);

            BattleSyncMgr.Instance.OnPlayerDeadEvent(playerDeadS2CEvt.deadUsername, playerDeadS2CEvt.killerUsername);

            if (BattleUI.Instance != null) BattleUI.Instance.ShowFlutterText(playerDeadS2CEvt.deadNickName, playerDeadS2CEvt.killerNickName);
        }
    }

}
