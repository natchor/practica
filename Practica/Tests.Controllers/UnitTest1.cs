using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Tests.Controllers
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public Task Test1()
        {
            Console.WriteLine("saliendo");

            Assert.AreEqual("algo", "algo");
        }

        [Test]
        public void Test2()
        {
            Console.WriteLine("saliendo");

            Assert.AreEqual("algo", "algo");
        }


        [TearDown]
        public void Clearnup()
        {
            Console.WriteLine("saliendo");
        }
    }
}