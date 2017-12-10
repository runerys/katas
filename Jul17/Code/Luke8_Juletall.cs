using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Code
{
    public class Luke8_Juletall
    {
        public long SumAlle()
        {
            return AlleJuletall().Sum();
        }

        public long[] AlleJuletall()
        {
            var juleTallHistory = new HashSet<long>();
            var juletall = new List<long>();

            for (var i = 0L; i < 10000001; i++)
            {
                var resultat = ErJuleTall(i, new HashSet<long>(), juleTallHistory);

                if (resultat == null)
                    break;

                if(resultat == true)
                    juletall.Add(i);
            }

            return juletall.ToArray();
            //return Range(0, 1000000)
            //        .Where(x => ErJuleTall(x, new HashSet<long>(), juleTallHistory))
            //        .ToArray();
        }

        public bool? ErJuleTall(long tall, HashSet<long> sequence, HashSet<long> juletallHistory)
        {
            if (juletallHistory.Contains(tall))
                return true; 

            var array = GetDigits(tall).Reverse();

            var squareSum = array.Select(x => x * x)
                                 .Sum();           

            if (squareSum > 10000000)
                return null; // no element should be bigger

            if (sequence.Contains(squareSum))
                return false; // break loop

            sequence.Add(squareSum);

            if (squareSum == 1)
            {
                juletallHistory.Add(tall);

                foreach (var x in sequence)
                {
                    juletallHistory.Add(x);
                }

                return true; // target
            }

            if (juletallHistory.Contains(squareSum))
                return true;

            return ErJuleTall(squareSum, sequence, juletallHistory);            
        }      

        private static IEnumerable<long> GetDigits(long source)
        {
            while (source > 0)
            {
                var digit = source % 10;
                source /= 10;
                yield return digit;
            }
        }

        //private static IEnumerable<long> Range(long start, long count)
        //{
        //    for (long current = 0; current < count; ++current)
        //        yield return start + current;
        //}
    }


    [TestFixture]
    public class JuleTallTests
    {
        [TestCase(13, true)]
        [TestCase(14, false)]
        [TestCase(8, false)]
        public void ErJuleTall(long number, bool expected)
        {
            var juletallfinner = new Luke8_Juletall();

            var erJuletall = juletallfinner.ErJuleTall(number, new HashSet<long>(), new HashSet<long>());

            Assert.AreEqual(expected, erJuletall);
        }       

        [Test]
        public void Konkurranse()
        {
            var juletallfinner = new Luke8_Juletall();
            Console.WriteLine(juletallfinner.SumAlle());
        }

        [Test]
        public void KonkurransePrint()
        {
            var juletallfinner = new Luke8_Juletall();
            Console.WriteLine(string.Join(", ", juletallfinner.AlleJuletall()));
        }
    }
}