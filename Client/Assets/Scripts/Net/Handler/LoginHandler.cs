using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using System;
using Tools;

namespace Net
{
    public class LoginHandler : HandlerBase
    {

        /// <summary>
        /// 注册监听事件
        /// </summary>
        public override void AddListener()
        {
            HandlerMediat.AddListener(MessageCode.Login, OnLoginReceived);
            HandlerMediat.AddListener(MessageCode.Register, OnRegisterReceived);
        }

        /// <summary>
        /// 移除监听事件
        /// </summary>
        public override void RemoveListener()
        {
            HandlerMediat.RemoveAllListener(MessageCode.Login);
            HandlerMediat.RemoveAllListener(MessageCode.Register);
        }

        /// <summary>
        /// 收到登录消息
        /// </summary>
        void OnLoginReceived(OperationResponse response)
        {
            ReturnCode returnCode = (ReturnCode)response.ReturnCode;
            if (returnCode == ReturnCode.Success)
            {
                LoginHintTextUI.Instance.SetText("用户名和密码验证成功", Color.green);
                Debug.LogError("用户名和密码验证成功");

                byte[] bytes = DictTool.GetValue<byte, object>(response.Parameters, 1) as byte[];

                ProtoData.LoginS2C loginS2C = BinSerializer.DeSerialize<ProtoData.LoginS2C>(bytes);
                GlobleHeroData.username = loginS2C.username;
                //验证成功，跳转到下一个场景
                SceneMgr.LoadScene(SceneType.ChooseScene);

            }
            else if (returnCode == ReturnCode.Failed)
            {
                LoginHintTextUI.Instance.SetText("用户名或密码错误", Color.red);
                Debug.LogError("用户名或密码错误");
            }
        }

        /// <summary>
        /// 收到注册消息
        /// </summary>
        void OnRegisterReceived(OperationResponse response)
        {
            ReturnCode returnCode = (ReturnCode)response.ReturnCode;
            if (returnCode == ReturnCode.Success)
            {
                LoginHintTextUI.Instance.SetText("注册成功，请返回登陆", Color.green);
                Debug.LogError("注册成功，请返回登陆");

            }
            else if (returnCode == ReturnCode.Failed)
            {
                LoginHintTextUI.Instance.SetText("所用的用户名已被注册，请更改用户名", Color.red);
                Debug.LogError("所用的用户名已被注册，请更改用户名");
            }
        }
    }
}
