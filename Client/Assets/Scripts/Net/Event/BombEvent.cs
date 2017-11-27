using UnityEngine;
using ExitGames.Client.Photon;
using Tools;
using System;

namespace Net
{
    public class BombEvent : EventBase
    {
        public override void AddListener()
        {
            EventMediat.AddListener(MessageCode.AddBomb, OnAddBombReceived);
            EventMediat.AddListener(MessageCode.OpenBomb, OnOpenBombReceived);
        }

        public override void RemoveListener()
        {
            EventMediat.RemoveListener(MessageCode.AddBomb, OnAddBombReceived);
            EventMediat.RemoveListener(MessageCode.OpenBomb, OnOpenBombReceived);
        }
        
        void OnAddBombReceived(EventData eventData)
        {
            byte[] bytes = (byte[])DictTool.GetValue<byte, object>(eventData.Parameters, 1);

            ProtoData.AddBombS2CEvt addBombS2CEvt = BinSerializer.DeSerialize<ProtoData.AddBombS2CEvt>(bytes);
            BombData bombData = new BombData();
            bombData.username = addBombS2CEvt.username;
            bombData.type = (BombType)addBombS2CEvt.bombType;
            bombData.id = addBombS2CEvt.bombId;
            bombData.startPos = new Vector3(addBombS2CEvt.startX, addBombS2CEvt.startY, addBombS2CEvt.startZ);
            bombData.endPos = new Vector3(addBombS2CEvt.endX, addBombS2CEvt.endY, addBombS2CEvt.endZ);

            BattleMgr.Instance.bombMgr.AddBomb(bombData);
        }


        void OnOpenBombReceived(EventData eventData)
        {
            byte[] bytes = (byte[])DictTool.GetValue<byte, object>(eventData.Parameters, 1);

            ProtoData.OpemBombS2CEvt openBombS2CEvt = BinSerializer.DeSerialize<ProtoData.OpemBombS2CEvt>(bytes);

            BattleMgr.Instance.bombMgr.OpenBomb(openBombS2CEvt);
        }
    }
}
