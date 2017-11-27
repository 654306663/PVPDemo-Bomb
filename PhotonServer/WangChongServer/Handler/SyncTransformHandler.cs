using MyGameServer.Tools;
using Photon.SocketServer;
using System;

namespace MyGameServer.Handler
{
    class SyncTransformHandler : IHandlerBase
    {
        public void AddListener()
        {
            HandlerMediat.AddListener(MessageCode.SyncTransform, OnSyncTransformReceived);
        }

        public void RemoveListener()
        {
            HandlerMediat.RemoveListener(MessageCode.SyncTransform, OnSyncTransformReceived);
        }

        //获取客户端位置请求的处理的代码
        public void OnSyncTransformReceived(ClientPeer peer, OperationRequest operationRequest, SendParameters sendParameters)
        {

            //接收位置并保持起来
            byte[] bytes = (byte[])DictTool.GetValue<byte, object>(operationRequest.Parameters, 1);
            ProtoData.SyncTransformC2S syncTransformC2S = BinSerializer.DeSerialize<ProtoData.SyncTransformC2S>(bytes);

            peer.x = syncTransformC2S.x;
            peer.y = syncTransformC2S.y;
            peer.z = syncTransformC2S.z;
            peer.angleY = syncTransformC2S.angleY;

        }
    }
}
