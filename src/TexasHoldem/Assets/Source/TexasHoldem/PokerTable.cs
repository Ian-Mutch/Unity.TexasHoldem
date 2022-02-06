using CardGameEngine;
using Mirror;
using System.Collections.Generic;
using UnityEngine;

namespace TexasHoldem
{
    public class PokerTable : NetworkBehaviour
    {
        #region Properties

        public static PokerTable Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = FindObjectOfType<PokerTable>();
                    if(_instance == null)
                        throw new System.Exception($"Failed to find instance of {nameof(PokerTable)}");
                }

                return _instance;
            }
        }

        #endregion

        #region Fields

        private static PokerTable _instance;
        private List<THNetworkPlayer> _players;
        private Transform _transform;
        [SyncVar]
        private int _dealerIndex = -1;
        private Deck _deck;

        #endregion

        #region Unity Callbacks

        private void OnValidate()
        {
            var networkManager = NetworkManager.singleton as THNetworkManager;
            if (networkManager == null)
                return;

            var maxConnections = networkManager.maxConnections;
            var startPositionCount = GetComponentsInChildren<NetworkStartPosition>()?.Length ?? 0;

            if(maxConnections != startPositionCount)
                Debug.LogError($"The number of start positions parented to {nameof(PokerTable)} does not match the number of max connections assigned in the {nameof(THNetworkManager)}");
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.Log($"Another instance of {nameof(PokerTable)} already exists, destroying duplicate", this);
                Destroy(gameObject);
                return;
            }

            _instance = this;

            _transform = transform;
            
            SetupTable();
        }

        #endregion

        #region Methods

        public void AddPlayer(int playerIndex, THNetworkPlayer player)
        {
            if (_players[playerIndex] != null)
            {
                var assignedPlayer = _players[playerIndex];
                Debug.LogError($"Cannot assign player {player.name} to player slot {playerIndex} as a player {assignedPlayer.name} has already been assigned to it", this);
                return;
            }

            _players[playerIndex] = player;

            var playerTransform = (RectTransform)player.transform;
            var parent = _transform.GetChild(playerIndex);
            if (parent == null)
                Debug.LogError("No child object was found to parent the player to", this);
            else
                playerTransform.SetParent(parent);

            playerTransform.anchoredPosition = Vector3.zero;
            playerTransform.localScale = Vector3.one; // Stops an issue with scaling when parenting
        }

        public void RemovePlayer(int playerIndex)
        {
            if (_players[playerIndex] == null)
            {
                Debug.LogError($"Cannot remove player from player slot {playerIndex} as it is already emppty");
                return;
            }

            _players[playerIndex] = null;
        }

        private void SetupTable()
        {
            var playerCount = NetworkManager.singleton.maxConnections;
            _players = new List<THNetworkPlayer>(playerCount);
            for (int i = 0; i < playerCount; i++)
                _players.Add(null);
        }

        private void DealCards()
        {
            var startingIndex = _dealerIndex;
            for (var i = 0; i < 2; i++)
                for (var j = _dealerIndex; j < _dealerIndex + _players.Count; j++)
                {
                    var card = _deck.Pop();
                    var player = _players[j % _players.Count];

                    //TODO: Send card to player
                }

        }

        #endregion

        #region Network Methods

        [ServerCallback]
        public void StartRound()
        {
            _deck = new Deck();
            _dealerIndex = 0;

            OnRoundStarting();
            DealCards();
        }

        [ClientRpc]
        private void OnRoundStarting()
        {
            Debug.Log("Round started");
        }

        #endregion
    }
}