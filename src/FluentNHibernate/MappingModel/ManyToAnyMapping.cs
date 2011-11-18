using System;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public sealed class ManyToAnyMapping : AnyMappingBase, IEquatable<ManyToAnyMapping>
    {
        public ManyToAnyMapping()
            : base(new AttributeStore())
        {}

        public ManyToAnyMapping(AttributeStore attributes)
            : base(attributes)
        {}

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessManyToAny(this);

            foreach (var metaValue in MetaValues)
                visitor.Visit(metaValue);

            foreach (var column in TypeColumns)
                visitor.Visit(column);

            foreach (var column in IdentifierColumns)
                visitor.Visit(column);
        }

        public bool Equals(ManyToAnyMapping other)
        {
            return base.Equals(other);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ManyToAnyMapping);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}