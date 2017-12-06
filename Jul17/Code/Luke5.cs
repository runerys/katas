using System;
using System.Linq;
using NUnit.Framework;

namespace Code
{
    /// <summary>
    /// https://julekalender.knowit.no/challenges/cjamxarej59470118e57rlzwr
    /// </summary>
    public class Luke5
    {
        public Rekke Generer(int antall)
        {
            var rekken = new int[antall];
            var gjeldende = 1;
            var sisteAntall = 0;
            var målsetning = 1;

            for (int i = 1; i <= antall; i++)
            {
                rekken[i - 1] = gjeldende;

                if (sisteAntall < gjeldende)
                {
                    sisteAntall++;
                }

                if (sisteAntall == målsetning)
                {
                    gjeldende++;
                    sisteAntall = 0;

                    målsetning = i == 1 ? 2 : rekken[i - 1];
                }
            }
            return new Rekke(rekken);
        }
    }

    public class Rekke
    {
        public int[] Tallene;
        public int Sum => Tallene.Sum();

        public Rekke(int[] tallene)
        {
            Tallene = tallene;

        }

        public void Print()
        {
            Console.WriteLine(string.Join(", ", Tallene));
        }
    }

    [TestFixture]
    public class Luke5Tests
    {
        [Test]
        public void RekkenStarterMed1()
        {
            var r = new Luke5().Generer(1);

            Assert.AreEqual(1, r.Sum, "Sum");
            Assert.AreEqual(1, r.Tallene.SingleOrDefault(), "Sum");
        }

        [Test]
        public void PosisjonAngirAntall()
        {
            var r = new Luke5().Generer(3);

            Assert.AreEqual(5, r.Sum, "Sum");
            Assert.That(r.Tallene, Is.EquivalentTo(new[] { 1, 2, 2 }), "Tallene");
        }

        [Test]
        public void PosisjonAngirAntall6()
        {
            var r = new Luke5().Generer(6);
            r.Print();
            Assert.AreEqual(15, r.Sum, "Sum");
            Assert.That(r.Tallene, Is.EquivalentTo(new[] { 1, 2, 2, 3, 3, 4 }), "Tallene");
        }

        [Test]
        [Explicit]
        public void EnMillionSum()
        {
            var r = new Luke5().Generer(1000000);
            Console.WriteLine(r.Sum);
        }
    }
}