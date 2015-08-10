using System;
using System.Reflection;
using Brick.FluentNHibernate.Conventions.Conventions;

namespace Brick.FluentNHibernate.Conventions
{
    /// <summary>
    ///     Use this class when you need to override the default identifier type and/or name
    ///     By default looks for 'Id' field, then looks for IdAttribute.
    ///     You can override column name as ColumnName attribute value on the attribute.
    /// </summary>
    public abstract class Identity
    {
        private int? _oldHashCode;

        public override bool Equals(object obj)
        {
            var thisIdProperty = GetIdProperty(obj);
            var other = obj as Identity;

            if (other == null) return false;

            var otherIdProperty = PrimaryKeyConvention.TryFindIdProperty(other.GetType());

            var thisId = thisIdProperty.GetValue(this);
            var otherId = otherIdProperty.GetValue(obj);

            var defaultOther = GetDefault(otherIdProperty.PropertyType);
            var defaultThis = GetDefault(thisIdProperty.PropertyType);

            var otherIsTransient = Equals(otherId, defaultOther);
            var thisIsTransient = Equals(thisId, defaultThis);

            if (otherIsTransient && thisIsTransient) return ReferenceEquals(other, this);

            return otherId.Equals(thisId);
        }

        private static PropertyInfo GetIdProperty(object obj)
        {
            var thisIdProperty = PrimaryKeyConvention.TryFindIdProperty(obj.GetType());
            if (thisIdProperty == null)
                throw new Exception("Could not find a property responsible for the primary key, either create a property with the name 'id' or use 'Id' attribute on an existing one.");
            return thisIdProperty;
        }

        private object GetDefault(Type t)
        {
            var methodInfo = GetType().GetMethod("GetDefaultGeneric", BindingFlags.NonPublic | BindingFlags.Instance);
            return methodInfo.MakeGenericMethod(t).Invoke(this, null);
        }

        protected T GetDefaultGeneric<T>()
        {
            return default(T);
        }

        public override int GetHashCode()
        {
            var idProp = GetIdProperty(this);
            var id = idProp.GetValue(this);

            if (_oldHashCode.HasValue) return _oldHashCode.Value;
            var thisIsTransient = Equals(id, GetDefault(idProp.GetType()));

            if (!thisIsTransient) return id.GetHashCode();

            _oldHashCode = base.GetHashCode();
            return _oldHashCode.Value;
        }
    }


    /// <summary>
    ///     Used by HNibernate to track identity of hidrated entities as well as to distinguish if the entity is persisted or
    ///     not.
    /// </summary>
    /// <remarks>
    ///     According to nhibernate as soon as the object is saved to the database two objects with the same primary key are
    ///     identical.
    ///     Untill then the hashcode is used to compare two transient objects.
    /// </remarks>
    public abstract class Identity<T> : Identity
    {
        public virtual T Id { get; set; }
    }
}