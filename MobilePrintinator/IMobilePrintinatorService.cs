using MobilePrintinator_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobilePrintinator
{
    public interface IMobilePrintinatorService
    {
        IMobilePrinter Printer();
        void Configure(PrinterProperties properties);
        void InitConfiguration();
        void SaveConfiguration();
        void LoadConfiguration();
        IEnumerable<string> GetBluetoothPrinterNames();
    }
}