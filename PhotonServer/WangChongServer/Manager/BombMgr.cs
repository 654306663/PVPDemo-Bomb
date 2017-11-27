using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGameServer.Manager
{
    class BombMgr : Singleton<BombMgr>
    {

        int bombId = 0;
        public int GetBombId()
        {
            return bombId++;
        }

    }
}
