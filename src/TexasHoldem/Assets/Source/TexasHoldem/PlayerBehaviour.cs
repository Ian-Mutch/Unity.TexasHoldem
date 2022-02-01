using Mirror;
using TMPro;
using UnityEngine;

namespace TexasHoldem
{
    public class PlayerBehaviour : NetworkBehaviour
    {
        [SerializeField, InspectorName("Display Name")]
        private TextMeshProUGUI _displayNameText;
        [SerializeField, InspectorName("Cash")]
        private TextMeshProUGUI _cashText;

        [SyncVar(hook = "OnDisplayNameSet")]
        private string _displayName;
        [SyncVar(hook = "OnCashChanged")]
        private uint _cash;

        /// <summary>
        ///     Called via hook on <see cref="_displayName"/> to update display name text component
        /// </summary>
        private void OnDisplayNameSet(string oldValue, string newValue) => _displayNameText.text = newValue;

        /// <summary>
        ///     Called via hook on <see cref="_cash"/> to update display name text component
        /// </summary>
        private void OnCashChanged(uint oldValue, uint newValue) => _cashText.text = $"£{newValue}";
    }
}