using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using wypokDownloader.Model;

namespace wypokDownloader.Helpers
{
    [ValueConversion(typeof(List<string>), typeof(string))]
    public class ListToStringConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(string))
                throw new InvalidOperationException("The target must be a String");

            List<HashtagModel> hashtags = value as List<HashtagModel>;
            List<string> hashtagNames =new List<string>();

            foreach (HashtagModel hashtag in hashtags)
            {
                hashtagNames.Add(hashtag.Name);
            }
            return String.Join(", ", (hashtagNames).ToArray());
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}