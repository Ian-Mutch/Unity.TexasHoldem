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
        var rank = Rank.Six;
        var expectedId = (int)rank << 3;
        expectedId |= (int)suit;

        // Act
        var card = new Card(suit, rank);

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
        var card = new Card(suit, Rank.Six);

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
    public void GetRank_Success()
    {
        // Arrange
        var rank = Rank.Six;
        var card = new Card(Suit.Clubs, rank);

        // Act
        var result = card.GetRank();

        // Assert
        Assert.AreEqual(rank, result);
    }

    [Test]
    public void GetRank_ThrowsInvalidOperation()
    {
        // Arrange
        var card = Card.UnknownCard;

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => card.GetRank());
    }
}