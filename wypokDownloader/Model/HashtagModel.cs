namespace wypokDownloader.Model
{
    public class HashtagModel
    {
        private readonly string _name;
        private string _directory;

        public string Name
        {
            get { return _name; }
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

        protected bool Equals(HashtagModel other)
        {
            return string.Equals(_name, other._name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((HashtagModel) obj);
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }
    }
}