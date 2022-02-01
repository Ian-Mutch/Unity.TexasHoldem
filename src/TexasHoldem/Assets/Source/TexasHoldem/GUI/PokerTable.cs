using Mirror;
using UnityEngine;

namespace TexasHoldem
{
    public class PokerTable : NetworkBehaviour
    {
        public static PokerTable Instance
        {
            get
            {
                if (_instance == null)
                    throw new System.InvalidOperationException($"No instance has been assigned for {nameof(PokerTable)}");

                return _instance;
            }
        }

        private static PokerTable _instance;
        private PlayerBehaviour[] _playerBehaviours;

        private void Awake()
        {
            if(_instance != null && _instance != this)
            {
                Debug.LogWarning($"A second instance of {nameof(PokerTable)} exists in the scene, destroying the gameobject", this);

                Destroy(gameObject);
                return;
            }

            _instance = this;
        }

        public void AddPlayer(PlayerBehaviour player)
        {

        }
    }
}