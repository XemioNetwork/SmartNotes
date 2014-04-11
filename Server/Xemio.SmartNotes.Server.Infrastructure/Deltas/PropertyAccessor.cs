﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Server.Infrastructure.Deltas
{
    internal abstract class PropertyAccessor<TEntityType> where TEntityType : class
    {
        protected PropertyAccessor(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }
            Property = property;
            if (Property.GetGetMethod() == null || Property.GetSetMethod() == null)
            {
                throw new ArgumentException("Property Must Have Public Getter And Setter", "property");
            }
        }

        public PropertyInfo Property
        {
            get;
            private set;
        }

        public void Copy(TEntityType from, TEntityType to)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }
            if (to == null)
            {
                throw new ArgumentNullException("to");
            }
            SetValue(to, GetValue(from));
        }

        public abstract object GetValue(TEntityType entity);

        public abstract void SetValue(TEntityType entity, object value);
    }
}
