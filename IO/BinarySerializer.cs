/* File Description
 * Original Works/Author: Thomas Slusny
 * Other Contributors: None
 * Author Website: http://indiearmory.com
 * License: MIT
*/

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SFGL.IO
{
    /// <summary>
    /// Provides simple way of loading and saving binary files
    /// </summary>
	public static class BinarySerializer
    {
		/// <summary>
		/// Serializes object to binary format in a simple way
		/// </summary>
		public static  void Serialize<T>(string path, T obj) where T : ISerializable
		{
			Stream fstream = File.Open(path, FileMode.Create);
			Serialize(fstream, obj);
		}

		/// <summary>
		/// Serializes object to binary format in a simple way
		/// </summary>
		public static void Serialize<T>(Stream destStream, T obj) where T : ISerializable
		{
			BinaryFormatter bformatter = new BinaryFormatter();
			bformatter.Serialize(destStream, obj);
			destStream.Close();
		}

		/// <summary>
		/// UnSerializes object from binary format in a simple way
		/// </summary>
		public static T UnSerialize<T>(string path)
		{
			if (!File.Exists(path))
				throw new Exception("File you trying to unserialize doesn't exist");
			Stream fstream = File.Open(path, FileMode.Open);
			return UnSerialize<T>(fstream);
		}

		/// <summary>
		/// UnSerializes object from binary format in a simple way
		/// </summary>
		public static T UnSerialize<T>(Stream data)
		{
			BinaryFormatter bformat = new BinaryFormatter();
			T obj = (T)bformat.Deserialize(data);
			data.Close();
			return obj;
		}
    }
}