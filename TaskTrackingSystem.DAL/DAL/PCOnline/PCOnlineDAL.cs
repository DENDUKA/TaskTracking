using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TaskTrackingSystem.AbstractDAL.PCOnline;
using TaskTrackingSystem.Shared.Entities.PCOnline;

namespace TaskTrackingSystem.DAL.DAL.PCOnline
{
    public class PCOnlineDAL : IPCOnlineDAL
    {
        private static readonly string DirectoryPath = @"D:\WebApplication\PCOnline\Logs";
        private static readonly string CurrentStatePath = @"CurrentState";

        public List<IpInfo> GetCurrentStatus()
        {
            var path = Path.Combine(DirectoryPath, CurrentStatePath);

            var result = new List<IpInfo>();

            if (File.Exists(path))
            {
                var fileText = File.ReadAllText(path);

                try
                {
                    result = JsonConvert.DeserializeObject<List<IpInfo>>(fileText);
                }
                catch (Exception)
                {

                }
            }

            return result;
        }
    }
}
