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
		
		public int PacketId;

		protected Packet (NetworkAgent parent, int id)
		{
			_netAgent = parent;
			PacketId = id;
			_netAgent.Packets.AddPacket (this);
		}
		
		public virtual void Execute()
		{
			_netAgent.WriteMessage (PacketId);
		}
		
		public void Send(NetConnection connection)
		{
			_netAgent.SendMessage (connection, true);
		}
		
		public virtual void HandleData(NetIncomingMessage data)
		{
		}
	}
}