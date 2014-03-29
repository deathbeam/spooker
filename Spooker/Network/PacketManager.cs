//-----------------------------------------------------------------------------
// PacketManager.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

using System.Collections.Generic;

namespace Spooker.Network
{
	/// <summary>
	/// 
	/// </summary>
	public class PacketManager
	{
		private readonly Dictionary<int,Packet> _packets = new Dictionary<int, Packet>();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="packet"></param>
		/// <returns></returns>
		public int AddPacket(Packet packet)
		{
			var id = packet.PacketId;
			_packets.Add (id, packet);
			return id;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Packet GetPacket(int id)
		{
			Packet packet;
			_packets.TryGetValue (id, out packet);
			return packet;
		}
	}
}