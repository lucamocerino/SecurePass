using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide
{
    class RunServer
    {
        
        public static int Main(String[] args)
        {
           ServerS server = new ServerS();
            server.StartListening();
            
            return 0;
        }
         
         
         
    }
}
