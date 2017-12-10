using System;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Code
{
    public class Luke10_Akevitten
    {
        public int FinnSisteMannNårAntall(int antall)
        {
            var plasseringer = new int[antall];

            var flaska = 0;
            while (true)
            {
                // finn første oppegående etter flaska
                var første = FørsteOppegåendeEtter(flaska, ref plasseringer);

                // hvis denne == flaska: ferdig
                if (første == flaska)
                    return flaska + 1;

                // denne går i gulvet
                plasseringer[første] = 1;

                // finn neste oppegående etter flaska
                var neste = FørsteOppegåendeEtter(flaska, ref plasseringer);

                // hvis denne == flaska: ferdig
                if (neste == flaska)
                    return flaska + 1;

                flaska = neste;

            }
        }
       
        private int FørsteOppegåendeEtter(int flaska, ref int[] plasseringer)
        {
            var sjekk = flaska + 1;
            while (true)
            {
                // roter
                if (sjekk == plasseringer.Length)
                    sjekk = 0;

                if (plasseringer[sjekk] == 0)
                    return sjekk;

                sjekk++;
            }
        }
    }

    [TestFixture]
    public class AkevittTests
    {
        [TestCase(3, 3)]
        [TestCase(6, 5)]
        public void Når6_SkalBli5(int antall, int forventetSiste)
        {
            var algoritme = new Luke10_Akevitten();

            var sistemann = algoritme.FinnSisteMannNårAntall(antall);
            Assert.AreEqual(forventetSiste, sistemann);
        }

        [Test]
        [Explicit]
        public void Konkurranse()
        {
            var algoritme = new Luke10_Akevitten();

            var sistemann = algoritme.FinnSisteMannNårAntall(1500);
            Console.WriteLine(sistemann);
        }
    }
}