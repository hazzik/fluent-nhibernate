using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.FluentInterfaceTests
{
    [TestFixture]
    public class WhereTests
    {
        private class StaticExample
        {
            public static string SomeValue = "SomeValue";
        }

        [Test]
        public void ShouldAllowStaticClassMemberReference()
        {
            Where(x => x.String == StaticExample.SomeValue)
                .ShouldEqual("String = 'SomeValue'");
        }

        const string SomeValue = "SomeValue";

        [Test]
        public void ShouldAllowConst()
        {
            Where(x => x.String == SomeValue)
                .ShouldEqual("String = 'SomeValue'");
        }

        [Test]
        public void ShouldAllowLocalVariable()
        {
            var local = "someValue";

            Where(x => x.String == local)
                .ShouldEqual("String = 'someValue'");
        }

        [Test]
        public void ShouldAllowIntEqualsInt()
        {
            Where(x => x.Int == 1)
                .ShouldEqual("Int = 1");
        }

        [Test]
        public void ShouldAllowEnumEqualsEnum()
        {
            // this will only work if the enum is mapped as an int...
            Where(x => x.Enum == Enum.One)
                .ShouldEqual("Enum = 1");
        }

        [Test]
        public void ShouldAllowStringEqualsString()
        {
            Where(x => x.String == "1")
                .ShouldEqual("String = '1'");
        }

        [Test]
        public void ShouldAllowNotEquals()
        {
            Where(x => x.String != "1")
                .ShouldEqual("String != '1'");
        }

        [Test]
        public void ShouldAllowGreater()
        {
            Where(x => x.Int > 1)
                .ShouldEqual("Int > 1");
        }

        [Test]
        public void ShouldAllowLess()
        {
            Where(x => x.Int < 1)
                .ShouldEqual("Int < 1");
        }

        [Test]
        public void ShouldAllowGreaterOrEqual()
        {
            Where(x => x.Int >= 1)
                .ShouldEqual("Int >= 1");
        }

        [Test]
        public void ShouldAllowLessOrEqual()
        {
            Where(x => x.Int <= 1)
                .ShouldEqual("Int <= 1");
        }

        [Test]
        public void ShouldAllowWhereAsString()
        {
            Where("some where clause")
                .ShouldEqual("some where clause");
        }

        [Test]
        public void ShouldAllowUsePropertyName()
        {
            Where(x => x.WithCustomName == 1)
                .ShouldEqual("CUSTOM = 1");
        }

        #region helpers

        static string Where(Expression<Func<Child, bool>> where)
        {
            return Configure(part => part.Where(where));
        }

        static string Where(string where)
        {
            return Configure(part => part.Where(where));
        }

        static string Configure(Action<OneToManyPart<Child>> oneToManyPart)
        {
            var target = new ClassMap<Target>();
            target.Id(x => x.Id);

            oneToManyPart(target.HasMany(x => x.Children));

            var child = new ClassMap<Child>();
            child.Map(x => x.WithCustomName)
                .Column("CUSTOM");

            var model = new PersistenceModel();

            model.Add(target);

            return model.BuildMappings()
                .First()
                .Classes.First()
                .Collections.First()
                .Where;
        }

        #endregion

        private class Target
        {
            public int Id { get; set; }
            public IList<Child> Children { get; set;}
        }

        private class Child
        {
            public string String { get; set; }
            public int Int { get; set; }
            public Enum Enum { get; set; }
            public int WithCustomName { get; set; }
        }

        private enum Enum
        {
            One = 1
        }
    }
}