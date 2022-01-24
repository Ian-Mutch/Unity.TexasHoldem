using CardGameEngine;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

public class DeckTests
{
    [Test]
    public void Initialise_Sucess()
    {
        // Arrange & Act
        var deck = new Deck();

        // Assert
        Assert.IsNotNull(deck);
        Assert.AreEqual(Deck.MaxCardCount, deck.Count);
        AssertContainsUniqueCards(deck);
    }

    [Test]
    public void Initialise_WithShuffler_Success()
    {
        // Arrange
        var shuffler = new ListDeckShuffler();

        // Act
        var deck = new Deck(shuffler);

        // Assert
        Assert.IsNotNull(deck);
        Assert.AreEqual(Deck.MaxCardCount, deck.Count);
        AssertContainsUniqueCards(deck);
    }

    [Test]
    public void Pop_Success()
    {
        // Arrange
        var deck = new Deck();

        // Act
        var card = deck.Pop();

        // Assert
        Assert.IsNotNull(card);
    }

    [Test]
    public void Pop_ThrowsInvalidOperation()
    {
        // Arrange
        var deck = new Deck();
        while(deck.Count > 0)
            deck.Pop();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => deck.Pop());
    }

    [Test]
    public void Shuffle_Success()
    {
        // Arrange
        var deckA = new Deck();
        var deckB = new Deck();

        // Act
        deckB.Shuffle();

        // Assert
        Assert.AreEqual(deckA.Count, deckB.Count);
        for (int i = 0; i < deckA.Count; i++)
        {
            var cardA = deckA.Pop();
            var cardB = deckB.Pop();

            if(cardA.Id != cardB.Id)
            {
                Assert.Pass();
                break;
            }
        }
    }

    private void AssertContainsUniqueCards(Deck deck)
    {
        var cards = new List<int>();
        while (deck.Count > 0)
        {
            var card = deck.Pop();
            if (cards.Any(x => x == card.Id))
                Assert.Fail("Cards in deck are not unique");

            cards.Add(card.Id);
        }
    }
}
