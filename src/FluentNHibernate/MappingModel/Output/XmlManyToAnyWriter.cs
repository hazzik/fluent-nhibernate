using System.Xml;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel.Output
{
    public class XmlManyToAnyWriter : NullMappingModelVisitor, IXmlWriter<ManyToAnyMapping>
    {
        private readonly IXmlWriterServiceLocator serviceLocator;
        private XmlDocument document;

        public XmlManyToAnyWriter(IXmlWriterServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public XmlDocument Write(ManyToAnyMapping mappingModel)
        {
            document = null;
            mappingModel.AcceptVisitor(this);
            return document;
        }

        public override void ProcessManyToAny(ManyToAnyMapping mapping)
        {
            document = new XmlDocument();

            var element = document.AddElement("many-to-any");

            if (mapping.IsSpecified("IdType"))
                element.WithAtt("id-type", mapping.IdType);

            if (mapping.IsSpecified("MetaType"))
                element.WithAtt("meta-type", mapping.MetaType);
        }

        public override void Visit(ColumnMapping columnMapping)
        {
            var writer = serviceLocator.GetWriter<ColumnMapping>();
            var columnXml = writer.Write(columnMapping);

            document.ImportAndAppendChild(columnXml);
        }

        public override void Visit(MetaValueMapping mapping)
        {
            var writer = serviceLocator.GetWriter<MetaValueMapping>();
            var metaValueXml = writer.Write(mapping);

            document.ImportAndAppendChild(metaValueXml);
        }
    }
}