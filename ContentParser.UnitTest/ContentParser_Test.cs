namespace ContentParser.UnitTest
{
    public class ContentParser_Test
    {
        public ContentParser_Test()
        {

        }

        [Theory]
        [MemberData(nameof(ExampleData))]
        public async Task ParseAsync_ShouldWorkProperlyWithCorrectInputs(string content, int expectedLine)
        {
            ContentParser contentParser = new();
            var contentFragments = await contentParser.ParseAsync(content);

            Assert.NotNull(contentFragments);
            Assert.Equal(expectedLine, contentFragments.Count);
        }

        [Fact]
        public async Task ParseAsync_ShouldWorkWithWrongConfigOptions()
        {
            var content = @"**ABP Framework** is completely open source and developed in a community-driven manner.
                        [Widget Type=  ""Poll"" PollName =""poll-name""]
                        Thanks _for_ *your * feedback.";

            ContentParser contentParser = new();
            var contentFragments = await contentParser.ParseAsync(content);

            Assert.NotNull(contentFragments);
            Assert.Equal(3, contentFragments.Count);
        }

        [Fact]
        public async Task ParseAsync_ShouldWorkWithWrongWidgetType()
        {
            var content = @"**ABP Framework** is completely open source and developed in a community-driven manner.
                        [Widget Wrong Type=  ""Poll"" PollName =""poll-name""]
                        Thanks _for_ *your * feedback.";

            ContentParser contentParser = new();
            var contentFragments = await contentParser.ParseAsync(content);

            Assert.NotNull(contentFragments);
            Assert.Equal(3, contentFragments.Count);
        }

        public static IEnumerable<object[]> ExampleData =>
             new List<object[]>
             {
              new object[] { @"**ABP Framework** is completely open source and developed in a community-driven manner.
                        [Widget Type=""Poll"" PollName=""poll-name""]
                        Thanks _for_ *your * feedback.", 3},

              new object[] { @"**ABP Framework** is completely open source and developed in a community-driven manner.
                    [Widget Type=""Poll"" PollName=""poll-name""]
                    Thanks _for_ *your * feedback.
                    [Widget Type=""Poll"" PollName=""poll-name1""]", 4 },

              new object[] { @"**ABP Framework** is completely open source and developed in a community-driven manner.
                    Thanks _for_ *your * feedback.
                    [Widget Type=""Poll"" PollName=""poll-name""]", 2 },

              new object[] {   @"[Widget Type=""Poll"" PollName=""poll-name""] gg [Widget Type=""Poll"" PollName=""poll-name1""]**ABP Framework** is completely open source and developed in a community-driven manner.
                    Thanks _for_ *your * feedback.
                    Thanks _for_ *your * feedback.", 4},

              new object[] { @"Thanks _for_ *your * feedback.
                    Thanks _for_ *your * feedback.", 1}
             };
    }
}