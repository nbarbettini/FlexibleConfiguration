namespace FlexibleConfiguration.Tests
{
    public class TestTarget
    {
        public string Foo { get; set; }

        public int Bar { get; set; }

        public TestEmbedded Child { get; set; }

        public string IgnoredDest { get; set; }
    }
}
