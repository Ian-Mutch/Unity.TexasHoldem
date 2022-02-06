using Mirror;
using System;
using UnityEngine;

namespace Networking
{
    public abstract class NetworkPlayerBase : NetworkBehaviour
    {
        #region Events

        public static event Action OnLocalPlayerSpawned;
        public static event Action OnLocalPlayerDespawned;

        #endregion

        #region Properties

        public static NetworkPlayerBase OwnedInstance => _ownedInstance;
        public int PlayerIndex => _playerIndex;
        public bool IsReady => _isReady;

        #endregion

        #region Fields

        private static NetworkPlayerBase _ownedInstance;

        [SyncVar]
        private int _playerIndex;
        [SyncVar(hook = nameof(OnReadyStatusChanged))]
        private bool _isReady;

        #endregion

        #region Networking Methods

        public override void OnStartClient()
        {
            base.OnStartClient();

            if (isLocalPlayer)
            {
                _ownedInstance = this;
                OnLocalPlayerSpawned?.Invoke();
            }
        }

        public override void OnStopClient()
        {
            base.OnStopClient();

            if (isLocalPlayer)
            {
                _ownedInstance = null;
                OnLocalPlayerDespawned?.Invoke();
            }
        }

        [ServerCallback]
        public void SetPlayerIndex(int playerIndex) => _playerIndex = playerIndex;

        #endregion

        #region Networking Hooks

        /// <summary>
        ///     Called via hook on <see cref="_isReady"/>
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private void OnReadyStatusChanged(bool oldValue, bool newValue)
        {
            Debug.Log($"Updating ready status from {oldValue} to {newValue} for {name}", this);

            OnReadyStatusChanged();
        }

        #endregion

        #region Networking Commands

        /// <summary>
        ///     Call to server to change ready state of player
        /// </summary>
        /// <param name="isReady"></param>
        [Command]
        public void CmdChangeReadyState(bool isReady)
        {
            _isReady = isReady;
            var networkManager = NetworkManager.singleton as NetworkManagerBase;
            if (networkManager == null)
            {
                Debug.LogWarning($"Failed to find instance of {nameof(NetworkManagerBase)}");
                return;
            }

            networkManager.UpdateReadyStatus();
        }

        #endregion

        #region Methods

        protected virtual void OnReadyStatusChanged()
        {

        }

        #endregion
    }
}