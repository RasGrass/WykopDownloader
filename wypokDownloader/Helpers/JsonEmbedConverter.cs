using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using wypokDownloader.Model;


namespace wypokDownloader.Helpers
{
    public class JsonEmbedConverter : JavaScriptConverter
	{
		public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
		{
			var embed = new Embed(
                (string) dictionary["type"],
                (string)dictionary["preview"],
                (string)dictionary["url"],
                (string)dictionary["source"], 
                (bool) dictionary["plus18"]
				);

			return embed;

		}

		public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override IEnumerable<Type> SupportedTypes
		{
			get { return new[] {typeof (Embed)}; }
		}
	}
}
