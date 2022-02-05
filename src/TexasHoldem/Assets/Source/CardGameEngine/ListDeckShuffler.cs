using System;
using System.Collections.Generic;

namespace CardGameEngine
{
    public class ListDeckShuffler : IDeckShuffler
    {
        #region Methods

        /// <summary>
        ///     Shuffles a list of cards, return the same list
        /// </summary>
        /// <param name="cards"></param>
        /// <returns><see cref="List{T}"/> as an <see cref="IEnumerable{T}"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public IEnumerable<Card> Shuffle(IEnumerable<Card> cards)
        {
            if(cards == null)
                throw new ArgumentNullException(nameof(cards));

            if (!(cards is List<Card> list))
                throw new InvalidOperationException($"{nameof(cards)} is not a IEnumerable of type List");

            var rnd = new Random();
            for (int i = 0; i < list.Count; i++)
            {
                // Knuth shuffle implementation
                var temp = list[i];
                var k = i + rnd.Next(list.Count - i);
                list[i] = list[k];
                list[k] = temp;
            }

            return list;
        }

        #endregion
    }
}
