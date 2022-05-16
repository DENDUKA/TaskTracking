using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTrackingSystem.Shared.Entities.Network
{
    public class PCNetworkInfo
    {
        public int Id { get; set; }
        public string AccountLogin { get; set; }
        [ForeignKey("AccountLogin")]        
        public Account Account { get; set; }
        public string Ip { get; set; }
        public string MAC { get; set; }
        public string PCName { get; set; }
        public bool IsOnline { get; set; }
    }
}
