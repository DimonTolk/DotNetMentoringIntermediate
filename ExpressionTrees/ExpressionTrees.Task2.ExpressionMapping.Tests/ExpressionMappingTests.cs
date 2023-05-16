using ExpressionTrees.Task2.ExpressionMapping.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Foo, Bar>();

            var foo = new Foo() { Id = 2, Name = "Some Name", Description = "Some Description" };
            var bar = mapper.Map(foo);

            //Assert.AreEqual(foo.Id, bar.HowOldYouOnTracker);
            //Assert.AreEqual(foo.DoYouLikeWorkOnAProject, bar.DoYouLikeWorkOnAProject);
            //Assert.AreEqual(foo.Why, bar.Why);
        }
    }
}
