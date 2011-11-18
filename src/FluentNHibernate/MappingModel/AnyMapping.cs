using System;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public sealed class AnyMapping : AnyMappingBase, IEquatable<AnyMapping>
    {
        public AnyMapping()
            : this(new AttributeStore())
        {}

        public AnyMapping(AttributeStore attributes)
            : base(attributes)
        {}

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessAny(this);

            foreach (var metaValue in MetaValues)
                visitor.Visit(metaValue);

            foreach (var column in TypeColumns)
                visitor.Visit(column);

            foreach (var column in IdentifierColumns)
                visitor.Visit(column);
        }

        public string Name
        {
            get { return attributes.GetOrDefault<string>("Name"); }
        }

        public string Access
        {
            get { return attributes.GetOrDefault<string>("Access"); }
        }

        public bool Insert
        {
            get { return attributes.GetOrDefault<bool>("Insert"); }
        }

        public bool Update
        {
            get { return attributes.GetOrDefault<bool>("Update"); }
        }

        public string Cascade
        {
            get { return attributes.GetOrDefault<string>("Cascade"); }
        }

        public bool Lazy
        {
            get { return attributes.GetOrDefault<bool>("Lazy"); }
        }

        public bool OptimisticLock
        {
            get { return attributes.GetOrDefault<bool>("OptimisticLock"); }
        }

        public bool Equals(AnyMapping other)
        {
            return base.Equals(other);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as AnyMapping);
        }

        public void Set<T>(Expression<Func<AnyMapping, T>> expression, int layer, T value)
        {
            Set(expression.ToMember().Name, layer, value);
        }

        protected override void Set(string attribute, int layer, object value)
        {
            attributes.Set(attribute, layer, value);
        }

        public override bool IsSpecified(string attribute)
        {
            return attributes.IsSpecified(attribute);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}