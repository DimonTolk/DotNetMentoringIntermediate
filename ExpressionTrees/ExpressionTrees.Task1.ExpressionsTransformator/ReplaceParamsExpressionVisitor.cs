using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    public class ReplaceParamsExpressionVisitor : ExpressionVisitor
    {
        private readonly Expression _expression;
        private IDictionary<string, object> _dictionary;

        public ReplaceParamsExpressionVisitor(Expression expression, IDictionary<string, object> dictionary)
        {
            _expression = expression;
            _dictionary = dictionary;
        }

        public ReplaceParamsExpressionVisitor(Expression<Func<int, string, string>> expression, params Expression<Func<string, object>>[] dicFunc)
        {
            _expression = expression;

            var dic = new Dictionary<string, object>();
            foreach (var func in dicFunc)
            {
                dic.Add(func.Parameters[0].Name, func.Compile().Invoke(""));
            }

            _dictionary = dic;
        }

        public LambdaExpression ReplaceWithConstants() => (LambdaExpression)Visit(_expression);

        protected override Expression VisitParameter(ParameterExpression node)
        {

            if (node.NodeType == ExpressionType.Parameter)
            {
                var paramNode = node as ParameterExpression;
                if (_dictionary.ContainsKey(paramNode.Name))
                {
                    return Expression.Constant(_dictionary[paramNode.Name]);
                }
            }

            return base.VisitParameter(node);
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            var parameters = node.Parameters.Where(p => !_dictionary.ContainsKey(p.Name)).ToList();

            return Expression.Lambda(Visit(node.Body), parameters);
        }
    }
}
