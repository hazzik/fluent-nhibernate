using System;
using System.Linq.Expressions;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Identity;

[Serializable]
public class IdMapping(AttributeStore underlyingStore) : ColumnBasedMappingBase(underlyingStore), IIdentityMapping, IEquatable<IdMapping>
{
    public IdMapping()
        : this(new AttributeStore())
    {}

    public Member Member { get; set; }

    public GeneratorMapping Generator => attributes.GetOrDefault<GeneratorMapping>();

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
        visitor.ProcessId(this);

        foreach (var column in Columns)
            visitor.Visit(column);

        if (Generator is not null)
            visitor.Visit(Generator);
    }

    public string Name => attributes.GetOrDefault<string>();

    public string Access => attributes.GetOrDefault<string>();

    public TypeReference Type => attributes.GetOrDefault<TypeReference>();

    public string UnsavedValue => attributes.GetOrDefault<string>();

    public Type ContainingEntityType { get; set; }

    public void Set(Expression<Func<IdMapping, object>> expression, int layer, object value)
    {
        Set(expression.ToMember().Name, layer, value);
    }

    protected override void Set(string attribute, int layer, object value)
    {
        attributes.Set(attribute, layer, value);
    }

    public bool Equals(IdMapping other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return base.Equals(other) && Equals(other.Member, Member) && other.ContainingEntityType == ContainingEntityType;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return Equals(obj as IdMapping);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int result = base.GetHashCode();
            result = (result * 397) ^ (Member is not null ? Member.GetHashCode() : 0);
            result = (result * 397) ^ (ContainingEntityType is not null ? ContainingEntityType.GetHashCode() : 0);
            return result;
        }
    }

    public override bool IsSpecified(string attribute)
    {
        return attributes.IsSpecified(attribute);
    }
}
