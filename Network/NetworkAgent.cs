using System;
using System.IO;
using System.Collections.Generic;
using Lidgren.Network;
using SFGL.Time;

namespace SFGL.Network
{
	public enum AgentRole
	{
		Client,
		Server
	}

	public class NetworkAgent : IUpdateable
	{
		public delegate void PlayerConnectEvent ();
		private PlayerConnectEvent OnPlayerConnect;

		private NetPeer _peer;
		private NetPeerConfiguration _config;
		private AgentRole _role;
		private NetOutgoingMessage _outgoingMessage;
		private List<NetIncomingMessage> _incomingMessages = new List<NetIncomingMessage>();
		private PacketManager packets;
		private int port;

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
		public NetworkAgent(AgentRole role, string tag, PacketManager packets, PlayerConnectEvent connectCallback, int port = 14242)
		{
			_role = role;
			_config = new NetPeerConfiguration(tag);
			this.port = port;

			if (_role == AgentRole.Server)
			{
				_config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
				_config.Port = port;
				//Casts the NetPeer to a NetServer
				_peer = new NetServer(_config);
			}
			if (_role == AgentRole.Client)
			{
				_config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
				//Casts the NetPeer to a NetClient
				_peer = new NetClient(_config);
			}

			this.packets = packets;
			OnPlayerConnect = connectCallback;

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
				_peer.Connect(ip, port);
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
		public void SendMessage(NetConnection recipient, bool IsGuaranteed)
		{
			NetDeliveryMethod method = IsGuaranteed ? NetDeliveryMethod.ReliableOrdered : NetDeliveryMethod.UnreliableSequenced;
			_peer.SendMessage(_outgoingMessage, recipient, method);
			_outgoingMessage = _peer.CreateMessage();
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Reads every message in the queue and returns a list of
		/// data messages. Other message types just write a Console
		/// note. This should be called every update by the Game
		/// Screen. The Game Screen should implement the actual
		/// handling of messages.
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
					NetConnectionStatus status = (NetConnectionStatus)incomingMessage.ReadByte();
					if(_role == AgentRole.Server)
						output += "Status Message: " + incomingMessage.ReadString() + "\n";

					if (status == NetConnectionStatus.Connected)
					{
						OnPlayerConnect();
					}
					break;
				case NetIncomingMessageType.Data:
					var packet = packets.GetPacket (incomingMessage.ReadInt32 ());
					packet.HandleData (incomingMessage);
					break;
				default:
					// unknown message type
					break;
				}
			}
			if (_role == AgentRole.Server)
			{
				StreamWriter textOut = new StreamWriter(new FileStream("netlog.txt", FileMode.Append, FileAccess.Write));
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