using System;
using System.Collections.Generic;

namespace wypokDownloader.Model
{
    public class EntryComment
    {
        private readonly int _id;
        private readonly string _author;
        private readonly Uri _authorAvatar;
        private readonly int _authorGroup;
        private readonly DateTime _date;
        private readonly string _content;
        private readonly int _voteCount;
        private readonly List<Dig> _voters;
        private readonly Embed _embed;
        private readonly int _userVote;

        public EntryComment()
        {
        }

        public EntryComment(int id, string author, Uri authorAvatar, int authorGroup, DateTime date, string content, int userVote, int voteCount, List<Dig> voters, Embed embed)
        {
            _id = id;
            _author = author;
            _authorAvatar = authorAvatar;
            _authorGroup = authorGroup;
            _date = date;
            _content = content;
            _voteCount = voteCount;
            _voters = voters;
            _embed = embed;
            _userVote = userVote;
        }

        public int Id
        {
            get { return _id; }
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

        public DateTime Date
        {
            get { return _date; }
        }

        public string Content
        {
            get { return _content; }
        }

        public int VoteCount
        {
            get { return _voteCount; }
        }

        public List<Dig> Voters
        {
            get { return _voters; }
        }

        public Embed Embed
        {
            get { return _embed; }
        }

        public int UserVote
        {
            get { return _userVote; }
        }
    }
}
