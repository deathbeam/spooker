//-----------------------------------------------------------------------------
// PacketReader.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: Lidgren Network
// License: MIT
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Spooker.Graphics;
using Spooker.Time;

namespace Spooker.Network
{
	public class PacketReader : BinaryReader
	{
		private static Dictionary<Type, MethodInfo> _readMethods;

		static PacketReader()
		{
			_readMethods = new Dictionary<Type, MethodInfo>();
			var methods = typeof(PacketReader).GetMethods(BindingFlags.Instance | BindingFlags.Public);
			foreach (var mi in methods)
			{
				if (mi.Name.Equals("Write", StringComparison.InvariantCulture))
				{
					var pis = mi.GetParameters();
					if (pis.Length == 1)
						_readMethods[pis[0].ParameterType] = mi;
				}
			}
		}

		public PacketReader() : this(0) { }

		public PacketReader(int capacity) : base(new MemoryStream(0)) { }

		public int Length
		{ 
			get { return (int)BaseStream.Length; }
		}

		public int Position
		{ 
			get { return (int)BaseStream.Position; }
			set { if (BaseStream.Position != value) BaseStream.Position = value; } 
		}

		internal byte[] Data
		{
			get { var stream = (MemoryStream)BaseStream; return stream.GetBuffer(); }			
			set { var ms = (MemoryStream)BaseStream; ms.Write(value, 0, value.Length); }
		}

		internal void Reset(int size)
		{
			var ms = (MemoryStream)BaseStream;
			ms.SetLength(size);
			ms.Position = 0;
		}

		public GameSpan ReadGameSpan()
		{
			return GameSpan.FromTicks (ReadInt64 ());
		}

		public Color ReadColor()
		{
			var value = new Color();
			value.A = ReadSingle ();
			value.R = ReadSingle();
			value.G = ReadSingle();
			value.B = ReadSingle();
			return value;
		}

		public Point ReadPoint()
		{
			var value = new Point();
			value.X = ReadInt32 ();
			value.Y = ReadInt32();
			return value;
		}

		public Vector2 ReadVector2()
		{
			var value = new Vector2();
			value.X = ReadSingle ();
			value.Y = ReadSingle();
			return value;
		}

		public Rectangle ReadRectangle()
		{
			var value = new Rectangle();
			value.X = ReadInt32 ();
			value.Y = ReadInt32 ();
			value.Width = ReadInt32 ();
			value.Height = ReadInt32 ();
			return value;
		}

		public Matrix ReadMatrix()
		{
			var value = new Matrix();
			value.M11 = ReadSingle();
			value.M12 = ReadSingle();
			value.M13 = ReadSingle();
			value.M14 = ReadSingle();
			value.M21 = ReadSingle();
			value.M22 = ReadSingle();
			value.M23 = ReadSingle();
			value.M24 = ReadSingle();
			value.M31 = ReadSingle();
			value.M32 = ReadSingle();
			value.M33 = ReadSingle();
			value.M34 = ReadSingle();
			value.M41 = ReadSingle();
			value.M42 = ReadSingle();
			value.M43 = ReadSingle();
			value.M44 = ReadSingle();
			return value;
		}

		/// <summary>
		/// Reads all public and private declared instance fields of the object in alphabetical order using reflection
		/// </summary>
		public void ReadAllFields(object target)
		{
			ReadAllFields(target, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
		}

		/// <summary>
		/// Reads all fields with the specified binding of the object in alphabetical order using reflection
		/// </summary>
		public void ReadAllFields(object target, BindingFlags flags)
		{
			if (target == null)
				return;
			Type tp = target.GetType();

			FieldInfo[] fields = tp.GetFields(flags);
			NetUtility.SortMembersList(fields);

			foreach (FieldInfo fi in fields)
			{
				object value;

				// find read method
				MethodInfo readMethod;
				if (_readMethods.TryGetValue(fi.FieldType, out readMethod))
				{
					// read value
					value = readMethod.Invoke(this, null);

					// set the value
					fi.SetValue(target, value);
				}
			}
		}

		/// <summary>
		/// Reads all public and private declared instance fields of the object in alphabetical order using reflection
		/// </summary>
		public void ReadAllProperties(object target)
		{
			ReadAllProperties(target, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
		}

		/// <summary>
		/// Reads all fields with the specified binding of the object in alphabetical order using reflection
		/// </summary>
		public void ReadAllProperties(object target, BindingFlags flags)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			if (target == null)
				return;
			Type tp = target.GetType();

			PropertyInfo[] fields = tp.GetProperties(flags);
			NetUtility.SortMembersList(fields);
			foreach (PropertyInfo fi in fields)
			{
				object value;

				// find read method
				MethodInfo readMethod;
				if (_readMethods.TryGetValue(fi.PropertyType, out readMethod))
				{
					// read value
					value = readMethod.Invoke(this, null);

					// set the value
					MethodInfo setMethod = fi.GetSetMethod((flags & BindingFlags.NonPublic) == BindingFlags.NonPublic);
					setMethod.Invoke(target, new object[] { value });
				}
			}
		}
	}
}