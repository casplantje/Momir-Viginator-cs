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
            var cardTask = m_factory.makeRandomAsync(16);
            cardTask.Wait();
            var card = cardTask.Result;

            Assert.NotNull(card);
            Assert.That(card.power, Is.EqualTo("9"));
            Assert.That(card.defense, Is.EqualTo("9"));
            Assert.That(card.name, Is.EqualTo("Draco"));
            Assert.That(card.manaCost, Is.EqualTo("{16}"));
        }

        [Test]
        public void WhenGettingACardByName_ThenItIsReturnedCorrectly()
        {
            var cardTask = m_factory.makeByNameAsync("Balustrade Spy");
            cardTask.Wait();
            var card = cardTask.Result;

            Assert.NotNull(card);

            Assert.That(card.power, Is.EqualTo("2"));
            Assert.That(card.defense, Is.EqualTo("3"));
            Assert.That(card.name, Is.EqualTo("Balustrade Spy"));
            Assert.That(card.manaCost, Is.EqualTo("{3}{B}"));
        }

        [Test]
        public void WhenGettingADoubleFacedCard_ThenItIsReturnedCorrectly()
        {
            var cardTask = m_factory.makeByNameAsync("Graveyard Trespasser Graveyard Glutton");
            cardTask.Wait();
            var card = cardTask.Result;

            Assert.NotNull(card);
            Assert.NotNull(card.otherSide);
            Assert.That(card.power, Is.EqualTo("3"));
            Assert.That(card.defense, Is.EqualTo("3"));
            Assert.That(card.name, Is.EqualTo("Graveyard Trespasser"));
            Assert.That(card.manaCost, Is.EqualTo("{2}{B}"));

            Assert.That(card.otherSide.power, Is.EqualTo("4"));
            Assert.That(card.otherSide.defense, Is.EqualTo("4"));
            Assert.That(card.otherSide.name, Is.EqualTo("Graveyard Glutton"));
        }

        [Test]
        public void WhenGettingACreatureWithVariablePowerAndToughness_ThenItIsParsedCorrectly()
        {
            var cardTask = m_factory.makeByNameAsync("Yavimaya Kavu");
            cardTask.Wait();
            var card = cardTask.Result;

            Assert.NotNull(card);
            Assert.That(card.power, Is.EqualTo("*"));
            Assert.That(card.defense, Is.EqualTo("*"));
        }
    }
}
