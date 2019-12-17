using System;
using System.Collections.Generic;
using AutoFixture;
using FluentAssertions;
using Xunit;

namespace StatParser.UnitTests.StatManagerTests
{
    public class Serialize_SingleEntry : StatManagerFixture
    {
        private readonly List<GameEntity> _entities = new List<GameEntity>();
        private readonly GameEntity _entity;

        public Serialize_SingleEntry()
        {
            // arrange
            _entity = new GameEntity
            {
                Name = Fixture.Create<string>(),
                Type = Fixture.Create<string>(),
                Using = Fixture.Create<string>(),
                Data = new Dictionary<string, string>
                {
                    {"Act part", Fixture.Create<uint>().ToString()},
                    {"Flags", Fixture.Create<string>()},
                    {"Talents", Fixture.Create<string>()}
                }
            };
            _entities.Add(_entity);
        }

        [Fact]
        public void Serialize_SingleEntry_MapsEntry()
        {
            // arrange

            // act
            var actual = SUT.Serialize(_entities);

            // assert
            var line = actual.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)[0];
            line.Should().Be($"new entry \"{_entity.Name}\"");
        }

        [Fact]
        public void Serialize_SingleEntry_MapsType()
        {
            // arrange

            // act
            var actual = SUT.Serialize(_entities);

            // assert
            var line = actual.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)[1];
            line.Should().Be($"type \"{_entity.Type}\"");
        }

        [Fact]
        public void Serialize_SingleEntry_MapsUsing()
        {
            // arrange

            // act
            var actual = SUT.Serialize(_entities);

            // assert
            var line = actual.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)[2];
            line.Should().Be($"using \"{_entity.Using}\"");
        }

        [Fact]
        public void Serialize_SingleEntry_MapsActPart()
        {
            // arrange

            // act
            var actual = SUT.Serialize(_entities);

            // assert
            var line = actual.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)[3];
            line.Should().Be($"data \"Act part\" \"{_entity.Data["Act part"]}\"");
        }

        [Fact]
        public void Serialize_SingleEntry_MapsFlags()
        {
            // arrange

            // act
            var actual = SUT.Serialize(_entities);

            // assert
            var line = actual.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)[4];
            line.Should().Be($"data \"Flags\" \"{_entity.Data["Flags"]}\"");
        }

        [Fact]
        public void Serialize_SingleEntry_MapsTalents()
        {
            // arrange

            // act
            var actual = SUT.Serialize(_entities);

            // assert
            var line = actual.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)[5];
            line.Should().Be($"data \"Talents\" \"{_entity.Data["Talents"]}\"");
        }
    }
}