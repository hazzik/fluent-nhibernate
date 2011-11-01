using FluentNHibernate.Testing.DomainModel.Mapping;
using FluentNHibernate.Utils;
using NUnit.Framework;

namespace FluentNHibernate.Testing
{
    [TestFixture]
    public class ExpressionToSqlTests
    {
        [Test]
        public void ConvertPropertyToPropertyName()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => x.Name);

            sql.ShouldEqual("Name");
        }

        [Test]
        public void ConvertMethodToMethodName()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => x.StringMethod());

            sql.ShouldEqual("StringMethod");
        }

        [Test]
        public void ConvertIntToValue()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => 1);

            sql.ShouldEqual("1");
        }

        [Test]
        public void ConvertStringToValue()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => "1");

            sql.ShouldEqual("'1'");
        }

        [Test]
        public void ConvertEqualsOfTwoProperties()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => x.Name == x.Name);

            sql.ShouldEqual("Name = Name");
        }

        [Test]
        public void ConvertEqualsPropertyAndInt()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => x.Position == 1);

            sql.ShouldEqual("Position = 1");
        }

        [Test]
        public void ConvertEqualsPropertyAndString()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => x.Name == "1");

            sql.ShouldEqual("Name = '1'");
        }

        [Test]
        public void ConvertEqualsMethodAndString()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => x.StringMethod() == "1");

            sql.ShouldEqual("StringMethod = '1'");
        }


        [Test]
        public void ConvertBooleanMethod()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => x.BooleanMethod());

            sql.ShouldEqual("BooleanMethod = 1");
        }
        
        [Test]
        public void ConvertBooleanMethodImplicitFalse()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => !x.BooleanMethod());

            sql.ShouldEqual("BooleanMethod = 0");
        }

        [Test]
        public void ConvertEqualsPropertyAndTrue()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => x.Active == true);

            sql.ShouldEqual("Active = 1");
        }

        [Test]
        public void ConvertEqualsPropertyAndBooleanConstant()
        {
            var b = true;

            var sql = ExpressionToSql.Convert<ChildObject>(x => x.Active == b);

            sql.ShouldEqual("Active = 1");
        }
        
        [Test]
        public void ConvertBooleanConstant()
        {
            var b = true;

            var sql = ExpressionToSql.Convert<ChildObject>(x => b);

            sql.ShouldEqual("1 = 1");
        }

        [Test]
        public void ConvertBooleanStaticProperty()
        {
            var b = true;

            var sql = ExpressionToSql.Convert<ChildObject>(x => Values.StaticBooleanValue);

            sql.ShouldEqual("0 = 1");
        }

        [Test]
        public void ConvertBooleanNonStaticProperty()
        {
            var values = new Values();

            var sql = ExpressionToSql.Convert<ChildObject>(x => values.BooleanValue);

            sql.ShouldEqual("0 = 1");
        }

        [Test]
        public void ConvertEqualsPropertyAndFalse()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => x.Active == false);

            sql.ShouldEqual("Active = 0");
        }

        [Test]
        public void ConvertBooleanPropertyImplicitTrue()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => x.Active);

            sql.ShouldEqual("Active = 1");
        }

        [Test]
        public void ConvertBooleanPropertyImplicitFalse()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => !x.Active);

            sql.ShouldEqual("Active = 0");
        }

        [Test]
        public void ConvertGreater()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => x.Position > 1);

            sql.ShouldEqual("Position > 1");
        }

        [Test]
        public void ConvertLess()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => x.Position < 1);

            sql.ShouldEqual("Position < 1");
        }

        [Test]
        public void ConvertGreaterEquals()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => x.Position >= 1);

            sql.ShouldEqual("Position >= 1");
        }

        [Test]
        public void ConvertLessEquals()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => x.Position <= 1);

            sql.ShouldEqual("Position <= 1");
        }

        [Test]
        public void ConvertNot()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => x.Position != 1);

            sql.ShouldEqual("Position != 1");
        }

        [Test]
        public void ConvertLocalVariable()
        {
            var local = "someValue";
            var sql = ExpressionToSql.Convert<ChildObject>(x => local);

            sql.ShouldEqual("'someValue'");
        }

        private const string someValue = "someValue";

        [Test]
        public void ConvertConst()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => someValue);

            sql.ShouldEqual("'someValue'");
        }


        [Test]
        public void ConvertStaticMemberReference()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => Values.StaticValue);

            sql.ShouldEqual("'someValue'");
        }

        [Test]
        public void ConvertStaticNestedMemberReference()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => Values.StaticValue.IsNormalized());

            sql.ShouldEqual("1 = 1");
        }

        [Test]
        public void ConvertBooleanConst()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => true);

            sql.ShouldEqual("1 = 1");
        }

        [Test]
        public void ConvertStaticMethodCall()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => Values.Method());

            sql.ShouldEqual("'someValue'");
        }

        private enum Something
        {
            Else = 10
        }

        [Test]
        public void ConvertEnumMemberReferenceMethodCall()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => Something.Else.ToString());

            sql.ShouldEqual("'Else'");
        }

        [Test]
        public void ConvertEnumMemberReference()
        {
            var sql = ExpressionToSql.Convert<ChildObject>(x => Something.Else);

            sql.ShouldEqual("10");
        }

        class ChildObject
        {
            public virtual string Name { get; set; }
            public virtual int Position { get; set; }
            public virtual bool Active { get; set; }
            public virtual bool BooleanMethod()
            {
                return true;
            }
            public virtual string StringMethod()
            {
                return "stringMethod";
            }
        }

        private class Values
        {
            public bool BooleanValue;
            public static string StaticValue = "someValue";
            public static bool StaticBooleanValue;
            public static string Method()
            {
                return "someValue";
            }
        }
    }
}