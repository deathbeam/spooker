using System;
using System.Collections.Generic;

namespace Spooker
{
	public class PropertyDict : Dictionary<string, string>
	{
		public PropertyDict () : base()
		{
		}

		public PropertyDict (IDictionary<string, string> copy) : base(copy)
		{
		}
	}
}