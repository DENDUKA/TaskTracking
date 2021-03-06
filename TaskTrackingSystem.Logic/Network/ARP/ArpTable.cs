using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using TaskTrackingSystem.Shared.Entities.Network;

namespace TaskTrackingSystem.Logic.Network.ARP
{
    /// <summary>
    ///		Reads the ARP table, used for storing IP address/MAC address pairings.
    /// </summary>
    public class ArpTable
	{
		/// <summary>
		///		The GetIpNetTable function retrieves the IP-to-physical address mapping 
		///		table.
		/// </summary>
		/// <param name="pIpNetTable">
		///		Pointer to a buffer that receives the IP-to-physical address mapping 
		///		table.
		///	</param>
		/// <param name="pdwSize">
		///		On input, specifies the size of the buffer pointed to by the 
		///		pIpNetTable parameter.
		///		On output, if the buffer is not large enough to hold the returned 
		///		mapping table, the function sets this parameter equal to the required 
		///		buffer size.
		///	</param>
		/// <param name="bOrder">
		///		Specifies whether the returned mapping table should be sorted in 
		///		ascending order by IP address. If this parameter is TRUE, the table is 
		///		sorted.
		///	</param>
		/// <returns>
		///		If the function succeeds, the return value is NO_ERROR.
		///	</returns>
		[DllImport("Iphlpapi.dll", SetLastError = true)]
		private static extern int GetIpNetTable(
			IntPtr pIpNetTable,
			ref int pdwSize,
			bool bOrder);

		/// <summary>
		///		The FlushIpNetTable function deletes all ARP entries for the specified 
		///		interface from the ARP table on the local computer.
		/// </summary>
		/// <param name="dwIfIndex">
		///		Index of the interface for which to delete all ARP entries.
		///	</param>
		/// <returns>
		///		If the function succeeds, the return value is NO_ERROR.
		///	</returns>
		[DllImport("Iphlpapi.dll", SetLastError = true)]
		private static extern int FlushIpNetTable(
			int dwIfIndex);

		/// <summary>
		///		The DeleteIpNetEntry function deletes an ARP entry from the ARP table 
		///		on the local computer.
		/// </summary>
		/// <param name="pArpEntry">
		///		Pointer to a MIB_IPNETROW structure. The information in this structure 
		///		specifies the entry to delete. The caller must specify values for at least 
		///		the dwIndex and dwAddr members of this structure.
		///	</param>
		/// <returns>
		///		If the function succeeds, the return value is NO_ERROR.
		///	</returns>
		[DllImport("Iphlpapi.dll", SetLastError = true)]
		private static extern int DeleteIpNetEntry(
			IntPtr pArpEntry);

		/// <summary>
		///		The SetIpNetEntry function modifies an existing ARP entry in the ARP 
		///		table on the local computer.
		/// </summary>
		/// <param name="pArpEntry">
		///		Pointer to a MIB_IPNETROW structure. The information in this structure 
		///		specifies the entry to modify and the new information for the entry. The 
		///		caller must specify values for all members of this structure.
		///	</param>
		/// <returns>
		///		If the function succeeds, the return value is NO_ERROR.
		///	</returns>
		[DllImport("Iphlpapi.dll", SetLastError = true)]
		private static extern int SetIpNetEntry(
			IntPtr pArpEntry);

		/// <summary>
		///		The CreateIpNetEntry function creates an Address Resolution Protocol 
		///		(ARP) entry in the ARP table on the local computer.
		/// </summary>
		/// <param name="pArpEntry">
		///		 Pointer to a MIB_IPNETROW structure that specifies information for 
		///		the new entry. The caller must specify values for all members of this 
		///		structure.
		///	</param>
		/// <returns>
		///		If the function succeeds, the return value is NO_ERROR.
		///	</returns>
		[DllImport("Iphlpapi.dll", SetLastError = true)]
		private static extern int CreateIpNetEntry(
			IntPtr pArpEntry);

		/// <summary>
		///		The CreateProxyArpEnry function creates a Proxy Address Resolution 
		///		Protocol (PARP) entry on the local computer for the specified IP 
		///		address.
		/// </summary>
		/// <param name="dwAddress">
		///		IP address for which this computer acts as a proxy.
		///</param>
		/// <param name="dwMask">
		///		Subnet mask for the IP address specified in dwAddress.
		///	</param>
		/// <param name="dwIfIndex">
		///		Index of the interface on which to proxy ARP for the IP address 
		///		identified by dwAddress. In other words, when an ARP request for 
		///		dwAddress is received on this interface, the local computer responds 
		///		with the physical address of this interface. If this interface is of a 
		///		type that does not support ARP, such as PPP, then the call fails.
		///	</param>
		/// <returns>
		///		If the function succeeds, the return value is NO_ERROR.
		///	</returns>
		[DllImport("Iphlpapi.dll", SetLastError = true)]
		private static extern int CreateProxyArpEntry(
			int dwAddress,
			int dwMask,
			int dwIfIndex);

		/// <summary>
		///		The DeleteProxyArpEntry function deletes the PARP entry on the local 
		///		computer specified by the dwAddress and dwIfIndex parameters.
		/// </summary>
		/// <param name="dwAddress">
		///		IP address for which this computer acts as a proxy.
		///</param>
		/// <param name="dwMask">
		///		Subnet mask for the IP address specified in dwAddress.
		///	</param>
		/// <param name="dwIfIndex">
		///		Index of the interface on which this computer is supporting proxy ARP 
		///		for the IP address specified by dwAddress.
		///	</param>
		/// <returns>
		///		If the function succeeds, the return value is NO_ERROR.
		///	</returns>
		[DllImport("Iphlpapi.dll", SetLastError = true)]
		private static extern int DeleteProxyArpEntry(
			int dwAddress,
			int dwMask,
			int dwIfIndex);

		/// <summary>
		///		Insufficient buffer space.
		/// </summary>
		private const int ERROR_INSUFFICIENT_BUFFER = 122;

		/// <summary>
		///		Read the ARP table. Returns null if reading failed.
		/// </summary>
		public List<ArpEntry> ReadArpTable()
		{
			// pointer to the block of memory we will use
			IntPtr pBuffer = IntPtr.Zero;

			// size of the buffer
			int bufferSize = 0;

			// return value
			int ret = 0;

			// make an initial call to the method to recieve the correct buffer
			// size
			ret = GetIpNetTable(pBuffer, ref bufferSize, true);

			// if an error other than insufficient buffer size occurs then return
			// null
			if (ret != ERROR_INSUFFICIENT_BUFFER)
			{
				Marshal.FreeHGlobal(pBuffer);
				return null;
			}

			// now we know how much memory to allocate, allocate it
			pBuffer = Marshal.AllocHGlobal(bufferSize);

			// make the real call now
			ret = GetIpNetTable(pBuffer, ref bufferSize, true);

			// check for failure
			if (ret != 0)
			{
				Marshal.FreeHGlobal(pBuffer);
				return null;
			}

			// the first 4 bytes is the number of entries
			int nEntries = Marshal.ReadInt32(new IntPtr(pBuffer.ToInt32()));

            // allocate space for each entry
            List<ArpEntry> entries = new List<ArpEntry>();

			// loop through each entry
			for (int i = 0; i < nEntries; i++)
			{
				var entr = new ArpEntry();

                // read the entry type
                entr.EntryType = (ArpEntryType)Marshal.ReadInt32(new IntPtr(pBuffer.ToInt32() + (24 * i) + 24));

                if (entr.EntryType == ArpEntryType.Invalid)
                {
                    continue;
                }

                // read the adapter index
                entr.AdapterIndex = Marshal.ReadInt32(new IntPtr(pBuffer.ToInt32() + (24 * i) + 4));

				// read the physical address
				int addressLength = Marshal.ReadInt32(new IntPtr(pBuffer.ToInt32() + (24 * i) + 8));
				byte[] macAddress = new byte[addressLength];

				for (int j = 0; j < addressLength; j++)
				{
					macAddress[j] = Marshal.ReadByte(new IntPtr(pBuffer.ToInt32() + (24 * i) + 12 + j));
				}

                entr.MediaAccessControlAddress = new MACAddress(macAddress);

				// read the IP address
				try
				{
                    entr.IPAddress = new IPAddress(Marshal.ReadInt32(new IntPtr(pBuffer.ToInt32() + (24 * i) + 20)));
				}
				catch (Exception ex)
				{
                    
                }

                entries.Add(entr);
            }

			// free the allocated memory
			Marshal.FreeHGlobal(pBuffer);

            return entries;
		}


		/// <summary>
		///		Deletes all ARP entries for the specified interface from the ARP table 
		///		on the local computer.
		/// </summary>
		/// <param name="adapterIndex">
		///		Index of the interface for which to delete all ARP entries.
		///	</param>
		/// <returns>
		///		Returns true for success, false otherwise.
		///	</returns>
		public bool FlushArpTable(int adapterIndex)
		{
			return FlushIpNetTable(adapterIndex) == 0;
		}

		/// <summary>
		///		Deletes an ARP entry from the ARP table on the local computer.
		/// </summary>
		/// <param name="entry">
		///		The entry to delete. This must at least have valid fields for the
		///		adapter index and IP address.
		///	</param>
		/// <returns>
		///		Returns true for success, false otherwise.
		///	</returns>
		public bool DeleteArpEntry(ArpEntry entry)
		{
			bool ret = false;

			// allocate 24 bytes for the entry
			IntPtr pArpEntry = Marshal.AllocHGlobal(24);

			Marshal.WriteInt32(pArpEntry, entry.AdapterIndex);
			Marshal.WriteInt32(new IntPtr(pArpEntry.ToInt32() + 4), entry.MediaAccessControlAddress.Address.Length);

			// write the MAC address
			for (int i = 0; i < entry.MediaAccessControlAddress.Address.Length; i++)
			{
				Marshal.WriteByte(new IntPtr(pArpEntry.ToInt32() + 8 + i), entry.MediaAccessControlAddress.Address[i]);
			}

			// pad out the MAC address to 8 bytes
			for (int i = 0; i < 8 - entry.MediaAccessControlAddress.Address.Length; i++)
			{
				Marshal.WriteByte(new IntPtr(pArpEntry.ToInt32() + 8 + i), 0);
			}

			// write the IP address			
			byte[] ip = entry.IPAddress.GetAddressBytes();

			for (int i = 0; i < ip.Length; i++)
			{
				Marshal.WriteByte(new IntPtr(pArpEntry.ToInt32() + 16 + i), ip[i]);
			}

			Marshal.WriteInt32(new IntPtr(pArpEntry.ToInt32() + 20), (int)entry.EntryType);

			// try to delete the entry
			ret = DeleteIpNetEntry(pArpEntry) == 0;

			// free the allocated memory
			Marshal.FreeHGlobal(pArpEntry);

			return ret;
		}


		/// <summary>
		///		Modifies an existing ARP entry in the ARP table on the local computer.
		/// </summary>
		/// <param name="entry">
		///		The entry to modify. All fields must be valid.
		///	</param>
		/// <returns>
		///		Returns true for success, false otherwise.
		///	</returns>
		public bool SetArpEntry(ArpEntry entry)
		{
			bool ret = false;

			// allocate 24 bytes for the entry
			IntPtr pArpEntry = Marshal.AllocHGlobal(24);

			Marshal.WriteInt32(pArpEntry, entry.AdapterIndex);
			Marshal.WriteInt32(new IntPtr(pArpEntry.ToInt32() + 4), entry.MediaAccessControlAddress.Address.Length);

			// write the MAC address
			for (int i = 0; i < entry.MediaAccessControlAddress.Address.Length; i++)
			{
				Marshal.WriteByte(new IntPtr(pArpEntry.ToInt32() + 8 + i), entry.MediaAccessControlAddress.Address[i]);
			}

			// pad out the MAC address to 8 bytes
			for (int i = 0; i < 8 - entry.MediaAccessControlAddress.Address.Length; i++)
			{
				Marshal.WriteByte(new IntPtr(pArpEntry.ToInt32() + 8 + i), 0);
			}

			// write the IP address			
			byte[] ip = entry.IPAddress.GetAddressBytes();

			for (int i = 0; i < ip.Length; i++)
			{
				Marshal.WriteByte(new IntPtr(pArpEntry.ToInt32() + 16 + i), ip[i]);
			}

			Marshal.WriteInt32(new IntPtr(pArpEntry.ToInt32() + 20), (int)entry.EntryType);

			// try to modify the entry
			ret = SetIpNetEntry(pArpEntry) == 0;

			// free the allocated memory
			Marshal.FreeHGlobal(pArpEntry);

			return ret;
		}


		/// <summary>
		///		Creates an Address Resolution Protocol (ARP) entry in the ARP table on 
		///		the local computer.
		/// </summary>
		/// <param name="entry">
		///		The entry to add. All fields must be valid.
		///	</param>
		/// <returns>
		///		Returns true for success, false otherwise.
		///	</returns>
		public bool CreateArpEntry(ArpEntry entry)
		{
			bool ret = false;

			// allocate 24 bytes for the entry
			IntPtr pArpEntry = Marshal.AllocHGlobal(24);

			Marshal.WriteInt32(pArpEntry, entry.AdapterIndex);
			Marshal.WriteInt32(new IntPtr(pArpEntry.ToInt32() + 4), entry.MediaAccessControlAddress.Address.Length);

			// write the MAC address
			for (int i = 0; i < entry.MediaAccessControlAddress.Address.Length; i++)
			{
				Marshal.WriteByte(new IntPtr(pArpEntry.ToInt32() + 8 + i), entry.MediaAccessControlAddress.Address[i]);
			}

			// pad out the MAC address to 8 bytes
			for (int i = 0; i < 8 - entry.MediaAccessControlAddress.Address.Length; i++)
			{
				Marshal.WriteByte(new IntPtr(pArpEntry.ToInt32() + 8 + i), 0);
			}

			// write the IP address			
			byte[] ip = entry.IPAddress.GetAddressBytes();

			for (int i = 0; i < ip.Length; i++)
			{
				Marshal.WriteByte(new IntPtr(pArpEntry.ToInt32() + 16 + i), ip[i]);
			}

			Marshal.WriteInt32(new IntPtr(pArpEntry.ToInt32() + 20), (int)entry.EntryType);

			// try to modify the entry
			ret = CreateIpNetEntry(pArpEntry) == 0;

			// free the allocated memory
			Marshal.FreeHGlobal(pArpEntry);

			return ret;
		}


		/// <summary>
		///		The CreateProxyArpEnry function creates a Proxy Address Resolution 
		///		Protocol (PARP) entry on the local computer for the specified IP 
		///		address.
		/// </summary>
		/// <param name="address">
		///		IP address for which this computer acts as a proxy.
		///</param>
		/// <param name="mask">
		///		Subnet mask for the IP address specified in dwAddress.
		///	</param>
		/// <param name="adapterIndex">
		///		Index of the interface on which to proxy ARP for the IP address 
		///		identified by dwAddress. In other words, when an ARP request for 
		///		dwAddress is received on this interface, the local computer responds 
		///		with the physical address of this interface. If this interface is of a 
		///		type that does not support ARP, such as PPP, then the call fails.
		///	</param>
		/// <returns>
		///		If the function succeeds, the return value is true, or false otherwise
		///	</returns>
		public bool CreateProxyArpEntry(IPAddress address, IPAddress mask, int adapterIndex)
		{
			return CreateProxyArpEntry((int)address.Address, (int)mask.Address, adapterIndex) == 0;
		}


		/// <summary>
		///		The DeleteProxyArpEntry function deletes the PARP entry on the local 
		///		computer specified by the dwAddress and dwIfIndex parameters.
		/// </summary>
		/// <param name="address">
		///		IP address for which this computer acts as a proxy.
		///</param>
		/// <param name="mask">
		///		Subnet mask for the IP address specified in dwAddress.
		///	</param>
		/// <param name="adapterIndex">
		///		Index of the interface on which this computer is supporting proxy ARP 
		///		for the IP address specified by dwAddress.
		///	</param>
		/// <returns>
		///		If the function succeeds, the return value is NO_ERROR.
		///	</returns>
		public bool DeleteProxyArpEntry(IPAddress address, IPAddress mask, int adapterIndex)
		{
			return DeleteProxyArpEntry((int)address.Address, (int)mask.Address, adapterIndex) == 0;
		}
	}
}
