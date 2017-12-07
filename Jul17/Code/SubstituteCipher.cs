using System;
using System.Linq;
using NUnit.Framework;

namespace Code
{
    public class SubstituteCipher
    {
        private const int AntallBokstaver = 'Z' - 'A' + 1;
        private const int Offset = 'Z' - AntallBokstaver;
        public static Func<int, int> AsciiPlussPosisjon = x => x + x - Offset;

        public string Dekod(string kodet, Func<int, int> shiftFunksjon)
        {
            var enigma = Enumerable.Range('A', AntallBokstaver)
                                   .Select(orig => new { Key = Krypter(orig, shiftFunksjon), Value = orig })
                                   .ToDictionary(v => (char)v.Key, v => (char)v.Value);
                
            return new string(kodet.ToCharArray().Select(x => enigma[x]).ToArray());
        }

        public static int Krypter(int bokstav, Func<int, int> shiftFunksjon)
        {
            return Offset + (bokstav - Offset + shiftFunksjon(bokstav)) % AntallBokstaver;           
        }
    }

    [TestFixture]
    public class Rot13Tests
    {
        [Test]
        public void Krypter()
        {
            var kryptert = SubstituteCipher.Krypter('L', SubstituteCipher.AsciiPlussPosisjon);
            Assert.AreEqual((int)'V', kryptert);
        }

        [TestCase("N", "A")]
        [TestCase("O", "B")]
        [TestCase("P", "C")]
        [TestCase("J", "W")]
        [TestCase("M", "Z")]
        public void Spesifikasjon(string input, string forventet)
        {
            var dekoder = new SubstituteCipher();

            Assert.AreEqual(forventet, dekoder.Dekod(input, x => 13));
        }

        [TestCase("B", "A")]
        [TestCase("A", "Z")]
        public void Spesifikasjon2(string input, string forventet)
        {
            var dekoder = new SubstituteCipher();

            Assert.AreEqual(forventet, dekoder.Dekod(input, x => 1));
        }

        [TestCase("E", "A")]
        public void Spesifikasjon30(string input, string forventet)
        {
            var dekoder = new SubstituteCipher();

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
            var dekoder = new SubstituteCipher();

            Assert.AreEqual(forventet, dekoder.Dekod(input, SubstituteCipher.AsciiPlussPosisjon));
        }

        [TestCase('A', 66)]
        [TestCase('B', 68)]
        [TestCase('Z', 90 + 26)]
        public void AsciiPlussPosisjon(char input, int forventet)
        {
            var resultat = SubstituteCipher.AsciiPlussPosisjon(input);
            Assert.AreEqual(forventet, resultat);
        }

        [Test]
        public void DekodEksempel()
        {
            var dekoder = new SubstituteCipher();

            var dekodet = dekoder.Dekod("PWVAYOBB", SubstituteCipher.AsciiPlussPosisjon);

            Assert.AreEqual("JULEMANN", dekodet);
        }

        [Test]
        public void DekodKonkurranse()
        {
            var dekoder = new SubstituteCipher();

            var dekodet = dekoder.Dekod("OTUJNMQTYOQOVVNEOXQVAOXJEYA", SubstituteCipher.AsciiPlussPosisjon);
            Console.WriteLine(dekodet);
        }
    }
}