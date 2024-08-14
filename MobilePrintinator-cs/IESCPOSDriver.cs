using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobilePrintinator_cs
{
    public interface IESCPOSDriver
    {
        Task SendDataAsync(byte[] data);
        Task Connect();
        bool isConnected { get; }
    }
}