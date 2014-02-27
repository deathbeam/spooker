using System;
using System.Collections.Generic;
using Lidgren.Network;

namespace SFGL.Network
{
	public class PacketManager
	{
		private Dictionary<int,Packet> packets = new Dictionary<int, Packet>();

		public int AddPacket(Packet packet)
		{
			var id = packet.PacketID;
			packets.Add (id, packet);
			return id;
		}

		public Packet GetPacket(int id)
		{
			Packet packet;
			packets.TryGetValue (id, out packet);
			return packet;
		}
	}
}