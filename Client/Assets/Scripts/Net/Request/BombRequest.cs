using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Net
{
    public class BombRequest : Singleton<BombRequest>
    {

        //发起炸弹信息请求
        public void SendAddBombRequest(BombData bombData)
        {
            ProtoData.AddBombC2S addBombC2S = new ProtoData.AddBombC2S();
            addBombC2S.bombType = (int)bombData.type;
            addBombC2S.durationTime = bombData.durationTime;
            addBombC2S.damageRange = bombData.damageRange;
            addBombC2S.startX = bombData.startPos.x;
            addBombC2S.startY = bombData.startPos.y;
            addBombC2S.startZ = bombData.startPos.z;
            addBombC2S.endX = bombData.endPos.x;
            addBombC2S.endY = bombData.endPos.y;
            addBombC2S.endZ = bombData.endPos.z;
            byte[] bytes = BinSerializer.Serialize(addBombC2S);
            
            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add(1, bytes);

            PhotonEngine.Peer.OpCustom((byte)MessageCode.AddBomb, data, true);//把添加炸弹信息传递给服务器
        }
    }
}

