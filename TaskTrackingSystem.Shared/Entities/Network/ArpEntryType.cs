namespace TaskTrackingSystem.Shared.Entities.Network
{
    public enum ArpEntryType : byte
	{
		/// <summary>
		///		Other.
		/// </summary>
		Other = 1,

		/// <summary>
		///		Invalid.
		/// </summary>
		Invalid,

		/// <summary>
		///		Dynamic.
		/// </summary>
		Dynamic,

		/// <summary>
		///		Static.
		/// </summary>
		Static
	}
}
