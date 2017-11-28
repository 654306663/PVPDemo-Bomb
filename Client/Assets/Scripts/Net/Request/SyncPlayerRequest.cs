using UnityEngine;
using System.Collections;
using Net;
using System.Collections.Generic;
using ProtoData;

public class SyncPlayerRequest : Singleton<SyncPlayerRequest>
{
    /// <summary>
    /// 发送该玩家加入信息
    /// </summary>
    public void SendSyncAddPlayerRequest()
    {
        AddPlayerC2S playerC2S = new AddPlayerC2S();
        playerC2S.modelName = GlobleHeroData.heroModelName;
        playerC2S.nickName = GlobleHeroData.nickName;
        playerC2S.hp = 100;
        byte[] bytes = BinSerializer.Serialize(playerC2S);

        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add(1, bytes);
        PhotonEngine.Peer.OpCustom((byte)MessageCode.AddPlayer, data, true);
    }

    /// <summary>
    /// 发送该玩家离开信息
    /// </summary>
    public void SendSyncRemovePlayerRequest()
    {
        PhotonEngine.Peer.OpCustom((byte)MessageCode.RemovePlayer, null, true);
    }
}
