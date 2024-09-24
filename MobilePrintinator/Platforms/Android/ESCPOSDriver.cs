using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Bluetooth;
using System.Text;
using System.Diagnostics;
using MobilePrintinator_cs;
using System.Text.RegularExpressions;

namespace MobilePrintinator.Platforms.Android
{
    public class ESCPOSDriver : IESCPOSDriver
    {
        private BluetoothAdapter m_adapter;
        private BluetoothSocket m_socket;
        private string m_name;

        public ESCPOSDriver(string name)
        {
            m_adapter = BluetoothAdapter.DefaultAdapter;
            m_name = name;
        }

        ~ESCPOSDriver()
        {
             m_socket?.Close();
        }

        public bool isConnected => m_socket?.IsConnected ?? false;

        public async Task Connect()
        {
            var device = m_adapter.BondedDevices.FirstOrDefault(d => d.Name.StartsWith(m_name));
            if (device != null)
            {
                m_socket = device.CreateRfcommSocketToServiceRecord(Java.Util.UUID.FromString("00001101-0000-1000-8000-00805F9B34FB"));
                try
                {
                    await m_socket?.ConnectAsync();
                } catch {
                    m_socket?.Close();
                    m_socket = null;
                }
            }
        }

        public async Task SendDataAsync(byte[] data)
        {
            if (isConnected)
            {
                var cts = new CancellationTokenSource(500);
                int offset = 0;

                while (offset < data.Length)
                {
                    int count = Math.Min(4096, data.Length - offset);
                    try
                    {
                        await m_socket.OutputStream.WriteAsync(data, offset, count, cts.Token);
                    } catch (Exception ex)
                    {
                        Debug.WriteLine("Stream write timed out: " + ex.ToString);
                    }
                    offset += count;
                }
                await m_socket.OutputStream.FlushAsync();
            }
        }

        public IEnumerable<string> GetBluetoothPrinterNames()
        {
            string pattern = "PT-2[0-9]{2}_[0-9A-F]+";
            List<String> result = new List<String>();
            if (m_adapter != null && m_adapter.BondedDevices != null)
            {
                foreach (BluetoothDevice device in m_adapter.BondedDevices)
                {
                    if (Regex.IsMatch(device.Name, pattern))
                        yield return device.Name;
                }
            }
        }
    }
}