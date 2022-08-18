using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Store_TechniaclTask.Services.HelperServices
{
    public class DependencyValidator<TEntity> where TEntity : class
    {
        private static readonly Type IEnumerableType = typeof(System.Collections. IEnumerable);
        private static readonly Type StringType = typeof(string);

        public static IEnumerable<KeyValuePair<string, IEnumerable<object>>> Dependencies(TEntity entity)
        {
            if (entity == null)
            {
                return Enumerable.Empty<KeyValuePair<string, IEnumerable<object>>>();
            }

            var dependents = new List<KeyValuePair<string, IEnumerable<object>>>();

            var properties = entity.GetType()
                .GetProperties()
                .Where(p => IEnumerableType.IsAssignableFrom(p.PropertyType) && !StringType.IsAssignableFrom(p.PropertyType));

            foreach (var property in properties)
            {
                var values = property.GetValue(entity) as IEnumerable<Type>;
                var children = (from object value in values select value).ToList();

                dependents.Add(new KeyValuePair<string, IEnumerable<object>>(property.Name, children));
            }

            return dependents;
        }
        public static IEnumerable<string> DependenciesNames(TEntity entity)
        {
            if (entity == null)
            {
                return Enumerable.Empty<string>();
            }

            var dependents = new List<string>();

            var properties = entity.GetType()
                .GetProperties()
                .Where(p => IEnumerableType.IsAssignableFrom(p.PropertyType) && !StringType.IsAssignableFrom(p.PropertyType));

            foreach (var property in properties)
            {
                //var values =;
                //var children = (from object value in values select value).ToList();
                if ((property.GetValue(entity) as IQueryable<Type>).Any())
                {
                dependents.Add(property.Name);
                }
            }

            return dependents;
        }
    }
}
