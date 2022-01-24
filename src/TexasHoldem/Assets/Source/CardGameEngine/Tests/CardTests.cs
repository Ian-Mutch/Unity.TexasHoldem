using CardGameEngine;
using NUnit.Framework;
using System;

public class CardTests
{
    [Test]
    public void Initialise_Success()
    {
        // Arrange
        var suit = Suit.Clubs;
        var suitCard = SuitCard.Six;
        var expectedId = (int)suitCard << 3;
        expectedId |= (int)suit;

        // Act
        var card = new Card(suit, suitCard);

        // Assert
        Assert.IsNotNull(card);
        Assert.IsFalse(card.IsUnknown);
        Assert.AreEqual(expectedId, card.Id);
    }

    [Test]
    public void GetSuit_Success()
    {
        // Arrange
        var suit = Suit.Clubs;
        var card = new Card(suit, SuitCard.Six);

        // Act
        var result = card.GetSuit();

        // Assert
        Assert.AreEqual(suit, result);
    }

    [Test]
    public void GetSuit_ThrowsInvalidOperation()
    {
        // Arrange
        var card = Card.UnknownCard;

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => card.GetSuit());
    }

    [Test]
    public void GetSuitCard_Success()
    {
        // Arrange
        var suitCard = SuitCard.Six;
        var card = new Card(Suit.Clubs, suitCard);

        // Act
        var result = card.GetSuitCard();

        // Assert
        Assert.AreEqual(suitCard, result);
    }

    [Test]
    public void GetSuitCard_ThrowsInvalidOperation()
    {
        // Arrange
        var card = Card.UnknownCard;

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => card.GetSuitCard());
    }
}