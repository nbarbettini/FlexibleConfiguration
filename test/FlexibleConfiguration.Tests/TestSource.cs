namespace FlexibleConfiguration.Tests
{
    public class TestSource
    {
        public string Foo { get; set; }

        public int Bar { get; set; }

        public TestEmbedded Child { get; set; }

        public string IgnoredSource { get; set; }
    }
}
