using AutoFixture;

namespace StatParser.UnitTests.StatManagerTests
{
    public abstract class StatManagerFixture
    {
        protected StatManager SUT;

        protected Fixture Fixture = new Fixture();

        protected StatManagerFixture()
        {
            SUT = new StatManager();
        }
    }
}
