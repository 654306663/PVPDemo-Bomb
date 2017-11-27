using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Net
{
    public class SyncTransitionRequest : Singleton<SyncTransitionRequest>
    {

        //发起位置信息请求
        public void SendSyncTransitionRequest(FSMTransition transition, params object[] objs)
        {
            ProtoData.SyncTransitionC2S syncTransitionC2S = new ProtoData.SyncTransitionC2S();
            syncTransitionC2S.targetTransition = (int)transition;
            byte[] bytes = BinSerializer.Serialize(syncTransitionC2S);

            //把位置信息x,y,z传递给服务器端
            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add(1, bytes);
            data.Add(2, objs);

            PhotonEngine.Peer.OpCustom((byte)MessageCode.SyncTransition, data, true);//把Player位置传递给服务器
        }
    }
}

