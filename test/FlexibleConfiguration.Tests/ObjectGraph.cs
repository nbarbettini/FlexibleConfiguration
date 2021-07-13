using FluentAssertions;
using Xunit;

namespace FlexibleConfiguration.Tests
{
    public class ObjectGraph
    {
        [Fact]
        public void ShouldBePreserved()
        {
            var source = new TestSource
            {
                Foo = "Foobar",
                Bar = 32,
                Child = new TestEmbedded
                {
                    Blah = "o hai",
                    Qux = 64
                }
            };

            var compiled = new ConfigurationBuilder()
                .AddObject(source)
                .Build();

            var dest = new TestTarget();
            compiled.Bind(dest);

            dest.Foo.Should().Be("Foobar");
            dest.Bar.Should().Be(32);
            dest.Child.Blah.Should().Be("o hai");
            dest.Child.Qux.Should().Be(64);
        }

        [Fact]
        public void ShouldIgnoreUnknownFields()
        {
            var source = new TestSource
            {
                Foo = "Foobar",
                Bar = 32,
                IgnoredSource = "NOPE"
            };

            var compiled = new ConfigurationBuilder()
                .AddObject(source)
                .Build();

            var dest = new TestTarget();
            compiled.Bind(dest);

            dest.IgnoredDest.Should().BeNullOrEmpty();
        }

        [Fact]
        public void ShouldBePreservedYaml()
        {
            var source =
                        @"
                        Foo: Foobar
                        Bar: 32
                        Child:
                          Blah: o hai
                          Qux: 64
                        ";

            var compiled = new ConfigurationBuilder()
                .AddYaml(source)
                .Build();

            var dest = new TestTarget();
            compiled.Bind(dest);

            dest.Foo.Should().Be("Foobar");
            dest.Bar.Should().Be(32);
            dest.Child.Blah.Should().Be("o hai");
            dest.Child.Qux.Should().Be(64);
        }
    }
}

