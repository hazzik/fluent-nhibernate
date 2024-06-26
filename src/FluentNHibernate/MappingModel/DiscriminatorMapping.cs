﻿using System;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel;

[Serializable]
public class DiscriminatorMapping(AttributeStore underlyingStore) : ColumnBasedMappingBase(underlyingStore), IEquatable<DiscriminatorMapping>
{
    public DiscriminatorMapping()
        : this(new AttributeStore())
    {}

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
        visitor.ProcessDiscriminator(this);

        foreach (var column in Columns)
            visitor.Visit(column);
    }

    public bool Force => attributes.GetOrDefault<bool>();

    public bool Insert => attributes.GetOrDefault<bool>();

    public string Formula => attributes.GetOrDefault<string>();

    public TypeReference Type => attributes.GetOrDefault<TypeReference>();

    public Type ContainingEntityType { get; set; }

    public bool Equals(DiscriminatorMapping other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return other.ContainingEntityType == ContainingEntityType &&
               other.Columns.ContentEquals(Columns) &&
               Equals(other.attributes, attributes);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != typeof(DiscriminatorMapping)) return false;
        return Equals((DiscriminatorMapping)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((ContainingEntityType is not null ? ContainingEntityType.GetHashCode() : 0) * 397) ^ ((Columns is not null ? Columns.GetHashCode() : 0) * 397) ^ (attributes is not null ? attributes.GetHashCode() : 0);
        }
    }

    public void Set(Expression<Func<DiscriminatorMapping, object>> expression, int layer, object value)
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
