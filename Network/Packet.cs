using System;
using Lidgren.Network;

namespace SFGL.Network
{
	public abstract class Packet
	{
		private NetworkAgent netAgent= null;
		public int PacketID { get; set; }

		public Packet ()
		{
			this.netAgent = null;
			this.PacketID = 0;
		}

		public Packet (NetworkAgent netAgent, int ID)
		{
			this.netAgent = netAgent;
			this.PacketID = ID;
		}

		public virtual void Execute()
		{
			netAgent.WriteMessage (PacketID);
		}

		public void Send(NetConnection connection)
		{
			netAgent.SendMessage (connection, true);
		}

		public virtual void HandleData(NetIncomingMessage data) { }
	}
}