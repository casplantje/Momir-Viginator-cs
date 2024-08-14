using MobilePrintinator_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePrintinator.Platforms.Windows
{
    public class ESCPOSDriver : IESCPOSDriver
    {
        public bool isConnected => throw new NotImplementedException();

        public Task Connect()
        {
            throw new NotImplementedException();
        }

        public Task SendDataAsync(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
