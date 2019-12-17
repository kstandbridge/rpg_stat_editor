using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace StatParser.UnitTests.StatManagerTests
{
    public class Serialize_WithParent : StatManagerFixture
    {
        private readonly string _data = @"new entry ""_Orc""
type ""Character""
using """"
data ""Act part"" ""2""
data ""Flags"" ""BleedImmunity;""
data ""Talents"" ""Guerilla;WildBeast""

new entry ""Red Ork""
type ""Character""
using ""_Orc""
data ""Act part"" ""3""
data ""Talents"" ""Guerilla;""";

        private readonly List<GameEntity> _entities;

        public Serialize_WithParent()
        {
            // arrange
            _entities = SUT.Deserialize(_data);
        }

        [Fact]
        public void Serialize_WithParent_DoesNotSaveInheritedData()
        {
            // arrange

            // act
            var actual = SUT.Serialize(_entities);

            // assert
            actual.Should().BeEquivalentTo(_data);
        }
    }
}