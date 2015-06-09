using System.Collections.Generic;
using System.Text.RegularExpressions;
using wypokDownloader.Model;

namespace wypokDownloader.Helpers
{
    public class HashtagsExtractor
    {
        private HashSet<HashtagModel> _hashtags = new HashSet<HashtagModel>();

        public HashSet<HashtagModel> Hashtags
        {
            get { return _hashtags; }
            set { _hashtags = value; }
        }

        public List<HashtagModel> ExtractHashTags(string content)
        {
           var result = new List<HashtagModel>();
            var match = Regex.Match(content, "(#\\w\\w+)");
            if(match.Value!="")
            result.Add(new HashtagModel(match.Value));
            while (true)
            {
                match = match.NextMatch();
                if (match.Success)
                {
                    if (match.Value != "")
                    result.Add(new HashtagModel(match.Value));
                }
                else
                {
                    break;
                }
            }
            Hashtags.UnionWith(result);
            return result;
        }  
    }
}