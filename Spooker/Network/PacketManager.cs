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
	/// Packet manager.
	/// </summary>
	public class PacketManager
	{
		private readonly Dictionary<int,Packet> _packets = new Dictionary<int, Packet>();

		/// <summary>
		/// Adds the packet.
		/// </summary>
		/// <returns>The packet.</returns>
		/// <param name="id">Identifier.</param>
		/// <param name="packet">Packet.</param>
		public int AddPacket(int id, Packet packet)
		{
			_packets.Add (id, packet);
			return id;
		}


		/// <summary>
		/// Gets the packet.
		/// </summary>
		/// <returns>The packet.</returns>
		/// <param name="id">Identifier.</param>
		public Packet GetPacket(int id)
		{
			Packet packet;
			_packets.TryGetValue (id, out packet);
			return packet;
		}
	}
}