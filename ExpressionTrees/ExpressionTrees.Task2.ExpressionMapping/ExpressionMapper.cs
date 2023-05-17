using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTrees.Task2.ExpressionMapping
{
    public class ExpressionMapper
    {
        public static Func<TSource, TDestination> CreateMap<TSource, TDestination>(Dictionary<string, string> customMappings = null)
        {
            var sourceParameter = Expression.Parameter(typeof(TSource), "source");
            var destinationVariable = Expression.Variable(typeof(TDestination), "destination");
            var createDestination = Expression.Assign(destinationVariable, Expression.New(typeof(TDestination)));

            var propertyMappings = new List<MemberBinding>();

            foreach (var destinationProperty in typeof(TDestination).GetProperties())
            {
                string sourcePropertyName = destinationProperty.Name;

                if (customMappings != null && customMappings.ContainsKey(destinationProperty.Name))
                    sourcePropertyName = customMappings[destinationProperty.Name];

                var sourceProperty = typeof(TSource).GetProperty(sourcePropertyName);

                if (sourceProperty != null && sourceProperty.PropertyType == destinationProperty.PropertyType)
                {
                    var sourcePropertyValue = Expression.Property(sourceParameter, sourceProperty);
                    var destinationPropertyAssignment = Expression.Bind(destinationProperty, sourcePropertyValue);
                    propertyMappings.Add(destinationPropertyAssignment);
                }
            }

            var memberInit = Expression.MemberInit(Expression.New(typeof(TDestination)), propertyMappings);

            var returnStatement = Expression.Label(typeof(TDestination));
            var returnExpression = Expression.Return(returnStatement, memberInit, typeof(TDestination));
            var labelExpression = Expression.Label(returnStatement, Expression.Default(typeof(TDestination)));

            var blockExpression = Expression.Block(new[] { destinationVariable }, createDestination, returnExpression, labelExpression);
            return Expression.Lambda<Func<TSource, TDestination>>(blockExpression, sourceParameter).Compile();
        }
    }
}
