using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public abstract class AnyMappingBase : MappingBase
    {
        protected readonly AttributeStore attributes;
        readonly LayeredColumns identifierColumns = new LayeredColumns();
        readonly IList<MetaValueMapping> metaValues = new List<MetaValueMapping>();
        readonly LayeredColumns typeColumns = new LayeredColumns();

        protected AnyMappingBase(AttributeStore attributes)
        {
            this.attributes = attributes;
        }

        public string IdType
        {
            get { return attributes.GetOrDefault<string>("IdType"); }
        }

        public TypeReference MetaType
        {
            get { return attributes.GetOrDefault<TypeReference>("MetaType"); }
        }

        public IEnumerable<ColumnMapping> TypeColumns
        {
            get { return typeColumns.Columns; }
        }

        public IEnumerable<ColumnMapping> IdentifierColumns
        {
            get { return identifierColumns.Columns; }
        }

        public IEnumerable<MetaValueMapping> MetaValues
        {
            get { return metaValues; }
        }

        public Type ContainingEntityType { get; set; }

        public void AddMetaValue(MetaValueMapping metaValue)
        {
            metaValues.Add(metaValue);
        }

        public void AddTypeColumn(int layer, ColumnMapping column)
        {
            typeColumns.AddColumn(layer, column);
        }

        public void AddIdentifierColumn(int layer, ColumnMapping column)
        {
            identifierColumns.AddColumn(layer, column);
        }

        protected bool Equals(AnyMappingBase other)
        {
            return Equals(other.attributes, attributes) &&
                other.typeColumns.ContentEquals(typeColumns) &&
                other.identifierColumns.ContentEquals(identifierColumns) &&
                other.metaValues.ContentEquals(metaValues) &&
                other.ContainingEntityType == ContainingEntityType;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (attributes != null ? attributes.GetHashCode() : 0);
                result = (result * 397) ^ (typeColumns != null ? typeColumns.GetHashCode() : 0);
                result = (result * 397) ^ (identifierColumns != null ? identifierColumns.GetHashCode() : 0);
                result = (result * 397) ^ (metaValues != null ? metaValues.GetHashCode() : 0);
                result = (result * 397) ^ (ContainingEntityType != null ? ContainingEntityType.GetHashCode() : 0);
                return result;
            }
        }

        public override bool IsSpecified(string attribute)
        {
            return attributes.IsSpecified(attribute);
        }

        protected override void Set(string attribute, int layer, object value)
        {
            attributes.Set(attribute, layer, value);
        }
    }
}