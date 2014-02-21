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
                throw new ArgumentException("The expression does not refer to a property.");

            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
                throw new ArgumentException("The expression does not refer to a property.");

            return propertyInfo;
        }
    }
}
