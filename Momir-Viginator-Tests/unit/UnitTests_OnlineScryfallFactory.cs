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

            Assert.AreEqual("9", card.power);
            Assert.AreEqual("9", card.defense);
            Assert.AreEqual("Draco", card.name);
            Assert.AreEqual("{16}", card.manaCost);
        }

        [Test]
        public void WhenGettingACardByName_ThenItIsReturnedCorrectly()
        {
            var card = m_factory.makeByName("Balustrade Spy");

            Assert.NotNull(card);

            Assert.AreEqual("2", card.power);
            Assert.AreEqual("3", card.defense);
            Assert.AreEqual("Balustrade Spy", card.name);
            Assert.AreEqual("{3}{B}", card.manaCost);
        }

        [Test]
        public void WhenGettingADoubleFacedCard_ThenItIsReturnedCorrectly()
        {
            var card = m_factory.makeByName("Graveyard Trespasser Graveyard Glutton");

            Assert.NotNull(card);
            Assert.NotNull(card.otherSide);
            Assert.AreEqual("Graveyard Trespasser", card.name);
            Assert.AreEqual("3", card.power);
            Assert.AreEqual("3", card.defense);
            Assert.AreEqual("{2}{B}", card.manaCost);

            Assert.AreEqual("Graveyard Glutton", card.otherSide.name);
            Assert.AreEqual("4", card.otherSide.power);
            Assert.AreEqual("4", card.otherSide.defense);
        }

        [Test]
        public void WhenGettingACreatureWithVariablePowerAndToughness_ThenItIsParsedCorrectly()
        {
            var card = m_factory.makeByName("Yavimaya Kavu");

            Assert.NotNull(card);
            Assert.AreEqual("*", card.power);
            Assert.AreEqual("*", card.defense);
        }
    }
}
