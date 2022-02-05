using Mirror;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TexasHoldem
{
    public class THNetworkPlayer : NetworkBehaviour
    {
        #region Events

        public static event Action OnLocalPlayerSpawned;
        public static event Action OnLocalPlayerDespawned;

        #endregion

        #region Properties

        public static THNetworkPlayer OwnedInstance => _ownedInstance;
        public bool IsReady => _isReady;

        #endregion

        #region Fields

        [SerializeField, InspectorName("Background")]
        private Image _backgroundImage;
        [SerializeField, InspectorName("Display Name")]
        private TextMeshProUGUI _displayNameText;
        [SerializeField, InspectorName("Cash")]
        private TextMeshProUGUI _cashText;

        [SyncVar]
        private int _playerIndex;
        [SyncVar(hook = nameof(OnReadyStatusChanged))]
        private bool _isReady;
        [SyncVar(hook = nameof(OnDisplayNameSet))]
        private string _displayName;
        [SyncVar(hook = nameof(OnCashChanged))]
        private uint _cash;

        private static THNetworkPlayer _ownedInstance;
        [SerializeField]
        private Color _readyColor = Color.green;
        private Color _normalColor;

        #endregion

        #region Unity Callbacks

        private void Reset()
        {
            _backgroundImage = GetComponent<Image>();
        }

        private void Awake()
        {
            if(_backgroundImage != null)
                _normalColor = _backgroundImage.color;
            else
                Debug.LogWarning("No background image has been assigned", this);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Server only call sets the player index on the server. Will be synced to clients on spawn.
        /// </summary>
        /// <param name="playerIndex"></param>
        public void SetPlayerIndex(int playerIndex)
        {
            if(isClient)
            {
                Debug.Log($"Stopping to call {nameof(SetPlayerIndex)} on client which should only be called on server");
                return;
            }

            _playerIndex = playerIndex;
        }

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

            Debug.Log($"Adding player {name} to the poker table", this);

            PokerTable.Instance.AddPlayer(_playerIndex, this);
        }

        public override void OnStopClient()
        {
            if (isLocalPlayer)
            {
                _ownedInstance = null;
                OnLocalPlayerDespawned?.Invoke();
            }

            Debug.Log($"Removing player {name} from the poker table", this);

            PokerTable.Instance.RemovePlayer(_playerIndex);

            base.OnStopClient();
        }

        #endregion

        #region Networking Hooks

        /// <summary>
        ///     Called via hook on <see cref="_displayName"/> to update display name text component
        /// </summary>
        private void OnDisplayNameSet(string oldValue, string newValue)
        {
            Debug.Log($"Updating display name from {oldValue} to {newValue} for {name}", this);

            _displayNameText.text = newValue;
        }

        /// <summary>
        ///     Called via hook on <see cref="_cash"/> to update display name text component
        /// </summary>
        private void OnCashChanged(uint oldValue, uint newValue)
        {
            Debug.Log($"Updating cash from {oldValue} to {newValue} for {name}", this);

            _cashText.text = $"£{newValue}";
        }


        /// <summary>
        ///     Called via hook on <see cref="_isReady"/> up update background color to represent player ready status
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private void OnReadyStatusChanged(bool oldValue, bool newValue)
        {
            Debug.Log($"Updating ready status from {oldValue} to {newValue} for {name}", this);

            _backgroundImage.color = _isReady ? _readyColor : _normalColor;
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
            var networkManager = NetworkManager.singleton as THNetworkManager;
            if(networkManager == null)
            {
                Debug.LogWarning($"Failed to find instance of {nameof(THNetworkManager)}");
                return;
            }

            networkManager.UpdateReadyStatus();
        }

        #endregion
    }
}