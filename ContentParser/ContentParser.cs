using System.Text.RegularExpressions;

namespace ContentParser;

public class ContentParser
{
    public async Task ParseAsync()
    {
        string content0 = @"**ABP Framework** is completely open source and developed in a community-driven manner.

                    [Widget Type=""Poll"" PollName=""poll-name""]
                    Thanks _for_ *your * feedback.
                    [Widget Type=""Poll"" PollName=""poll-name1""]";

        string content1 = @"**ABP Framework** is completely open source and developed in a community-driven manner.
                    Thanks _for_ *your * feedback.
                    [Widget Type=""Poll"" PollName=""poll-name""]";

        string content2 = @"[Widget Type=""Poll"" PollName=""poll-name""] gg [Widget Type=""Poll"" PollName=""poll-name1""]**ABP Framework** is completely open source and developed in a community-driven manner.
                    Thanks _for_ *your * feedback.
                    Thanks _for_ *your * feedback.";

        string content3 = @"Thanks _for_ *your * feedback.
                    Thanks _for_ *your * feedback.";

        string content4 = @"[Widget Type=""Poll"" PollName=""poll-name""]";

        var contents = new List<string>() { content0, content1, content2, content3, content4 };

        foreach (var content in contents)
        {
            var polls = Regex.Matches(content, @"\[.*?\]").Select(p => p.Value).ToList();

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
                else
                {
                    parsedList.Add(replacedText);
                }
            }



            for (int b = 0, k = 0; b < parsedList.Count; b++)
            {
                if (parsedList[b] == delimeter)
                {
                    Console.WriteLine(polls[k++]);
                }
                else
                {
                    Console.WriteLine(parsedList[b]);
                }
            }

            Console.WriteLine("--------");
        }

        await Task.CompletedTask;
    }
}

