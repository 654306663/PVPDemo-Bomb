using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ProtoData;

namespace Net
{
    public class LoginRequest : Singleton<LoginRequest>
    {

        /// <summary>
        /// 发送登录请求
        /// </summary>
        public void SendLoginRequest(string username, string password)
        {
            LoginC2S loginC2S = new LoginC2S();
            loginC2S.username = username;
            loginC2S.password = password;

            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add(1, BinSerializer.Serialize(loginC2S));
            PhotonEngine.Peer.OpCustom((byte)MessageCode.Login, data, true);
        }

        /// <summary>
        /// 发送注册请求
        /// </summary>
        public void SendRegisterRequest(string username, string password)
        {
            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add(1, username);
            data.Add(2, password);
            PhotonEngine.Peer.OpCustom((byte)MessageCode.Register, data, true);

        }
    }
}

