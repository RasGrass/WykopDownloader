using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using wypokDownloader.Model;


namespace wypokDownloader.Helpers
{
    public class JsonEntryConverter : JavaScriptConverter
    {
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            IDictionary<string, object> embedRootElement = dictionary["embed"] as IDictionary<string, object>;
            List<Embed> embedList = new List<Embed>();
            var converter = new JsonEmbedConverter();
            if (embedRootElement != null)
            {
                embedList.Add(
                    converter.Deserialize(embedRootElement, typeof (Embed), new JavaScriptSerializer()) as Embed);
            }


            var entry = new EntryModel(
                (int) dictionary["id"],
                (string) dictionary["author"],
                "",
                (int) dictionary["author_group"], 
                DateTime.Parse((string)dictionary["date"]),
                (string) dictionary["body"],
                "",
                "",
                "",
                1,
                new List<EntryComment>(), 
                (int) dictionary["vote_count"],
                (int) dictionary["user_vote"],
                new List<Dig>(){new Dig()}, 
                (bool) dictionary["user_favorite"], 
                embedList);
             
            return entry;

        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get { return new[] { typeof(EntryModel), typeof(Embed) }; }
        }
    }
}
