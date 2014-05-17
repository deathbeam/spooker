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
		/// Gets the <see cref="Spooker.Network.Packet"/> with the specified key.
		/// </summary>
		/// <param name="key">Key.</param>
		public Packet this[int key]
		{
			get
			{
				Packet packet;
				_packets.TryGetValue (key, out packet);
				return packet;
			}
		}

		/// <summary>
		/// Adds the packet.
		/// </summary>
		/// <returns>The packet.</returns>
		/// <param name="id">Identifier.</param>
		/// <param name="packet">Packet.</param>
		public int Add(int id, Packet packet)
		{
			_packets.Add (id, packet);
			return id;
		}

		/// <summary>
		/// Remove packet with the specified id.
		/// </summary>
		/// <param name="id">Identifier.</param>
		public void Remove(int id)
		{
			_packets.Remove (id);
		}
	}
}