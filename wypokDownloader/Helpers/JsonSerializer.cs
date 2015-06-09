using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using wypokDownloader.Model;

namespace wypokDownloader.Helpers
{
    public class JsonSerializer
    {
        private readonly JavaScriptSerializer _serializer;

        public JsonSerializer()
        {
            _serializer = new JavaScriptSerializer();
            _serializer.RegisterConverters(new JavaScriptConverter[] { new JsonEntryConverter() });

        }

        public string SerializeEntry(EntryModel entryModel)
        {
            return Task<string>.Factory.StartNew(() => _serializer.Serialize(entryModel)).Result;

        }

        public List<EntryModel> DeserializeEntries(string text)
        {
            _serializer.MaxJsonLength=Int32.MaxValue;
            return Task<List<EntryModel>>.Factory.StartNew(() => _serializer.Deserialize<List<EntryModel>>(text)).Result;
        }
 
    }
}