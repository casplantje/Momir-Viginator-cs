using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Momir_Viginator_cs;

namespace Momir_Viginator_Tests
{
    [TestFixture]
    public class UnitTests_OnlineScryfallFactory
    {
        private OnlineScryfallFactory m_factory;
        public UnitTests_OnlineScryfallFactory()
        {
            m_factory = new OnlineScryfallFactory();
        }

        [Test]
        public void WhenGettingARandomCreatureWithCMC16_ThenItIsDraco()
        {
            var card = m_factory.makeRandom(16);

            Assert.NotNull(card);

            Assert.AreEqual(9, card.power);
            Assert.AreEqual(9, card.defense);
            Assert.AreEqual("Draco", card.name);
            Assert.AreEqual("{16}", card.manaCost);
        }

        [Test]
        public void WhenGettingACardByName_ThenItIsReturnedCorrectly()
        {
            var card = m_factory.makeByName("Balustrade Spy");

            Assert.NotNull(card);

            Assert.AreEqual(card.power, 2);
            Assert.AreEqual(card.defense, 3);
            Assert.AreEqual(card.name, "Balustrade Spy");
            Assert.AreEqual(card.manaCost, "{3}{B}");
        }

        [Test]
        public void WhenGettingADoubleFacedCard_ThenItIsReturnedCorrectly()
        {
            var card = m_factory.makeByName("Graveyard Trespasser Graveyard Glutton");

            Assert.NotNull(card);
            Assert.NotNull(card.otherSide);
            Assert.AreEqual(card.name, "Graveyard Trespasser");
            Assert.AreEqual(card.power, 3);
            Assert.AreEqual(card.defense, 3);
            Assert.AreEqual(card.manaCost, "{2}{B}");

            Assert.AreEqual(card.otherSide.name, "Graveyard Glutton");
            Assert.AreEqual(card.otherSide.power, 4);
            Assert.AreEqual(card.otherSide.defense, 4);
        }
    }
}
