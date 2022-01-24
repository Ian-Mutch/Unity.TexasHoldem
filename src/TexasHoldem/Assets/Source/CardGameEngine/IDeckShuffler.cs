using System.Collections.Generic;

namespace CardGameEngine
{
    public interface IDeckShuffler
    {
        IEnumerable<Card> Shuffle(IEnumerable<Card> cards);
    }
}
