using Mirror;
using System.Collections.Generic;
using UnityEngine;

namespace Networking
{
    public abstract class NetworkManagerBase : NetworkManager
    {
        #region Fields

        [SerializeField, Min(2)]
        private int _minPlayers = 2;
        private Dictionary<int, NetworkPlayerBase> _players;
        private bool _allPlayersReady;

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

            _players = new Dictionary<int, NetworkPlayerBase>();
            for (int i = 0; i < maxConnections; i++)
                _players.Add(i, null);
        }

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            foreach (var entry in _players)
            {
                if (entry.Value == null)
                {
                    var player = GetPlayerToAdd(conn, entry.Key);
                    _players[entry.Key] = player;

                    // instantiating a "Player" prefab gives it the name "Player(clone)"
                    // => appending the connectionId is WAY more useful for debugging!
                    player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";

                    NetworkServer.AddPlayerForConnection(conn, player.gameObject);
                    return;
                }
            }

            Debug.Log("Failed to add additional player as the maximum player count has been reached", this);
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            var networkPlayer = conn.identity.GetComponent<NetworkPlayerBase>();
            if (networkPlayer == null)
                Debug.LogWarning($"No {(nameof(NetworkPlayerBase))} was found on disconnecting client {conn.connectionId}");
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

        [ServerCallback]
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

                OnPlayersReady();
            }
            else
                _allPlayersReady = false;
        }

        protected abstract NetworkPlayerBase GetPlayerToAdd(NetworkConnection conn, int playerIndex);

        protected virtual void OnPlayersReady()
        {
            Debug.Log("All players are ready");
        }

        #endregion
    }
}