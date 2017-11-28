using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGameServer.Net
{
    class ClientMgr : Singleton<ClientMgr>
    {
        public List<Client> allPeerList = new List<Client>(); // 通过这个集合可以访问到所有客户端的Peer，从而向任何一个客户端发送数据

        private List<Client> battlePeerList = new List<Client>();

        public List<Client> BattlePeerList { get { return battlePeerList; } }

        public void AddBattlePeer(Client client)
        {
            if (!BattlePeerList.Contains(client))
            {
                BattlePeerList.Add(client);
            }
        }

        public void RemoveBattlePeer(Client client)
        {
            if (BattlePeerList.Contains(client))
            {
                BattlePeerList.Remove(client);
            }
        }
    }
}
