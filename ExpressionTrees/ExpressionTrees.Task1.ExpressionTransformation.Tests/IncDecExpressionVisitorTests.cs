using ExpressionTrees.Task1.ExpressionsTransformer;
using FluentAssertions;
using System.Linq.Expressions;
using Xunit;

namespace ExpressionTrees.Task1.ExpressionTransformation.Tests
{
    public class IncDecExpressionVisitorTests
    {
        [Fact]
        public void Visit_WhenValueIsIncrementing_ShouldReturnExpressionWithPostIncrement()
        {
            // Arrange
            Expression<Func<int, int>> expression = (data) => data + 1;
            var incDecVisitor = new IncDecExpressionVisitor();
            var expected = "data => data++";

            // Act
            var incDecResult = incDecVisitor.Visit(expression);

            // Assert
            incDecResult.ToString().Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Visit_WhenValueIsDecrementing_ShouldReturnExpressionWithPostIncrement()
        {
            // Arrange
            Expression<Func<int, int>> expression = (data) => data - 1;
            var incDecVisitor = new IncDecExpressionVisitor();
            var expected = "data => data--";

            // Act
            var incDecResult = incDecVisitor.Visit(expression);

            // Assert
            incDecResult.ToString().Should().BeEquivalentTo(expected);
        }
    }
}