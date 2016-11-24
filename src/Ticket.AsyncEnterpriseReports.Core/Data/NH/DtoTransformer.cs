using NHibernate.Transform;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;



namespace Ticket.AsyncEnterpriseReports.Core.Data.NHibernate
{
    public class InlineTransformer<T> : IResultTransformer
    {
        private readonly Type _result;
        private readonly Dictionary<string, PropertyInfo> _properties = new Dictionary<string, PropertyInfo>();
        private readonly Dictionary<PropertyInfo, object> _hardCodedProperties = new Dictionary<PropertyInfo, object>();
        private readonly Dictionary<PropertyInfo, Func<T, bool>> _dynamicBooleanProperties = new Dictionary<PropertyInfo, Func<T, bool>>();
        
        public InlineTransformer()
        {
            _result = typeof(T);
        }

        public InlineTransformer<T> Property<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            var propertyName = ((MemberExpression)expression.Body).Member.Name;

            _properties.Add(propertyName, _result.GetProperty(propertyName));

            return this;
        }

        public InlineTransformer<T> Property<TProperty>(Expression<Func<T, TProperty>> expression, string columnName)
        {
            var propertyName = ((MemberExpression)expression.Body).Member.Name;

            _properties.Add(columnName, _result.GetProperty(propertyName));

            return this;
        }

        public InlineTransformer<T> HardCodedProperty<TProperty>(Expression<Func<T, TProperty>> expression, TProperty value)
        {
            var propertyName = ((MemberExpression)expression.Body).Member.Name;

            _hardCodedProperties.Add(_result.GetProperty(propertyName), value);

            return this;
        }


        public InlineTransformer<T> HardCodedBooleanProperty<TProperty>(Expression<Func<T, TProperty>> expression, Func<T, bool> predicate)
        {
            var propertyName = ((MemberExpression)expression.Body).Member.Name;

            _dynamicBooleanProperties.Add(_result.GetProperty(propertyName), predicate);

            return this;
        }

      

        public object TransformTuple(object[] tuple, string[] aliases)
        {
            object instance = Activator.CreateInstance(_result);

            //foreach (var alias in aliases) {
            //    Log.To(this).Message("Alias[{0}]", alias).Debug();
            //}

            foreach (var property in _properties)
            {
                var index = Array.IndexOf(aliases, property.Key);

                if (index == -1)
                    throw new Exception(string.Format("Field {0} is not found on result set!",property.Key));

                var tupleValue = tuple[index];

                var propertyType = string.Empty;

                if (tupleValue != null)
                {
                    tupleValue = NormalizeValue(property.Value.PropertyType, tupleValue);

                    if (property.Value.PropertyType != null)
                        propertyType = property.Value.PropertyType.ToString();

                }


                var propertyValue = string.Empty;


                if (tupleValue != null)
                    propertyValue = tupleValue.ToString();

                //Log.To(this).Message("Tuple[{0}]: {1} -> {2}", property.Key, propertyType, propertyValue).Debug();
                
                property.Value.SetValue(instance, tupleValue, null);
            }


            foreach (var hardCodedProperty in _hardCodedProperties)
            {
                //Log.To(this).Message("HardCoded[{0}] -> {1}", hardCodedProperty.Key.Name, hardCodedProperty.Value.ToString()).Debug();

                hardCodedProperty.Key.SetValue(instance, hardCodedProperty.Value, null);
            }

            Func<T,bool> predicate;
            var futureBoolValue = false;

            foreach (var booleanProperty in _dynamicBooleanProperties)
            {
                //Log.To(this).Message("HardCoded[{0}] -> {1}", hardCodedProperty.Key.Name, hardCodedProperty.Value.ToString()).Debug();

                predicate = booleanProperty.Value;
                futureBoolValue = predicate((T)instance);

                booleanProperty.Key.SetValue(instance,futureBoolValue , null);
            }

            return instance;
        }

        private object NormalizeValue(Type propertyType, object value)
        {
            object obj;

            if (propertyType != value.GetType())
            {
                if (Nullable.GetUnderlyingType(propertyType) != null)
                {
                    var innerType = propertyType.GetGenericArguments()[0];
                    obj = Convert.ChangeType(value, innerType);
                }
                else
                {
                    obj = Convert.ChangeType(value, propertyType);
                }
            }
            else
            {
                obj = value;
            }

            return obj;
        }

        public IList TransformList(IList collection)
        {
            return collection;
        }
    }
}
