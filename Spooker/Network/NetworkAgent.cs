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
	/// 
	/// </summary>
	public enum AgentRole
	{
		/// <summary></summary>
		Client,
        /// <summary></summary>
		Server
	}

	/// <summary>
	/// 
	/// </summary>
	public class NetworkAgent : IUpdateable
	{
		private readonly NetPeer _peer;
	    private readonly AgentRole _role;
		private NetOutgoingMessage _outgoingMessage;
	    private readonly int _port;

		public delegate void ConnectEvent (NetIncomingMessage data);
		public event ConnectEvent OnConnect;
		public event ConnectEvent OnDisconnect;

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Manages (adds or removes) packets to packet manager stack.
		/// </summary>
		////////////////////////////////////////////////////////////
		public PacketManager Packets;

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Return list of all connections connected to this peer.
		/// </summary>
		////////////////////////////////////////////////////////////
		public List<NetConnection> Connections
		{
			get
			{
				return _peer.Connections;
			}
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Customize appIdentifier. Note: Client and server
		/// appIdentifier must be the same.
		/// </summary>
		////////////////////////////////////////////////////////////
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

			Packets = new PacketManager ();

			_peer.Start();
			_outgoingMessage = _peer.CreateMessage();
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Connects to a server. Throws an exception if you attempt
		/// to call Connect as a Server.
		/// </summary>
		////////////////////////////////////////////////////////////
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

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Closes the NetPeer
		/// </summary>
		////////////////////////////////////////////////////////////
		public void Shutdown()
		{
			_peer.Shutdown("Closing connection.");
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Sends off _outgoingMessage and then clears it for the
		/// next send. Defaults to UnreliableSequenced for fast
		/// transfer which guarantees that older messages won't be
		/// processed after new messages. If IsGuaranteed is true it
		/// uses ReliableSequenced which is safer but much slower.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void SendMessage(NetConnection recipient)
		{
			SendMessage(recipient, false);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Sends off _outgoingMessage and then clears it for the
		/// next send. Defaults to UnreliableSequenced for fast
		/// transfer which guarantees that older messages won't be
		/// processed after new messages. If IsGuaranteed is true it
		/// uses ReliableSequenced which is safer but much slower.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void SendMessage(NetConnection recipient, bool isGuaranteed)
		{
			NetDeliveryMethod method = isGuaranteed ? NetDeliveryMethod.ReliableOrdered : NetDeliveryMethod.UnreliableSequenced;
			_peer.SendMessage(_outgoingMessage, recipient, method);
			_outgoingMessage = _peer.CreateMessage();
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Reads every message in the queue and processes a list of
		/// data messages. Other message types just write a Console
		/// note. This should be called every update by the Game
		/// Screen.
		/// </summary>
		////////////////////////////////////////////////////////////
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
						OnDisconnect.Invoke(incomingMessage);
					}

					if (status == NetConnectionStatus.Connected)
					{
						OnConnect.Invoke(incomingMessage);
					}
					break;
				case NetIncomingMessageType.Data:
					var packet = Packets.GetPacket (incomingMessage.ReadInt32 ());
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

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Write string to message
		/// </summary>
		////////////////////////////////////////////////////////////
		public void WriteMessage(string message)
		{
			_outgoingMessage.Write(message);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Write bool to message
		/// </summary>
		////////////////////////////////////////////////////////////
		public void WriteMessage(bool message)
		{
			_outgoingMessage.Write(message);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Write byte to message
		/// </summary>
		////////////////////////////////////////////////////////////
		public void WriteMessage(byte message)
		{
			_outgoingMessage.Write(message);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Write short to message
		/// </summary>
		////////////////////////////////////////////////////////////
		public void WriteMessage(short message)
		{
			_outgoingMessage.Write(message);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Write int to message
		/// </summary>
		////////////////////////////////////////////////////////////
		public void WriteMessage(int message)
		{
			_outgoingMessage.Write(message);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Write long to message
		/// </summary>
		////////////////////////////////////////////////////////////
		public void WriteMessage(long message)
		{
			_outgoingMessage.Write(message);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Write float to message
		/// </summary>
		////////////////////////////////////////////////////////////
		public void WriteMessage(float message)
		{
			_outgoingMessage.Write(message);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Write double to message
		/// </summary>
		////////////////////////////////////////////////////////////
		public void WriteMessage(double message)
		{
			_outgoingMessage.Write(message);
		}
	}
}