using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTrees.Task2.ExpressionMapping
{
    public class MappingGenerator
    {
        public Mapper<TSource, TDestination> Generate<TSource, TDestination>() where TDestination : new()
        {
            var sourceParameters = Expression.Parameter(typeof(TSource));
            var destitanionType = typeof(TDestination);
            var constructor = Expression.New(destitanionType);
            var initializedConstructor = Expression.MemberInit(constructor, CreateMemberBindings(destitanionType, sourceParameters));

            var mapFunction =
                Expression.Lambda<Func<TSource, TDestination>>(initializedConstructor, sourceParameters);

            return new Mapper<TSource, TDestination>(mapFunction.Compile());
        }

        private IList<MemberBinding> CreateMemberBindings(Type constructorType, ParameterExpression parameter)
        {
            var sourceType = parameter.Type;

            var sourceProperties = sourceType.GetProperties().Where(x => x.CanRead).ToList();
            var destinationProperties = constructorType.GetProperties().Where(x => x.CanWrite).ToList();

            IList<MemberBinding> memberBindings = new List<MemberBinding>();

            foreach (var sourceProp in sourceProperties)
            {
                var propName = sourceProp.Name;
                var destProp = destinationProperties.FirstOrDefault(x => x.Name == propName);

                if (destProp == null)
                {
                    continue;
                }

                var accessMember = Expression.MakeMemberAccess(parameter, sourceProp);
                var assignMember = Expression.Bind(destProp, accessMember);
                memberBindings.Add(assignMember);
            }

            return memberBindings;
        }
    }
}
