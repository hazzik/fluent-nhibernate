using System;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Collections;

[Serializable]
public class OneToManyMapping(AttributeStore attributes) : MappingBase, ICollectionRelationshipMapping, IEquatable<OneToManyMapping>
{
    readonly AttributeStore attributes = attributes;

    public OneToManyMapping()
        : this(new AttributeStore())
    {}

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
        visitor.ProcessOneToMany(this);
    }

    public Type ChildType => attributes.GetOrDefault<Type>();

    public TypeReference Class => attributes.GetOrDefault<TypeReference>();

    public string NotFound => attributes.GetOrDefault<string>();

    public string EntityName => attributes.GetOrDefault<string>();

    public Type ContainingEntityType { get; set; }

    public bool Equals(OneToManyMapping other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals(other.attributes, attributes) && other.ContainingEntityType == ContainingEntityType;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != typeof(OneToManyMapping)) return false;
        return Equals((OneToManyMapping)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((attributes is not null ? attributes.GetHashCode() : 0) * 397) ^ (ContainingEntityType is not null ? ContainingEntityType.GetHashCode() : 0);
        }
    }

    public void Set<T>(Expression<Func<OneToManyMapping, T>> expression, int layer, T value)
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
}
