using System.Net;

namespace TaskTrackingSystem.Shared.Entities.Network
{
    public class ArpEntry
	{
		/// <summary>
		///		The adapter this entry belongs to.
		/// </summary>
		private int m_adapterIndex;

		/// <summary>
		///		The physical address in the entry.
		/// </summary>
		private MACAddress m_macAddress = MACAddress.BroadcastAddress;

		/// <summary>
		///		The IP address in the entry.
		/// </summary>
		private IPAddress m_ipAddress = IPAddress.Any;

		/// <summary>
		///		The type of this entry.
		/// </summary>
		private ArpEntryType m_type = ArpEntryType.Invalid;

		/// <summary>
		///		The adapter this entry is associated with.
		/// </summary>
		public int AdapterIndex
        {
            get => m_adapterIndex;
            set => m_adapterIndex = value;
        }


        /// <summary>
        ///		The physical address.
        /// </summary>
        public MACAddress MediaAccessControlAddress
		{
			get
			{
				return m_macAddress;
			}
			set
			{
				m_macAddress = value;
			}
		}


		/// <summary>
		///		The IP address.
		/// </summary>	
		public IPAddress IPAddress
		{
			get
			{
				return m_ipAddress;
			}
			set
			{
				m_ipAddress = value;
			}
		}


		/// <summary>
		///		The entry type.
		/// </summary>
		public ArpEntryType EntryType
		{
			get
			{
				return m_type;
			}
			set
			{
				m_type = value;
			}
		}
	}
}
