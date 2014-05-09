//-----------------------------------------------------------------------------
// NetworkAgent.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

using System;
using System.IO;
using System.Collections.Generic;
using Lidgren.Network;
using Spooker.Time;

namespace Spooker.Network
{
	/// <summary>
	/// Agent role.
	/// </summary>
	public enum AgentRole : byte
	{
		Client,
		Server
	}

	/// <summary>
	/// Network agent.
	/// </summary>
	public class NetworkAgent : IUpdateable
	{
		private readonly NetPeer _peer;
	    private readonly AgentRole _role;
		private NetOutgoingMessage _outgoingMessage;
	    private readonly int _port;

		/// <summary>
		/// Occurs when client connects or disconnects.
		/// </summary>
		public delegate void ConnectEvent (NetIncomingMessage data);

		/// <summary>
		/// Occurs when client connects.
		/// </summary>
		public event ConnectEvent OnConnect;

		/// <summary>
		/// Occurs when client disconnects.
		/// </summary>
		public event ConnectEvent OnDisconnect;

		/// <summary>
		/// The packet manager.
		/// </summary>
		public PacketManager PacketManager;

		/// <summary>
		/// Gets the connections.
		/// </summary>
		/// <value>The connections.</value>
		public List<NetConnection> Connections
		{
			get
			{
				return _peer.Connections;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Network.NetworkAgent"/> class.
		/// Note: Client and server tag must be the same.
		/// </summary>
		/// <param name="role">Role.</param>
		/// <param name="tag">Tag.</param>
		/// <param name="port">Port.</param>
		public NetworkAgent(AgentRole role, string tag, int port = 14242)
		{
		    _role = role;
			var config = new NetPeerConfiguration(tag);
			_port = port;

		    if (_role == AgentRole.Server)
			{
				config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
				config.Port = port;
				//Casts the NetPeer to a NetServer
				_peer = new NetServer(config);
			}
			if (_role == AgentRole.Client)
			{
				config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
				//Casts the NetPeer to a NetClient
				_peer = new NetClient(config);
			}

			PacketManager = new PacketManager ();

			_peer.Start();
			_outgoingMessage = _peer.CreateMessage();
		}

		/// <summary>
		/// Connects to a server. Throws an exception if you attempt
		/// to call Connect as a Server.
		/// </summary>
		/// <param name="ip">Ip.</param>
		public void Connect(string ip)
		{
			if (_role == AgentRole.Client)
			{
				_peer.Connect(ip, _port);
			}
			else
			{
				throw new SystemException("Attempted to connect as server. Only clients should connect.");
			}
		} 

		/// <summary>
		/// Shutdown this instance.
		/// </summary>
		public void Shutdown()
		{
			_peer.Shutdown("Closing connection.");
		}

		/// <summary>
		/// Sends off _outgoingMessage and then clears it for the
		/// next send. Defaults to UnreliableSequenced for fast
		/// transfer which guarantees that older messages won't be
		/// processed after new messages.
		/// </summary>
		/// <param name="recipient">Recipient.</param>
		public void SendMessage(NetConnection recipient)
		{
			SendMessage(recipient, false);
		}

		/// <summary>
		/// Sends off _outgoingMessage and then clears it for the
		/// next send. Defaults to UnreliableSequenced for fast
		/// transfer which guarantees that older messages won't be
		/// processed after new messages. If IsGuaranteed is true it
		/// uses ReliableSequenced which is safer but much slower.
		/// </summary>
		/// <param name="recipient">Recipient.</param>
		/// <param name="isGuaranteed">If set to <c>true</c> is guaranteed.</param>
		public void SendMessage(NetConnection recipient, bool isGuaranteed)
		{
			NetDeliveryMethod method = isGuaranteed ? NetDeliveryMethod.ReliableOrdered : NetDeliveryMethod.UnreliableSequenced;
			_peer.SendMessage(_outgoingMessage, recipient, method);
			_outgoingMessage = _peer.CreateMessage();
		}

		/// <summary>
		/// Component uses this for updating itself.
		/// </summary>
		/// <param name="gameTime">Provides snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			NetIncomingMessage incomingMessage;
			string output = "";

			while ((incomingMessage = _peer.ReadMessage()) != null)
			{
				switch (incomingMessage.MessageType)
				{
				case NetIncomingMessageType.DiscoveryRequest:
					_peer.SendDiscoveryResponse(null, incomingMessage.SenderEndPoint);
					break;
				case NetIncomingMessageType.VerboseDebugMessage:
				case NetIncomingMessageType.DebugMessage:
				case NetIncomingMessageType.WarningMessage:
				case NetIncomingMessageType.ErrorMessage:
					if(_role == AgentRole.Server)
						output += incomingMessage.ReadString() + "\n";
					break;
				case NetIncomingMessageType.StatusChanged:
					var status = (NetConnectionStatus)incomingMessage.ReadByte();
					if(_role == AgentRole.Server)
						output += "Status Message: " + incomingMessage.ReadString() + "\n";

					if (status == NetConnectionStatus.Disconnected)
					{
						if (OnDisconnect != null)
							OnDisconnect.Invoke (incomingMessage);
					}

					if (status == NetConnectionStatus.Connected)
					{
						if (OnConnect != null)
							OnConnect.Invoke (incomingMessage);
					}
					break;
				case NetIncomingMessageType.Data:
					var packet = PacketManager.GetPacket (incomingMessage.ReadInt32 ());
					packet.HandleData (incomingMessage);
					break;
				}
			}
			if (_role == AgentRole.Server)
			{
				var textOut = new StreamWriter(new FileStream("netlog.txt", FileMode.Append, FileAccess.Write));
				textOut.Write(output);
				textOut.Close();
			}
		}

		/// <summary>
		/// Write the specified writer.
		/// </summary>
		/// <param name="writer">Writer.</param>
		public void Write(PacketWriter writer)
		{
			_outgoingMessage.Write(writer.Data);
		}
	}
}