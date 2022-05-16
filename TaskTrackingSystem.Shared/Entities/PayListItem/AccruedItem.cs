using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTrackingSystem.Shared.Entities.PayListItem
{
    public class AccruedItem
    {
        public string Type { get; set; }
        public int Days { get; set; }
        public int Hours { get; set; }
        public string Period { get; set; }
        public float Summa { get; set; }
    }
}
