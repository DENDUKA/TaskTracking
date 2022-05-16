using System;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using TaskTrackingSystem.Logic;
using TaskTrackingSystem.Models;
using TaskTrackingSystem.Shared.Entities;

namespace TaskTrackingSystem.SignalR.Hubs
{
    public class PremiumHub : Hub
    {
        public string Connect()
        {
            var xxxx = ProjectBLL.Instance.GetAllForProjectType(1);
            var prjs = ProjectBLL.Instance.GetAllForProjectType(1).Where(x => x.StatusId == Status.DoneId).Select(o => new { projecttype = o.ProjectType.Name, name = o.Name, cost = o.Cost, acc = o.Account.Name }).ToList();
            var yy = "";
            try
            {
                yy = JsonConvert.SerializeObject(prjs);
            }
            catch (Exception ex)
            {
            }
            return yy;
        }

        public bool MarkAsPaid(int id)
        {
            var result = ProjectBLL.Instance.MarkAsPaid(id);
            return result;
        }
    }
}