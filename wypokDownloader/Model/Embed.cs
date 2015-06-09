using System;

namespace wypokDownloader.Model
{
    public class Embed
    {
        private readonly string _type;
        private readonly string _preview;
        private readonly string _url;
        private readonly string _source;
        private readonly bool _plus18;

        public Embed()
        {
        }

        public Embed(string type, string preview, string url, string source, bool plus18)
        {
            _type = type;
            _preview = preview;
            _url = url;
            _source = source;
            _plus18 = plus18;
        }

        public string Type
        {
            get { return _type; }
        }

        public string Preview
        {
            get { return _preview; }
        }

        public string Url
        {
            get { return _url; }
        }

        public string Source
        {
            get { return _source; }
        }

        public bool Plus18
        {
            get { return _plus18; }
        }
    }
}
