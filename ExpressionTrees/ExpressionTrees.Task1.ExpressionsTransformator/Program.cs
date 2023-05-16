/*
 * Create a class based on ExpressionVisitor, which makes expression tree transformation:
 * 1. converts expressions like <variable> + 1 to increment operations, <variable> - 1 - into decrement operations.
 * 2. changes parameter values in a lambda expression to constants, taking the following as transformation parameters:
 *    - source expression;
 *    - dictionary: <parameter name: value for replacement>
 * The results could be printed in console or checked via Debugger using any Visualizer.
 */
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Expression Visitor for increment/decrement.");
            Console.WriteLine();

            Expression<Func< int, int>> expression
                = (data) => data + 1;

            var incDecVisitor = new IncDecExpressionVisitor();
            var replacement = new Dictionary<string, object>();
            replacement["data"] = 7;
            var replaceVisitor = new ReplaceParamsExpressionVisitor(expression, replacement).ReplaceWithConstants();
            var testData = 10;
            
            var incDecResult = incDecVisitor.Visit(expression);

            var replaceResult = replaceVisitor.ToString();

            Console.ReadLine();
        }
    }
}
