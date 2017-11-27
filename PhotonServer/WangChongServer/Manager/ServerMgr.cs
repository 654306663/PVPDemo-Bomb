using MyGameServer.Handler;
using MyGameServer.Threads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGameServer.Manager
{
    class ServerMgr : Singleton<ServerMgr>
    {

        public void OnServerOpen()
        {
            TimerThread.Start();
            AddHandler();
            AddUpdate();
        }

        public void OnServerClose()
        {
            RemoveHandler();
            RemoveUpdate();
            TimerThread.Stop();

        }

        #region 所有Handler
        LoginHandler loginHandler;
        SyncTransformHandler syncTransformHandler;
        SyncPlayerHandler syncPlayerHandler;
        SyncTransitionHandler syncTransitionHandler;
        BombHandler bombHandler;

        // 初始化所有Handler
        void AddHandler()
        {
            loginHandler = new LoginHandler();
            loginHandler.AddListener();

            syncTransformHandler = new SyncTransformHandler();
            syncTransformHandler.AddListener();

            syncPlayerHandler = new SyncPlayerHandler();
            syncPlayerHandler.AddListener();

            syncTransitionHandler = new SyncTransitionHandler();
            syncTransitionHandler.AddListener();

            bombHandler = new BombHandler();
            bombHandler.AddListener();
        }

        // 移除所有Handler
        void RemoveHandler()
        {
            loginHandler.RemoveListener();
            syncTransformHandler.RemoveListener();
            syncPlayerHandler.RemoveListener();
            syncTransitionHandler.RemoveListener();
            bombHandler.RemoveListener();
        }
        #endregion

        #region 所有Update
        SyncTransformUpdate syncTransformUpdate = new SyncTransformUpdate();
        public BombUpdate bombUpdate = new BombUpdate();

        void AddUpdate()
        {
            TimerThread.Action += syncTransformUpdate.Updata;
            TimerThread.Action += bombUpdate.Update;
        }

        void RemoveUpdate()
        {
            TimerThread.Action -= syncTransformUpdate.Updata;
            TimerThread.Action -= bombUpdate.Update;
        }
        #endregion
    }
}
