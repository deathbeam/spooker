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
		private static readonly Dictionary<Type, MethodInfo> ReadMethods;

		static PacketReader()
		{
			ReadMethods = new Dictionary<Type, MethodInfo>();
			var methods = typeof(PacketReader).GetMethods(BindingFlags.Instance | BindingFlags.Public);
			foreach (var mi in methods)
			{
				if (mi.Name.Equals("Write", StringComparison.InvariantCulture))
				{
					var pis = mi.GetParameters();
					if (pis.Length == 1)
						ReadMethods[pis[0].ParameterType] = mi;
				}
			}
		}

		public PacketReader() : this(0) { }

        public PacketReader(int capacity) : base(new MemoryStream(capacity)) { }

		public int Length
		{ 
			get { return (int)BaseStream.Length; }
		}

		public int Position
		{ 
			get { return (int)BaseStream.Position; }
			set { BaseStream.Position = value; } 
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
			return new Color(ReadSingle (), ReadSingle (), ReadSingle (), ReadSingle ());
		}

		public Point ReadPoint()
		{
			return new Point(ReadInt32 (), ReadInt32());
		}

		public Vector2 ReadVector2()
		{
			return new Vector2(ReadSingle (), ReadSingle ());
		}

		public Rectangle ReadRectangle()
		{
            return new Rectangle(ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32());
		}

		public Matrix ReadMatrix()
		{
            return new Matrix(
                ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle(),
                ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle(),
                ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle(),
                ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
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
			if (target == null) return;
			Type tp = target.GetType();

			FieldInfo[] fields = tp.GetFields(flags);
			NetUtility.SortMembersList(fields);

			foreach (FieldInfo fi in fields)
			{
				MethodInfo readMethod;
				if (ReadMethods.TryGetValue(fi.FieldType, out readMethod))
				{
				    object value = readMethod.Invoke(this, null);
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
			if (target == null) throw new ArgumentNullException("target");

			Type tp = target.GetType();

			PropertyInfo[] fields = tp.GetProperties(flags);
			NetUtility.SortMembersList(fields);
			foreach (PropertyInfo fi in fields)
			{
				MethodInfo readMethod;
				if (ReadMethods.TryGetValue(fi.PropertyType, out readMethod))
				{
					object value = readMethod.Invoke(this, null);
					MethodInfo setMethod = fi.GetSetMethod((flags & BindingFlags.NonPublic) == BindingFlags.NonPublic);
					setMethod.Invoke(target, new[] { value });
				}
			}
		}
	}
}