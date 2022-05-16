using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTrackingSystem.Shared.Entities.PayListItem
{
    public class PaidItem
    {
        public string Type { get; set; }
        public string Period { get; set; }
        public float Summa { get; set; }
    }
}
