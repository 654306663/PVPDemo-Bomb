using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGameServer.Handler
{
    interface IHandlerBase
    {
        void AddListener(); 

        void RemoveListener();
    }
}
