//-----------------------------------------------------------------------------
// Packet.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

using Lidgren.Network;

namespace Spooker.Network
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class Packet
	{
		private readonly NetworkAgent _netAgent;

		/// <summary>
		/// 
		/// </summary>
		public int PacketId { get; set; }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="netAgent"></param>
		/// <param name="id"></param>
		protected Packet (NetworkAgent netAgent, int id)
		{
			_netAgent = netAgent;
			PacketId = id;
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual void Execute()
		{
			_netAgent.WriteMessage (PacketId);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="connection"></param>
		public void Send(NetConnection connection)
		{
			_netAgent.SendMessage (connection, true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		public virtual void HandleData(NetIncomingMessage data) { }
	}
}