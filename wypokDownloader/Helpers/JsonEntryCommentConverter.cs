using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using wypokDownloader.Model;


namespace wypokDownloader.Helpers
{
    public class JsonEntryCommentConverter : JavaScriptConverter
	{
		public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
		{
			var entryComment = new EntryComment(
				(int)dictionary["id"],
			    (string)dictionary["author"],
			    dictionary["author_avatar"] as Uri,
			    (int)dictionary["author_group"],
				(DateTime)dictionary["date"],
				(string)dictionary["body"],
				(int)dictionary["vote_count"],
				(int)dictionary["user_vote"],
				dictionary["voters"] as List<Dig>,
				dictionary["embed"] as Embed);

			return entryComment;

		}

		public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override IEnumerable<Type> SupportedTypes
		{
			get { return new[] {typeof (EntryModel)}; }
		}
	}
}
