using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Shared.Helpers
{
    public static class ReflectionHelper
    {
        public static PropertyInfo GetProperty<T>(Expression<Func<T, object>> propertySelector)
        {
            if (propertySelector == null)
                throw new ArgumentNullException("propertySelector");

            var memberExpression = propertySelector.Body as MemberExpression;
            if (memberExpression == null)
            {
                var unaryExpression = propertySelector.Body as UnaryExpression;
                if (unaryExpression == null)
                    throw new ArgumentException("The expression does not refer to a property.");

                memberExpression = unaryExpression.Operand as MemberExpression;
            }

            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
                throw new ArgumentException("The expression does not refer to a property.");

            return propertyInfo;
        }
        public static MemberInfo GetMemberInfo(Expression expression)
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
