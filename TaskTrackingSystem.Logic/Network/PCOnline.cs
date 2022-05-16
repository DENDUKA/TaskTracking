using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using EasyWakeOnLan;
using TaskTrackingSystem.Logic.Network.ARP;
using TaskTrackingSystem.Shared.Entities.Network;

namespace TaskTrackingSystem.Logic.Network
{
    public class PCOnline
    {
        public static PCOnline Instance { get; private set; }

        static PCOnline()
        {
            Instance = new PCOnline();
        }

        private static string ipStart = "192.168.1.";

        public List<PCNetworkInfo> GetPCList(bool allIp = false)
        {
            var pcnis = PCNetworkInfoBLL.Instance.GetAll();

            if (allIp)
            {
                AddIpsNotInBD(pcnis);
            }

            Task<List<PCNetworkInfo>> t = new Task<List<PCNetworkInfo>>(() => this.RunScan(pcnis, allIp));
            t.Start();
            t.Wait();
            
            return t.Result;
        }

        private void AddIpsNotInBD(List<PCNetworkInfo> pcnis)
        {
            for (int i = 1; i <= 255; i++)
            {
                var ip = ipStart + i;
                if (!pcnis.Any(x => x.Ip == ip))
                {
                    pcnis.Add(new PCNetworkInfo() { Ip = ip });
                }
            }
        }

        private List<PCNetworkInfo> RunScan(List<PCNetworkInfo> pcnis, bool allIp = false)
        {
            pcnis.ForEach(x => x.IsOnline = false);

            Ping(pcnis);

            Debug.WriteLine("Ping END");

            pcnis = pcnis.Where(x => x.Id != 0 || x.MAC != null || x.IsOnline).ToList();

            Debug.WriteLine("END RunScan");

            return pcnis;
        }

        public List<PCNetworkInfo> RunScanIps(List<PCNetworkInfo> pcnis)
        {
            var task = Task.Run(() => Parallel.ForEach(pcnis, Ping));
            task.Wait();

            return pcnis;
        }

        public bool Ping(string ip)
        {
            PCNetworkInfo pcni = new PCNetworkInfo() { Ip = ip };
            Ping(pcni);

            return pcni.IsOnline;
        }

        private void Ping(List<PCNetworkInfo> pcniList)
        {
            //foreach (var pcni in pcniList)
            //{
            //    Ping(pcni);
            //}

            var task = Task.Run(() => Parallel.ForEach(pcniList, this.Ping));
            task.Wait();
        }

        private void Ping(PCNetworkInfo pcni)
        {
            var ping = new Ping();
            PingReply pingReply = null;

            try
            {
                pingReply = ping.Send(pcni.Ip);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка: {pcni.Ip}  {ex.Message}");
            }
            if (pingReply is null)
            {
                return;
            }

            Debug.WriteLine($"{pcni.Ip} : Staus {pingReply.Status}");

            if (pingReply.Status != IPStatus.TimedOut)
            {
                if (pingReply.Status == IPStatus.Success)
                {
                    try
                    {
                        var ip = IPAddress.Parse(pcni.Ip);
                        var entry = Dns.GetHostEntry(ip);

                        pcni.PCName = entry.HostName;
                        pcni.IsOnline = true;

                        if (pcni.MAC == null)
                        {
                            pcni.MAC = ConvertIpToMAC(pcni.Ip);
                        }

                        Debug.WriteLine($"{pcni.Ip} : {entry.HostName}");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Ошибка: {pcni.Ip} {ex.Message}");
                    }
                }
            }
        }

        public void WakeOnLan(string mac)
        {
            //WOL.WakeOnLan(mac);

            var WOLClient = new EasyWakeOnLanClient();
            WOLClient.Wake(mac);
        }

        public string ConvertIpToMAC(string ipString)
        {
            var ip = IPAddress.Parse(ipString);

            var addr = new byte[6];
            var length = addr.Length;

            SendARP(ip.GetHashCode(), 0, addr, ref length);

            if (addr.Sum(x => x) == 0)
            {
                return null;
            }

            return BitConverter.ToString(addr, 0, 6);
        }

        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        private static extern int SendARP(int DestinationIP, int SourceIP, [Out] byte[] pMacAddr, ref int PhyAddrLen);

        private List<PCNetworkInfo> ConcatInfo(List<PCNetworkInfo> list1, List<PCNetworkInfo> list2)
        {
            var result = new List<PCNetworkInfo>(list1);

            foreach (var ni in result)
            {
                var nif = list2.FirstOrDefault(x => x.Ip == ni.Ip);

                if (nif != null)
                {
                    ni.MAC = nif.MAC;
                }
            }

            var notAdded = list2.Where(x => result.Count(y => y.Ip == x.Ip) == 0);

            result.AddRange(notAdded);

            return result;
        }

        private List<PCNetworkInfo> GetFromARP()
        {
            var pcni = new List<PCNetworkInfo>();
            var arpTable = new ArpTable().ReadArpTable();

            foreach (var arp in arpTable.Where(x => x.IPAddress.ToString().Contains(ipStart)))
            {
                pcni.Add(new PCNetworkInfo() { Ip = arp.IPAddress.ToString(), MAC = arp.MediaAccessControlAddress.ToString() });
            }

            return pcni;
        }
    }
}
