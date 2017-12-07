using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace Code
{
    public class Rot13
    {
        public static Func<char, int> AsciiPlussPosisjon = (x) => (int)x + (int)x - 64;

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

        public static char Krypter(Func<char, int> shiftFunc, int i, char c)
        {
            var ny = (i - 64 + shiftFunc(c)) % 26;           
            var justert = 64 + ny;
            return (char)justert;
        }
    }

    [TestFixture]
    public class Rot13Tests
    {
        [Test]
        public void Krypter()
        {
            var kryptert = Rot13.Krypter(Rot13.AsciiPlussPosisjon, (int)'L', 'L');
            Assert.AreEqual('V', kryptert);
        }

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

            Assert.AreEqual(forventet, dekoder.Dekod(input, Rot13.AsciiPlussPosisjon));
        }

        [TestCase('A', 66)]
        [TestCase('B', 68)]
        [TestCase('Z', 90 + 26)]
        public void AsciiPlussPosisjon(char input, int forventet)
        {            
            var resultat = Rot13.AsciiPlussPosisjon(input);
            Assert.AreEqual(forventet, resultat);
        }

      

        [Test]
        public void DekodEksempel()
        {
            var dekoder = new Rot13();

            var dekodet = dekoder.Dekod("PWVAYOBB", Rot13.AsciiPlussPosisjon);

            Assert.AreEqual("JULEMANN", dekodet);
        }

        [Test]
        public void DekodKonkurranse()
        {
            var dekoder = new Rot13();

            var dekodet = dekoder.Dekod("OTUJNMQTYOQOVVNEOXQVAOXJEYA", Rot13.AsciiPlussPosisjon);
            Console.WriteLine(dekodet);
        }
    }
}