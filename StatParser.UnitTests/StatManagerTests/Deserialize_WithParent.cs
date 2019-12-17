using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;

namespace StatParser.UnitTests.StatManagerTests
{
    public class Deserialize_WithParent : StatManagerFixture
    {
        private readonly string _data;

        public Deserialize_WithParent()
        {
            // arrange
            _data = @"
                new entry ""_Orc""
                type ""Character""
                using """"
                data ""Act part"" ""2""
                data ""Flags"" ""BleedImmunity;""
                data ""Talents"" ""Guerilla;WildBeast""

                new entry ""Red Ork""
                type ""Character""
                using ""_Orc""
                data ""Act part"" ""3""
                data ""Talents"" ""Guerilla;""
            ";
        }

        [Fact]
        public void Deserialize_WithParent_MapsInheritedFlag()
        {
            // arrange
            
            // act
            var actual = SUT.Deserialize(_data);

            // assert
            actual[1].Data["Flags"].Should().Be("BleedImmunity;");
        }
    }}
