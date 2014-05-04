//-----------------------------------------------------------------------------
// PacketWriter.cs
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
	public class PacketWriter : BinaryWriter
	{
		private static Dictionary<Type, MethodInfo> _writeMethods;

		static PacketWriter()
		{
			_writeMethods = new Dictionary<Type, MethodInfo>();
			var methods = typeof(PacketWriter).GetMethods(BindingFlags.Instance | BindingFlags.Public);
			foreach (var mi in methods)
			{
				if (mi.Name.Equals("Write", StringComparison.InvariantCulture))
				{
					var pis = mi.GetParameters();
					if (pis.Length == 1)
						_writeMethods[pis[0].ParameterType] = mi;
				}
			}
		}

		public PacketWriter () : this (0) { }

		public PacketWriter (int capacity) : base ( new MemoryStream(capacity)) { }

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
		}

		internal void Reset() 
		{
			var stream = (MemoryStream)this.BaseStream;
			stream.SetLength(0);
			stream.Position = 0;

		}

		public void Write(GameSpan value)
		{
			Write (value.Ticks);
		}

		public void Write(Color value)
		{
			Write (value.A);
			Write (value.R);
			Write (value.G);
			Write (value.B);
		}

		public void Write(Point value)
		{
			Write (value.X);
			Write (value.Y);
		}

		public void Write(Vector2 value)
		{
			Write (value.X);
			Write (value.Y);
		}

		public void Write(Rectangle value)
		{
			Write (value.X);
			Write (value.Y);
			Write (value.Width);
			Write (value.Height);
		}

		public void Write (Matrix Value)
		{
			Write (Value.M11);
			Write (Value.M12);
			Write (Value.M13);
			Write (Value.M14);
			Write (Value.M21);
			Write (Value.M22);
			Write (Value.M23);
			Write (Value.M24);
			Write (Value.M31);
			Write (Value.M32);
			Write (Value.M33);
			Write (Value.M34);
			Write (Value.M41);
			Write (Value.M42);
			Write (Value.M43);
			Write (Value.M44);
		}

		/// <summary>
		/// Writes all public and private declared instance fields of the object in alphabetical order using reflection
		/// </summary>
		public void WriteAllFields(object ob)
		{
			WriteAllFields(ob, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
		}

		/// <summary>
		/// Writes all fields with specified binding in alphabetical order using reflection
		/// </summary>
		public void WriteAllFields(object ob, BindingFlags flags)
		{
			if (ob == null)
				return;
			Type tp = ob.GetType();

			FieldInfo[] fields = tp.GetFields(flags);
			NetUtility.SortMembersList(fields);

			foreach (FieldInfo fi in fields)
			{
				object value = fi.GetValue(ob);

				// find the appropriate Write method
				MethodInfo writeMethod;
				if (_writeMethods.TryGetValue(fi.FieldType, out writeMethod))
					writeMethod.Invoke(this, new object[] { value });
				else
					throw new NotImplementedException("Failed to find write method for type " + fi.FieldType);
			}
		}

		/// <summary>
		/// Writes all public and private declared instance properties of the object in alphabetical order using reflection
		/// </summary>
		public void WriteAllProperties(object ob)
		{
			WriteAllProperties(ob, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
		}

		/// <summary>
		/// Writes all properties with specified binding in alphabetical order using reflection
		/// </summary>
		public void WriteAllProperties(object ob, BindingFlags flags)
		{
			if (ob == null)
				return;
			Type tp = ob.GetType();

			PropertyInfo[] fields = tp.GetProperties(flags);
			NetUtility.SortMembersList(fields);

			foreach (PropertyInfo fi in fields)
			{
				MethodInfo getMethod = fi.GetGetMethod((flags & BindingFlags.NonPublic) == BindingFlags.NonPublic);
				object value = getMethod.Invoke(ob, null);

				// find the appropriate Write method
				MethodInfo writeMethod;
				if (_writeMethods.TryGetValue(fi.PropertyType, out writeMethod))
					writeMethod.Invoke(this, new object[] { value });
			}
		}
	}
}