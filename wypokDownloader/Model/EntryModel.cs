using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Media.Imaging;
using wypokDownloader.Helpers;

namespace wypokDownloader.Model
{
    public class EntryModel
    {
        private readonly int _id;
        private readonly string _author;
        private readonly string _authorAvatar;
        private readonly int _authorGroup;
        private readonly DateTime _date;
        private readonly string _content;
        private readonly string _url;
        private readonly string _receiver;
        private readonly string _receiverAvatar;
        private readonly int _receiverGroup;
        private readonly List<EntryComment> _comments;
        private readonly int _voteCount;
        private readonly int _userVote;
        private readonly List<Dig> _voters;
        private readonly bool _userFavorite;
        private readonly List<Embed> _embed;
        private readonly List<HashtagModel> _hashtags;
        private bool _synchronized;
        public static HashtagsExtractor hashtagsExtractor = new HashtagsExtractor();

        public EntryModel()
        {
        }

        public EntryModel(int id, string author, string authorAvatar, int authorGroup, DateTime date, string content, string url, string receiver, string receiverAvatar, int receiverGroup, List<EntryComment> comments, int voteCount, int userVote, List<Dig> voters, bool userFavorite, List<Embed> embed)
        {
            _id = id;
            _author = author;
            _authorAvatar = authorAvatar;
            _authorGroup = authorGroup;
            _date = date;
            _content = content;
            _url = url;
            _receiver = receiver;
            _receiverAvatar = receiverAvatar;
            _receiverGroup = receiverGroup;
            _comments = comments;
            _voteCount = voteCount;
            _userVote = userVote;
            _voters = voters;
            _userFavorite = userFavorite;
            _embed = embed;
            _hashtags = hashtagsExtractor.ExtractHashTags(content);
        }



        public bool Synchronized
        {
            get { return _synchronized;}
            set { _synchronized = value; }
        }

        public int Id
        {
            get { return _id; }
        }

        public string Author
        {
            get { return _author; }
        }

        public string AuthorAvatar
        {
            get { return _authorAvatar; }
        }

        public int AuthorGroup
        {
            get { return _authorGroup; }
        }

        public DateTime Date
        {
            get { return _date; }
        }

        public string Url
        {
            get { return _url; }
        }

        public string Content
        {
            get { return _content; }
        }

        public string Receiver
        {
            get { return _receiver; }
        }

        public string ReceiverAvatar
        {
            get { return _receiverAvatar; }
        }

        public int ReceiverGroup
        {
            get { return _receiverGroup; }
        }

        public List<EntryComment> Comments
        {
            get { return _comments; }
        }

        public int UserVote
        {
            get { return _userVote; }
        }

        public int VoteCount
        {
            get { return _voteCount; }
        }

        public List<Dig> Voters
        {
            get { return _voters; }
        }

        public bool UserFavorite
        {
            get { return _userFavorite; }
        }

        public List<Embed> Embed
        {
            get { return _embed; }
        }

        public List<HashtagModel> HashTags
        {
            get
            {
                return _hashtags;
            }
        }


        public static Image GetImageFromUrl(string url)
        {
            if (url == null ||url.Contains("youtube") || url.Contains("gfycat"))
            {
                return null;
            }
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            BitmapImage b = new BitmapImage();
            
            using (HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                using (Stream stream = httpWebReponse.GetResponseStream())
                {
                    return Image.FromStream(stream);
                }
            }
        }
        public override string ToString()
        {
            return Embed.ToArray()[0].Type + " " + Embed.ToArray()[0].Source;
        }
    }
}
