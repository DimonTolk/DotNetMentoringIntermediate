using ExpressionTrees.Task2.ExpressionMapping.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ExpressionTrees.Task2.ExpressionMapping.Tests
{
    [TestClass]
    public class ExpressionMappingTests
    {
        [TestMethod]
        public void Map_WhenPropertiesAreSame_ShouldDirectlyMap()
        {
            // Arrange
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Foo, Bar>();

            var source = new Foo { Id = 1, Name = "name", Description = "description" };

            // Act
            var res = mapper.Map(source);

            // Assert
            Assert.IsInstanceOfType(res, typeof(Bar));
            Assert.AreEqual(res.Name, "name");
            Assert.AreEqual(res.Description, "description");
            Assert.AreEqual(res.Id, 1);
        }

        [TestMethod]
        public void Map_WhenPropertiesNamesAreDifferent_ShouldMapAccordingToTheRules()
        {
            var customMappings = new Dictionary<string, string>();
            customMappings["Id"] = "DifferentId";
            customMappings["Name"] = "DifferentName";
            customMappings["Description"] = "DifferentDescription";
            var mapFunction = ExpressionMapper.CreateMap<DifferentFoo, Foo> (customMappings);

            var source = new DifferentFoo() { DifferentId = 2, DifferentName = "Some Name", DifferentDescription = "Some Description" };
            var destination = mapFunction(source);
            Assert.AreEqual(source.DifferentId, destination.Id);
            Assert.AreEqual(source.DifferentName, destination.Name);
            Assert.AreEqual(source.DifferentDescription, destination.Description);
        }
    }
}
