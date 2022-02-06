using Mirror;
using Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TexasHoldem
{
    public class THNetworkPlayer : NetworkPlayerBase
    {
        #region Fields

        [SerializeField, InspectorName("Background")]
        private Image _backgroundImage;
        [SerializeField, InspectorName("Display Name")]
        private TextMeshProUGUI _displayNameText;
        [SerializeField, InspectorName("Cash")]
        private TextMeshProUGUI _cashText;

        [SyncVar(hook = nameof(OnDisplayNameSet))]
        private string _displayName;
        [SyncVar(hook = nameof(OnCashChanged))]
        private uint _cash;

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
            if (_backgroundImage != null)
                _normalColor = _backgroundImage.color;
            else
                Debug.LogWarning("No background image has been assigned", this);
        }

        #endregion

        #region Networking Methods

        public override void OnStartClient()
        {
            base.OnStartClient();

            Debug.Log($"Adding player {name} to the poker table", this);

            PokerTable.Instance.AddPlayer(PlayerIndex, this);
        }

        public override void OnStopClient()
        {
            Debug.Log($"Removing player {name} from the poker table", this);

            PokerTable.Instance.RemovePlayer(PlayerIndex);

            base.OnStopClient();
        }

        [ServerCallback]
        public void SetDisplayName(string name) => _displayName = name;

        [ServerCallback]
        public void SetCash(uint cash) => _cash = cash;

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

        #endregion

        #region Methods

        protected override void OnReadyStatusChanged()
        {
            _backgroundImage.color = IsReady ? _readyColor : _normalColor;
        }

        #endregion
    }
}