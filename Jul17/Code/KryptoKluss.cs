using System;
using System.Linq;
using NUnit.Framework;

namespace Code
{
    /// <summary>
    /// https://julekalender.knowit.no/challenges/cjatouw266ynt0103dxxo04jy
    /// </summary>
    public class KryptoKluss
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
        public void KrypterMedAsciiPlussPosisjon()
        {
            var kryptert = KryptoKluss.Krypter('L', KryptoKluss.AsciiPlussPosisjon);
            Assert.AreEqual((int)'V', kryptert);
        }       

        [TestCase("B", "A")]
        [TestCase("A", "Z")]
        public void ShiftMed1(string input, string forventet)
        {
            var dekoder = new KryptoKluss();

            Assert.AreEqual(forventet, dekoder.Dekod(input, x => 1));
        }

        [TestCase("E", "A")]
        public void ShiftMed30(string input, string forventet)
        {
            var dekoder = new KryptoKluss();

            Assert.AreEqual(forventet, dekoder.Dekod(input, x => 30));
        }

        [TestCase("P", "J")]
        [TestCase("W", "U")]
        [TestCase("V", "L")]
        [TestCase("A", "E")]
        [TestCase("Y", "M")]
        [TestCase("O", "A")]
        [TestCase("B", "N")]
        public void SpesifikasjonEksempel(string input, string forventet)
        {
            var dekoder = new KryptoKluss();

            Assert.AreEqual(forventet, dekoder.Dekod(input, KryptoKluss.AsciiPlussPosisjon));
        }

        [TestCase('A', 66)]
        [TestCase('B', 68)]
        [TestCase('Z', 90 + 26)]
        public void AsciiPlussPosisjon(char input, int forventet)
        {
            var resultat = KryptoKluss.AsciiPlussPosisjon(input);
            Assert.AreEqual(forventet, resultat);
        }

        [Test]
        public void DekodEksempel()
        {
            var dekoder = new KryptoKluss();

            var dekodet = dekoder.Dekod("PWVAYOBB", KryptoKluss.AsciiPlussPosisjon);

            Assert.AreEqual("JULEMANN", dekodet);
        }

        [Test]
        public void DekodKonkurranse()
        {
            var dekoder = new KryptoKluss();

            var dekodet = dekoder.Dekod("OTUJNMQTYOQOVVNEOXQVAOXJEYA", KryptoKluss.AsciiPlussPosisjon);
            Console.WriteLine(dekodet);
        }
    }
}