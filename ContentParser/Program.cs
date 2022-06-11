namespace ContentParser;

using System;


public class Program
{
    protected Program()
    {

    }

    static Task Main(string[] args)
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

        try
        {
            var contents = new List<string>() { content0, content1, content2, content3, content4 };

            foreach (var content in contents)
            {
                ContentParser parser = new();
                var contentFragments = parser.ParseAsync(content).Result;
                Console.WriteLine(string.Join(" ", contentFragments));
                Console.WriteLine();
            }
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}







