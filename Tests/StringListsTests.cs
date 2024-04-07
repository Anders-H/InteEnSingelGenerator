using InteEnSingelGenerator;

namespace Tests
{
    [TestClass]
    public class StringListsTests
    {
        [TestMethod]
        public void SpeakList0()
        {
            var a = new StringList();
            Assert.AreEqual("", a.SpeakList());
        }

        [TestMethod]
        public void SpeakList1()
        {
            var a = new StringList { "A" };
            Assert.AreEqual("A", a.SpeakList());
        }

        [TestMethod]
        public void SpeakList2()
        {
            var a = new StringList { "A", "B" };
            Assert.AreEqual("A och B", a.SpeakList());
        }

        [TestMethod]
        public void SpeakList3()
        {
            var a = new StringList { "A", "B", "C" };
            Assert.AreEqual("A, B och C", a.SpeakList());
        }

        [TestMethod]
        public void SpeakList4()
        {
            var a = new StringList { "A", "B", "C", "D" };
            Assert.AreEqual("A, B, C och D", a.SpeakList());
        }

        [TestMethod]
        public void CommaList0()
        {
            var a = new StringList();
            Assert.AreEqual("", a.CommaList());
        }

        [TestMethod]
        public void CommaList1()
        {
            var a = new StringList { "A" };
            Assert.AreEqual("A", a.CommaList());
        }

        [TestMethod]
        public void CommaList2()
        {
            var a = new StringList { "A", "B" };
            Assert.AreEqual("A, B", a.CommaList());
        }

        [TestMethod]
        public void CommaList3()
        {
            var a = new StringList { "A", "B", "C" };
            Assert.AreEqual("A, B, C", a.CommaList());
        }

        [TestMethod]
        public void CommaList4()
        {
            var a = new StringList { "A", "B", "C", "D" };
            Assert.AreEqual("A, B, C, D", a.CommaList());
        }
    }
}