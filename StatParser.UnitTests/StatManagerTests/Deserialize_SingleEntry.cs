using FluentAssertions;
using Xunit;

namespace StatParser.UnitTests.StatManagerTests
{
    public class Deserialize_SingleEntry : StatManagerFixture
    {
        private readonly string _data;

        public Deserialize_SingleEntry()
        {
            // arrange
            _data = @"
                new entry ""_Troll""
                type ""Character""
                using """"
                data ""Act part"" """"
                data ""Flags"" ""KnockdownImmunity;PetrifiedImmunity;""
                data ""Talents"" ""AttackOfOpportunity""
            ";
        }

        [Fact]
        public void Deserialize_SingleEntry_MapsSingleEntry()
        {
            // arrange
            
            // act
            var actual = SUT.Deserialize(_data);

            // assert
            actual.Count.Should().Be(1);
        }

        [Fact]
        public void Deserialize_SingleEntry_MapsName()
        {
            // arrange
            
            // act
            var actual = SUT.Deserialize(_data);

            // assert
            actual[0].Name.Should().Be("_Troll");
        }

        [Fact]
        public void Deserialize_SingleEntry_MapsType()
        {
            // arrange
            
            // act
            var actual = SUT.Deserialize(_data);

            // assert
            actual[0].Type.Should().Be("Character");
        }

        [Fact]
        public void Deserialize_SingleEntry_MapsUsing()
        {
            // arrange
            
            // act
            var actual = SUT.Deserialize(_data);

            // assert
            actual[0].Using.Should().BeEmpty();
        }

        [Fact]
        public void Deserialize_SingleEntry_MapsFlags()
        {
            // arrange
            
            // act
            var actual = SUT.Deserialize(_data);

            // assert
            actual[0].Data["Flags"].Should().Be("KnockdownImmunity;PetrifiedImmunity;");
        }

        [Fact]
        public void Deserialize_SingleEntry_MapsTalents()
        {
            // arrange
            
            // act
            var actual = SUT.Deserialize(_data);

            // assert
            actual[0].Data["Talents"].Should().Be("AttackOfOpportunity");
        }
    }
}