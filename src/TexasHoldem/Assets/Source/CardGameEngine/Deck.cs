using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGameEngine
{
    public class Deck
    {
        #region Constants

        public const int MaxCardCount = 52;

        #endregion

        #region Properties

        public int Count => _cards.Count;

        #endregion

        #region Fields

        private List<Card> _cards;
        private IDeckShuffler _shuffler;

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates an instance of <see cref="Deck"/> populated with all 52 cards
        /// </summary>
        public Deck()
        {
            _cards = new List<Card>(52);
            PopulateDeck(Suit.Hearts);
            PopulateDeck(Suit.Diamonds);
            PopulateDeck(Suit.Clubs);
            PopulateDeck(Suit.Spades);
        }

        /// <summary>
        ///     Creates an instance of <see cref="Deck"/> with a implementation of <see cref="IDeckShuffler"/>
        /// </summary>
        /// <param name="shuffler"></param>
        public Deck(IDeckShuffler shuffler) : this()
        {
            _shuffler = shuffler;
        }

        #endregion

        #region Methods

        public void Shuffle()
        {
            if (_shuffler == null)
                _shuffler = new ListDeckShuffler();

            // The default implementation of a deck shuffler (see above) returns a list but if a custom implementation is given, check the returning type.
            var shuffledCards = _shuffler.Shuffle(_cards);
            if (shuffledCards is List<Card> list)
                _cards = list;
            else
                _cards = shuffledCards.ToList();
        }

        /// <summary>
        ///     Removes a <see cref="Card"/> from the top of the deck
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public Card Pop()
        {
            if (_cards.Count == 0)
                throw new InvalidOperationException("Cannot remove any more cards from the deck as it is empty");

            var card = _cards[_cards.Count - 1];
            _cards.RemoveAt(_cards.Count - 1);
            return card;
        }

        private void PopulateDeck(Suit suit)
        {
            // Each suit in a 52 card deck contains 13 cards
            for (int i = 1; i <= 13; i++)
                _cards.Add(new Card(suit, (Rank)i));
        }

        #endregion
    }
}