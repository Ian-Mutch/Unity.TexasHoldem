using Mirror;
using System.Collections.Generic;
using UnityEngine;

namespace TexasHoldem
{
    [RequireComponent(typeof(TelepathyTransport))]
    public class THNetworkManager : NetworkManager
    {
        #region Fields

        [SerializeField, Min(2)]
        private int _minPlayers = 2;
        private Dictionary<int, THNetworkPlayer> _players;
        private bool _allPlayersReady;
        private int _dealerIndex = -1;

        #endregion

        #region Unity Callbacks

        public override void OnValidate()
        {
            base.OnValidate();

            maxConnections = Mathf.Max(_minPlayers, maxConnections);
        }

        #endregion

        #region Networking Methods

        public override void OnStartServer()
        {
            base.OnStartServer();

            _players = new Dictionary<int, THNetworkPlayer>();
            for (int i = 0; i < maxConnections; i++)
                _players.Add(i, null);
        }

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            foreach (var entry in _players)
            {
                if (entry.Value == null)
                {
                    OnTHServerAddPlayer(conn, entry.Key);
                    return;
                }
            }

            Debug.Log("Failed to add additional player as the maximum player count has been reached", this);
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            var networkPlayer = conn.identity.GetComponent<THNetworkPlayer>();
            if (networkPlayer == null)
                Debug.LogWarning($"No {(nameof(THNetworkPlayer))} was found on disconnecting client {conn.connectionId}");
            else
            {
                foreach (var entry in _players)
                {
                    if (entry.Value == networkPlayer)
                    {
                        _players[entry.Key] = null;
                        break;
                    }
                }
            }

            base.OnServerDisconnect(conn);
        }

        #endregion

        #region Methods

        public void UpdateReadyStatus()
        {
            int currentPlayers = 0;
            int readyPlayers = 0;

            foreach (var item in _players)
            {
                var player = item.Value;
                if (player != null)
                {
                    currentPlayers++;
                    if (player.IsReady)
                        readyPlayers++;
                }
            }

            if (currentPlayers == readyPlayers && readyPlayers > _minPlayers)
            {
                _allPlayersReady = true;

                //TODO: Start Round
            }
            else
                _allPlayersReady = false;
        }

        private void OnTHServerAddPlayer(NetworkConnection conn, int playerIndex)
        {
            var startPos = GetStartPosition();
            if (startPos == null)
                throw new System.Exception("No start position was found");

            var player = Instantiate(playerPrefab, startPos);
            var networkPlayerComp = player.GetComponent<THNetworkPlayer>();
            networkPlayerComp.SetPlayerIndex(playerIndex);

            _players[playerIndex] = networkPlayerComp;

            // instantiating a "Player" prefab gives it the name "Player(clone)"
            // => appending the connectionId is WAY more useful for debugging!
            player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";

            NetworkServer.AddPlayerForConnection(conn, player);
        }

        #endregion
    }
}