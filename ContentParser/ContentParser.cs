using System.Text.RegularExpressions;

namespace ContentParser;

public class ContentParser
{
    private readonly List<string> _options;
    public ContentParser()
    {
        _options = new()
        {
            "Poll"
        };
    }

    public Task<List<string>> ParseAsync(string content)
    {
        string delimeter = "----";
        var replacedText = Regex.Replace(content, @"\[.*?\]", delimeter);

        var parsedList = new List<string>();
        if (!replacedText.Contains(delimeter))
        {
            parsedList.Add(replacedText);
        }

        while (replacedText.Contains(delimeter))
        {
            //For parsing delimeter
            var index = replacedText.IndexOf(delimeter);
            if (index != 0)
            {
                parsedList.Add(replacedText.Substring(0, index));
                replacedText = replacedText.Substring(index, replacedText.Length - index);
                index = 0;
            }

            parsedList.Add(replacedText.Substring(index, delimeter.Length));
            replacedText = replacedText.Substring(delimeter.Length, replacedText.Length - delimeter.Length);

            //for parsing the other side
            index = replacedText.IndexOf(delimeter);
            if (index != -1)
            {
                parsedList.Add(replacedText.Substring(0, index));
                replacedText = replacedText.Substring(index, replacedText.Length - index);

            }
            else if (replacedText.Length > 0)
            {
                parsedList.Add(replacedText);
            }
        }

        var pollNames = Regex.Matches(content, @"(?<=PollName\s*=\s*"")(.*?)(?="")").Select(p => p.Value).ToList();
        var polls = Regex.Matches(content, @"(?<=Widget Type\s*=\s*"")(.*?)(?="")").Select(p => p.Value).ToList();

        List<string> contentFragments = new();
        for (int i = 0, k = 0; i < parsedList.Count; i++)
        {
            if (parsedList[i] == delimeter)
            {
                if (polls.Count > k)
                {
                    var name = _options.FirstOrDefault(p => p == polls[k]);
                    if (name is not null && pollNames.Count > k)
                    {
                        contentFragments.Add($"{name}-{pollNames[k]}");
                    }
                }
                k++;
            }
            else
            {
                contentFragments.Add(parsedList[i]);
            }
        }

        return Task.FromResult(contentFragments);
    }
}

