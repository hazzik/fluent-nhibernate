using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Testing.Testing;
using NUnit.Framework;

namespace FluentNHibernate.Testing.MappingModel.Output
{
    [TestFixture]
    public class XmlManyToAnyWriterTester
    {
        private IXmlWriter<ManyToAnyMapping> writer;
        readonly XmlWriterTestHelper<ManyToAnyMapping> testHelper = new XmlWriterTestHelper<ManyToAnyMapping>();

        [SetUp]
        public void GetWriterFromContainer()
        {
            var container = new XmlWriterContainer();
            writer = container.Resolve<IXmlWriter<ManyToAnyMapping>>();
        }

        [Test]
        public void ShouldWriteIdTypeAttribute()
        {
            testHelper.Check(x => x.IdType, "id").MapsToAttribute("id-type");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteMetaTypeAttribute()
        {
            testHelper.Check(x => x.MetaType, new TypeReference("meta")).MapsToAttribute("meta-type");

            testHelper.VerifyAll(writer);
        }

        [Test]
        public void ShouldWriteTypeColumns()
        {
            var mapping = new ManyToAnyMapping();

            mapping.AddTypeColumn(Layer.Defaults, new ColumnMapping("Column1"));

            writer.VerifyXml(mapping)
                .Element("column").Exists();
        }

        [Test]
        public void ShouldWriteIdentifierColumns()
        {
            var mapping = new ManyToAnyMapping();

            mapping.AddIdentifierColumn(Layer.Defaults, new ColumnMapping("Column1"));

            writer.VerifyXml(mapping)
                .Element("column").Exists();
        }

        [Test]
        public void ShouldWriteTypeColumnsBeforeIdentifiers()
        {
            var mapping = new ManyToAnyMapping();

            mapping.AddIdentifierColumn(Layer.Defaults, new ColumnMapping("Column1"));
            mapping.AddTypeColumn(Layer.Defaults, new ColumnMapping("Column2"));

            writer.VerifyXml(mapping)
                .Element("column[1]").HasAttribute("name", "Column2");
        }

        [Test]
        public void ShouldWriteMetaValues()
        {
            var mapping = new ManyToAnyMapping();

            mapping.AddMetaValue(new MetaValueMapping());

            writer.VerifyXml(mapping)
                .Element("meta-value").Exists();
        }
    }
}