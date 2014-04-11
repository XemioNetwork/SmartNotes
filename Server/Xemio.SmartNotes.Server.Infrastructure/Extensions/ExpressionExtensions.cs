using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Server.Infrastructure.Extensions
{
    /// <summary>
    /// Contains extension methods for the <see cref="Expression"/> class.
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Returns the <see cref="MemberInfo"/> of this <see cref="Expression"/>.
        /// </summary>
        /// <param name="expression">The expression.</param>
        public static MemberInfo GetMemberInfo(this Expression expression)
        {
            var lambda = (LambdaExpression)expression;

            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)lambda.Body;
            }

            return memberExpression.Member;
        }
    }
}
