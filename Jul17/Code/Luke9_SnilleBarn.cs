using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Code
{
    public class Luke9_SnilleBarn
    {
        public static int[] FinnAntallSummer(int tall)
        {           
            var antallSummerForTall = new int[tall + 1];

            var sum1TilN = new long[tall + 1];
            long sum = 0;
            for (int i = 1; i <= tall; i++)
            {
                sum += 2 * i;
                sum1TilN[i] = sum;
            }

            var limit = tall / 2 + 1; // trenger bare sjekke opp til halvparten av tallet
            for (int i = 2; i <= limit; i++)
            {
                for (int j = 0; j <= i - 2; j++)
                {
                    var t = (sum1TilN[i] - sum1TilN[j]) / 2;

                    if (t > tall)
                        continue;

                    antallSummerForTall[t]++;
                }
            }

            return antallSummerForTall;
        }
    }

    [TestFixture]
    public class SnilleBarnTests
    {      
        [Test]
        public void FinnSummer10()
        {
            var summer = Luke9_SnilleBarn.FinnAntallSummer(10);

            Assert.That(summer, Is.EquivalentTo(new[] { 0, 0, 0, 1, 0, 1, 1, 1, 0, 2, 1 }));
        }

        [Test]
        [Explicit]
        public void Print10()
        {
            var summer = Luke9_SnilleBarn.FinnAntallSummer(10);

            Console.WriteLine(string.Join(", ", summer));
        }

        [Test]
        [Explicit]
        public void Konkurranse()
        {
            var summer = Luke9_SnilleBarn.FinnAntallSummer(130000);

            Console.WriteLine(summer.Sum());
        }
    }
}