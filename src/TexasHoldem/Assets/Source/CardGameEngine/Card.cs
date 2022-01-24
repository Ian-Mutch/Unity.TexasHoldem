namespace CardGameEngine
{
    public class Card
    {
        private const byte SuitCardShift = 3; // The amount to shift the suit card bits by to make room for the suit bits
        private const byte SuitMask = 7; // 00000111;

        public static readonly Card UnknownCard = new Card();

        /// <summary>
        ///     Bit packed integer containing suit and identifier. From right to left, the first 3 bits represent suit, the following 4 bits represent the identifier.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         00000000 represents an unknown card.
        ///         01011010 represents the <see cref="SuitCard.Jack"/> of <see cref="Suit.Diamonds"/>. 
        ///         From right to left, 010 is <see cref="Suit.Diamonds"/> and 1011 is <see cref="SuitCard.Jack"/>
        ///     </para>
        /// </remarks>
        public int Id => _id;
        public bool IsUnknown => _id == 0;

        private readonly int _id;

        /// <summary>
        ///     Creates an instance of a <see cref="Card"/> that is considered unknown i.e. a card that has not be identified/shown to players
        /// </summary>
        public Card()
        {
            _id = 0;
        }

        /// <summary>
        ///     Creates an instance of a <see cref="Card"/> that has a suit and identifier i.e a card that has been shown to players
        /// </summary>
        /// <param name="suit"></param>
        /// <param name="suitCard"></param>
        public Card(Suit suit, SuitCard suitCard)
        {
            _id = (int)suitCard << SuitCardShift;
            _id |= (int)suit;
        }

        public Suit GetSuit()
        {
            if (IsUnknown)
                throw new System.InvalidOperationException("Cannot get suit for an unknown card");

            var suit = _id & SuitMask; // Uses a mask when comparing bit sequences to set the bits to zero where both bits don't equal to 1.
                                       // The result of this operation is the suit where the first 3 bits are only relevant.
            return (Suit)suit;
        }

        public SuitCard GetSuitCard()
        {
            if (IsUnknown)
                throw new System.InvalidOperationException("Cannot get suit card for an unknown card");

            var suitCard = _id >> SuitCardShift; // Shifts bits to the right enough so that all is remaining is the suit card
            return (SuitCard)suitCard;
        }
    }
}