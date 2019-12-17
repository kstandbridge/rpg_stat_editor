using FluentAssertions;
using Xunit;

namespace StatParser.UnitTests.StatManagerTests
{
    public class Deserialize_MultipleEntries : StatManagerFixture
    {
        private readonly string _data;

        public Deserialize_MultipleEntries()
        {
            // arrange
            _data = @"
                new entry ""Trolls_Grunt_Strong""
                type ""Character""
                using ""_Troll""
                data ""Act part"" ""4""
                data ""Flags"" ""KnockdownImmunity;PetrifiedImmunity;PoisonImmunity""
                data ""Talents"" ""AttackOfOpportunity""

                new entry ""_Orc""
                type ""Character""
                using """"
                data ""Act part"" ""2""
                data ""Flags"" ""BleedImmunity;""
                data ""Talents"" ""Guerilla;WildBeast""
            ";
        }

        [Fact]
        public void Deserialize_MultipleEntries_MapsMultipleEntries()
        {
            // arrange
            
            // act
            var actual = SUT.Deserialize(_data);

            // assert
            actual.Count.Should().Be(2);
        }

        [Fact]
        public void Deserialize_MultipleEntries_MapsTalents()
        {
            // arrange
            
            // act
            var actual = SUT.Deserialize(_data);

            // assert
            actual[1].Data["Talents"].Should().Be("Guerilla;WildBeast");
        }

        [Fact]
        public void Deserialize_MultipleEntries_MapsUsing()
        {
            // arrange
            
            // act
            var actual = SUT.Deserialize(_data);

            // assert
            actual[0].Using.Should().Be("_Troll");
        }
    }
}