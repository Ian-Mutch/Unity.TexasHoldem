using Mirror;
using Networking;
using UnityEngine;

namespace TexasHoldem
{
    [RequireComponent(typeof(TelepathyTransport))]
    public class THNetworkManager : NetworkManagerBase
    {
        #region Methods

        protected override void OnPlayersReady()
        {
            base.OnPlayersReady();

            PokerTable.Instance.StartRound();
        }

        protected override NetworkPlayerBase GetPlayerToAdd(NetworkConnection conn, int playerIndex)
        {
            var player = Instantiate(playerPrefab);
            var networkPlayerComp = player.GetComponent<THNetworkPlayer>();
            networkPlayerComp.SetPlayerIndex(playerIndex);
            networkPlayerComp.SetDisplayName($"Player {playerIndex,2}"); // For testing until player display name functionality has been implemented
            networkPlayerComp.SetCash(10000); // For testing until player cash functionality has been implemented

            return networkPlayerComp;
        }

        #endregion
    }
}