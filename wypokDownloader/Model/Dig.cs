using System;

namespace wypokDownloader.Model
{
    public class Dig
    {
        private readonly string _author;
        private readonly Uri _authorAvatar;
        private readonly int _authorGroup;

        public Dig()
        {
        }

        public Dig(string author, Uri authorAvatar, int authorGroup)
        {
            _author = author;
            _authorAvatar = authorAvatar;
            _authorGroup = authorGroup;
        }

        public string Author
        {
            get { return _author; }
        }

        public Uri AuthorAvatar
        {
            get { return _authorAvatar; }
        }

        public int AuthorGroup
        {
            get { return _authorGroup; }
        }

    }
}
