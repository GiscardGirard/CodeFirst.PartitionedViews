using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirst.PartitionedViews.Adapters
{
    public abstract class PartitionedViewConfiguration
    {
        internal abstract void OnModelCreating(DbModelBuilder modelBuilder);

        public abstract IEnumerable<string> PrimaryKeyPropertyNames { get; }

        public abstract IEnumerable<string> DataRangeKeyPropertyNames { get; }
        public abstract Type DataType { get; }

        public abstract string ConnectionName { get; }
    }

    public class PartitionedViewConfiguration<T>:PartitionedViewConfiguration
    {
        public delegate void ModelCreatingHandler(DbModelBuilder modelBuilder);

        public event ModelCreatingHandler ModelCreating;

        public Expression<Func<T, Object>> PrimaryKeyExpression { get; set; }
        public Expression<Func<T, Object>> DataRangeKeyExpression { get; set; }
        private IEnumerable<string> GetPropertyNamesFromKeyExpression(Expression<Func<T, Object>> keyProperty)
        {
            var newExpression = keyProperty.Body as NewExpression;

            if (newExpression != null)
            {
                return newExpression.Arguments.Cast<MemberExpression>().Select(e => e.Member.Name);
            }
            var memberExpression = keyProperty.Body as MemberExpression;
            return new string[] { memberExpression.Member.Name };
        }
        override internal void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ModelCreating?.Invoke(modelBuilder);
        }        

        override public IEnumerable<string> PrimaryKeyPropertyNames
        {
            get { return GetPropertyNamesFromKeyExpression(PrimaryKeyExpression); }
        }
        override public IEnumerable<string> DataRangeKeyPropertyNames
        {
            get { return GetPropertyNamesFromKeyExpression(DataRangeKeyExpression); }
        }
        override public Type DataType
        {
            get { return typeof(T); }
        }       
        override public string ConnectionName
        {
            get { return String.Format("name={0}", typeof(T).Name); }
        }
    }
}
