using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace Code
{
    public class Rot13
    {
        
        public string Dekod(string kodet, Func<char, int> rootFunc)
        {
            Dictionary<char, char> oversettelser = new Dictionary<char, char>();

            for (int i = 65; i <= 90; i++)
            {
                var c = (char) i;
                var justert = Krypter(rootFunc, i, c);
                oversettelser.Add((char)justert, c);
            }

            return new string(kodet.ToCharArray().Select(x => oversettelser[x]).ToArray());
        }

        public static char Krypter(Func<char, int> rootFunc, int i, char c)
        {
            var flyttet = i - 64 + rootFunc(c);
            var justert = flyttet % (90 - 64) + 64;
            return (char)justert;
        }
    }

    [TestFixture]
    public class Rot13Tests
    {       
        [TestCase("N", "A")]
        [TestCase("O", "B")]
        [TestCase("P", "C")]
        [TestCase("J", "W")]
        [TestCase("M", "Z")]
        public void Spesifikasjon(string input, string forventet)
        {
            var dekoder = new Rot13();

            Assert.AreEqual(forventet, dekoder.Dekod(input, x => 13));
        }

        [TestCase("B", "A")]        
        [TestCase("A", "Z")]        
        public void Spesifikasjon2(string input, string forventet)
        {
            var dekoder = new Rot13();

            Assert.AreEqual(forventet, dekoder.Dekod(input, x => 1));
        }

        [TestCase("E", "A")]        
        public void Spesifikasjon30(string input, string forventet)
        {
            var dekoder = new Rot13();

            Assert.AreEqual(forventet, dekoder.Dekod(input, x => 30));
        }

        [TestCase("P", "J")]
        [TestCase("W", "U")]
        [TestCase("V", "L")]
        [TestCase("A", "E")]
        [TestCase("Y", "M")]
        [TestCase("O", "A")]
        [TestCase("B", "N")]
        public void SpesifikasjonEksepmel(string input, string forventet)
        {
            var dekoder = new Rot13();

            Assert.AreEqual(forventet, dekoder.Dekod(input, x => (int)x + 66 - (int)x));
        }

        [Test]
        public void RotFunksjon()
        {
            Func<char, int> f = (x) => (int) x + 66 - (int) x;

            var resultat = f('A');
            Assert.AreEqual(65 + 1, resultat);
        }

        [Test]
        public void Krypter()
        {          
            var kryptert = Rot13.Krypter(x => (int) x + 66 - (int) x, (int) 'L', 'L');
            Assert.AreEqual('V', kryptert);
        }

        [Test]
        public void DekodEksempel()
        {
            var dekoder = new Rot13();

            var dekodet = dekoder.Dekod("PWVAYOBB", x => (int)x + 66 - (int)x);

            Assert.AreEqual("JULEMANN", dekodet);
        }

        [Test]
        public void DekodKonkurranse()
        {
            var dekoder = new Rot13();

            var dekodet = dekoder.Dekod("OTUJNMQTYOQOVVNEOXQVAOXJEYA", x => (int)x + 66 - (int)x);
            Console.WriteLine(dekodet);
        }
    }
}