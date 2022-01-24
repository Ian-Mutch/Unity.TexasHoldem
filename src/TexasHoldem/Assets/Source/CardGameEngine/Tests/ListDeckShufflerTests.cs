using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGameEngine.Tests
{
    public class ListDeckShufflerTests
    {
        [Test]
        public void Initialise_Sucess()
        {
            // Arrange & Act
            var shuffler = new ListDeckShuffler();

            // Assert
            Assert.IsNotNull(shuffler);
        }

        [Test]
        public void Shuffle_Success()
        {
            // Arrange
            var cards = new List<Card>()
            {
                new Card(Suit.Spades, SuitCard.Ace),
                new Card(Suit.Spades, SuitCard.Two),
                new Card(Suit.Spades, SuitCard.Three),
                new Card(Suit.Spades, SuitCard.Four),
                new Card(Suit.Spades, SuitCard.Five),
                new Card(Suit.Spades, SuitCard.Six),
                new Card(Suit.Spades, SuitCard.Seven),
                new Card(Suit.Spades, SuitCard.Eight)
            };
            var cardCount = cards.Count;
            var shuffler = new ListDeckShuffler();

            // Act
            var shuffledCards = shuffler.Shuffle(cards);

            // Assert
            Assert.IsNotNull(cards);
            Assert.AreEqual(cards, shuffledCards);
            Assert.AreEqual(cardCount, shuffledCards.Count());

            var cardIndex = 0;
            foreach (var card in shuffledCards)
            {
                var origCard = cards[cardIndex];
                if(card.Id != origCard.Id)
                {
                    Assert.Pass();
                    break;
                }

                cardIndex++;
            }
        }

        [Test]
        public void Shuffle_ThrowsArgumentNull()
        {
            // Arrange
            var shuffle = new ListDeckShuffler();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => shuffle.Shuffle(null));
        }

        [Test]
        public void Shuffle_ThrowsInvalidOperation()
        {
            // Arrange
            var shuffle = new ListDeckShuffler();
            var cards = Array.Empty<Card>();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => shuffle.Shuffle(cards));
        }
    }
}
