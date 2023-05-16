using ExpressionTrees.Task1.ExpressionsTransformer;
using FluentAssertions;
using System.Linq.Expressions;
using Xunit;

namespace ExpressionTrees.Task1.ExpressionTransformation.Tests
{
    public class ReplaceParamsExpressionVisitorTests
    {
        [Fact]
        public void ReplaceWithConstants_WhenExpressionAndDictionaryPassed_ShouldReplaceVariablesWithConstants() 
        {
            // Arrange
            Expression<Func<int, int, string, string>> expression
                = (a, b, c) => (a + 1) + (b - 1) + c;

            var replacement = new Dictionary<string, object>();
            replacement["a"] = 5;
            replacement["b"] = 7;
            replacement["c"] = "Test";

            // Act
            var replaceVisitor = new ReplaceParamsExpressionVisitor(expression, replacement).ReplaceWithConstants();

            // Assert
            replaceVisitor.ToString().Should().BeEquivalentTo("() => (Convert(((5 + 1) + (7 - 1)), Object) + \"Test\")");
        }

        [Fact]
        public void ReplaceWithConstants_WhenExpressionAndDictionaryPassedAndCompiled_ShouldReturnCorrectResult()
        {
            // Arrange
            Expression<Func<int, int, string, string>> expression
                = (a, b, c) => (a + 1) + (b - 1) + c;

            var replacement = new Dictionary<string, object>();
            replacement["a"] = 5;
            replacement["b"] = 7;
            replacement["c"] = "Test";

            // Act
            var replaceVisitor = new ReplaceParamsExpressionVisitor(expression, replacement).ReplaceWithConstants();
            var result = replaceVisitor.Compile().DynamicInvoke().ToString();

            // Assert
            result.Should().BeEquivalentTo("12Test");
        }
    }
}
