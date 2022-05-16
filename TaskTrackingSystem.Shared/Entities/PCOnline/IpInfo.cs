using System;

namespace TaskTrackingSystem.Shared.Entities.PCOnline
{
    [Serializable]
    public class IpInfo : IComparable
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public string Name { get; set; }
        public string HostName { get; set; }
        public DateTime Time { get; set; }
        public bool IsOnline { get; set; }

        public int CompareTo(object obj)
        {
            var ip1s = Ip.Split('.');
            var ip2s = ((IpInfo)obj).Ip.Split('.');

            int n1, n2;

            for (var i = 0; i <= 3; i++)
            {
                n1 = Convert.ToInt32(ip1s[i]);
                n2 = Convert.ToInt32(ip2s[i]);

                if (n1 < n2)
                    return -1;
                if (n1 > n2)
                    return 1;
            }

            return 0;
        }
    }
}

