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
	/// Packet.
	/// </summary>
	public abstract class Packet
	{
		private readonly NetworkAgent _parent;

		/// <summary>
		/// The writer used for writing data for this packet.
		/// </summary>
		protected PacketWriter Writer;

		/// <summary>
		/// The reader used for reading data from received message.
		/// </summary>
		protected PacketReader Reader;

		/// <summary>
		/// Gets the ID of this packet.
		/// </summary>
		/// <value>The ID.</value>
		protected abstract int ID { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Network.Packet"/> class.
		/// </summary>
		/// <param name="parent">Parent.</param>
		protected Packet (NetworkAgent parent)
		{
			_parent = parent;
			_parent.PacketManager.AddPacket (ID, this);
			Writer = new PacketWriter ();
			Reader = new PacketReader ();
		}

		/// <summary>
		/// Send the packet to specified connection.
		/// </summary>
		/// <param name="connection">Connection.</param>
		public void Send(NetConnection connection)
		{
			_parent.Write (Writer);
			_parent.SendMessage (connection, true);
		}

		/// <summary>
		/// Execute this packet.
		/// </summary>
		public virtual void Execute()
		{
			Writer.Reset ();
			Writer.Write (ID);
		}

		/// <summary>
		/// Handles the data.
		/// </summary>
		/// <param name="data">Data.</param>
		public virtual void HandleData(NetIncomingMessage data)
		{
			Reader.Reset (data.LengthBytes);
			Reader.Data = data.Data;
		}
	}
}