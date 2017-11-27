using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Net
{
    public class SyncTransformRequest : Singleton<SyncTransformRequest>
    {

        //发起位置信息请求
        public void SendSyncTransformRequest(Vector3 pos, float angleY)
        {
            ProtoData.SyncTransformC2S syncTransformC2S = new ProtoData.SyncTransformC2S();
            syncTransformC2S.x = pos.x;
            syncTransformC2S.y = pos.y;
            syncTransformC2S.z = pos.z;
            syncTransformC2S.angleY = angleY;
            byte[] bytes = BinSerializer.Serialize(syncTransformC2S);

            //把位置信息x,y,z传递给服务器端
            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add(1, bytes);

            PhotonEngine.Peer.OpCustom((byte)MessageCode.SyncTransform, data, true);//把Player位置传递给服务器
        }
    }
}

