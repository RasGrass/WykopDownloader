using System.Collections.Generic;

namespace wypokDownloader.Model
{
    public class HashtagModel
    {
        private string _name;
        private string _directory;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Directory
        {
            get { return _directory; }
            set { _directory = value; }
        }

        public HashtagModel(string hashtag)
        {
            _name = hashtag;
            _directory = hashtag.Substring(1);
        }

        private sealed class NameEqualityComparer : IEqualityComparer<HashtagModel>
        {
            public bool Equals(HashtagModel x, HashtagModel y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x._name, y._name);
            }

            public int GetHashCode(HashtagModel obj)
            {
                return (obj._name != null ? obj._name.GetHashCode() : 0);
            }
        }

        protected bool Equals(HashtagModel other)
        {
            return string.Equals(_name, other._name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((HashtagModel) obj);
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }
    }
}