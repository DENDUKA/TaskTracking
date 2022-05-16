using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using TaskTrackingSystem.Logic.Network;
using TaskTrackingSystem.Shared.Entities.Network;

namespace TaskTrackingSystem.SignalR.Hubs
{
    public class WolHub : Hub
    {
        int i = 0;

        public WolHub()
        {    
        }

        public void Connect()
        {
            
        }

        public List<PCNetworkInfo> GetPCList(bool scanAll)
        {
            var result = PCOnline.Instance.GetPCList(scanAll);

            return result;
        }
        
        public void SendUpdatedDate(List<PCNetworkInfo> pcnis)
        {
            Debug.WriteLine($"SendUpdatedDate");

            i++;
            var x = Clients.Client(connectionId);
            Clients.Client(connectionId).getUpdates(pcnis);
           
        }

        public void WOL(string mac)
        {
            PCOnline.Instance.WakeOnLan(mac);
        }

        public object SavePCNI(int id, string mac, string ip, string pcName, string accLogin)
        {
            var pcni = new PCNetworkInfo() { Id = id, AccountLogin = accLogin, Ip = ip, MAC = mac, PCName = pcName };

            Logic.Logic.Instance.PCNetworkInfo.Save(pcni);

            return new { pcni.Id, pcni.Ip };
        }

        public int Delete(int id)
        {
            if (id == 0)
            {
                return -1;
            }

            PCNetworkInfoBLL.Instance.Delete(id);

            return id;
        }

        public object UpdateStatusByIp (string ip)
        {
            var status = PCOnline.Instance.Ping(ip);

            return new { ip, status };
        }

        public object UpdateStatus(string[] ips)
        {
            List<PCNetworkInfo> pcnis = PCOnline.Instance.RunScanIps(ips.Select(x => new PCNetworkInfo() { Ip = x }).ToList());


            return pcnis.Select(x => new { x.Ip, x.IsOnline });
        }


        private readonly static List<string> connections = new List<string>();

        private string connectionId;

        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;

            connections.Add(Context.ConnectionId);

            Debug.WriteLine($"Connected {Context.ConnectionId} подключений {connections.Count}");

            this.connectionId = Context.ConnectionId;

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;

            connections.Remove(Context.ConnectionId);

            Debug.WriteLine($"Disconnected {Context.ConnectionId} подключений {connections.Count}");

            return base.OnDisconnected(stopCalled);
        }
    }
}