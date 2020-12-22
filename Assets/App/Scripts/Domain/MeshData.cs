using System;
using System.Collections.Generic;

namespace DynamicBox.Domain
{
	[Serializable]
	public class MeshData
	{
		public class Root
		{
			public List<Datum> data;
		}

		public class Datum
		{
			public string id;
			public object @default;
			public TimeStamps timeStamps;
			public string manufacturerId;
			public string sku;
			public string series;
			public object serial;
			public string name;
			public Thumbnail thumbnail;
			public Size size;
			public string category;
			public List<string> tags;
			public List<string> path;
			public object description;
			public string item_type;
			public string url;
		}

		public class TimeStamps
		{
			public string created;
			public string lastUpdated;
		}

		public class Thumbnail
		{
			public string id;
			public string mediaUrl;
			public string thumbUrl;
			public string type;
			public TimeStamps timeStamps;
		}

		public class Size
		{
			public int x;
			public int y;
		}
	}
}