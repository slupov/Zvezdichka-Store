using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Remotion.Linq.Parsing.Structure;

namespace Zvezdichka.Services.Extensions
{
    public static class QueryableExtensions
    {
        private static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();

        private static readonly FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryCompiler");

        private static readonly PropertyInfo NodeTypeProviderField = QueryCompilerTypeInfo.DeclaredProperties.Single(x => x.Name == "NodeTypeProvider");

        private static readonly MethodInfo CreateQueryParserMethod = QueryCompilerTypeInfo.DeclaredMethods.First(x => x.Name == "CreateQueryParser");

        private static readonly FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");

        private static readonly PropertyInfo DatabaseDependenciesField
            = typeof(Database).GetTypeInfo().DeclaredProperties.Single(x => x.Name == "Dependencies");

        public static string ToSql<TEntity>(this IQueryable<TEntity> query) where TEntity : class
        {
//            if (!(query is EntityQueryable<TEntity>) && !(query is InternalDbSet<TEntity>))
//            {
//                throw new ArgumentException("Invalid query");
//            }

            var queryCompiler = (IQueryCompiler)QueryCompilerField.GetValue(query.Provider);
            var nodeTypeProvider = (INodeTypeProvider)NodeTypeProviderField.GetValue(queryCompiler);

            var parser = (IQueryParser)CreateQueryParserMethod.Invoke(queryCompiler, new object[] { nodeTypeProvider });
            var queryModel = parser.GetParsedQuery(query.Expression);
            var database = DataBaseField.GetValue(queryCompiler);

            var queryCompilationContextFactory = ((DatabaseDependencies)DatabaseDependenciesField.GetValue(database)).QueryCompilationContextFactory;
            var queryCompilationContext = queryCompilationContextFactory.Create(false);

            var modelVisitor = (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();
            modelVisitor.CreateQueryExecutor<TEntity>(queryModel);
            var sql = modelVisitor.Queries.First().ToString();

            return sql;
        }

        //        public static IQueryable<T> Include<T>(this IQueryable<T> query, Expression<Func<T, object>> selector)
        //        {
        //            IQueryable<T> path = new PropertyPathVisitor().GetPropertyPath<T>(query, selector);
        //            return query.Include(selector);
        //            //selector was path
        //        }
        //
        //        class PropertyPathVisitor : ExpressionVisitor
        //        {
        //            private Stack<string> stack;
        //
        //            public IQueryable<T> GetPropertyPath<T>(IQueryable<T> query, Expression expression)
        //            {
        //
        //            }
        //
        //            protected override Expression VisitMember(MemberExpression expression)
        //            {
        //                if (this.stack != null)
        //                    this.stack.Push(expression.Member.Name);
        //                return base.VisitMember(expression);
        //            }
        //
        //            protected override Expression VisitMethodCall(MethodCallExpression expression)
        //            {
        //                if (IsLinqOperator(expression.Method))
        //                {
        //                    for (int i = 1; i < expression.Arguments.Count; i++)
        //                    {
        //                        Visit(expression.Arguments[i]);
        //                    }
        //                    Visit(expression.Arguments[0]);
        //                    return expression;
        //                }
        //                return base.VisitMethodCall(expression);
        //            }
        //
        //            private static bool IsLinqOperator(MethodInfo method)
        //            {
        //                if (method.DeclaringType != typeof(Queryable) && method.DeclaringType != typeof(Enumerable))
        //                    return false;
        //                return Attribute.GetCustomAttribute(method, typeof(ExtensionAttribute)) != null;
        //            }
        //        }
    }
}
