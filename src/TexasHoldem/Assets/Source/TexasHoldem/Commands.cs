using System;
using UnityEngine;
using UnityEngine.UI;

namespace TexasHoldem
{
    public class Commands : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Button _ready;

        #endregion

        #region Unity Callbacks

        private void OnEnable()
        {
            THNetworkPlayer.OnLocalPlayerSpawned += OnLocalPlayerSpawned;
            THNetworkPlayer.OnLocalPlayerDespawned += OnLocalPlayerDespawned;
        }

        private void OnDisable()
        {
            THNetworkPlayer.OnLocalPlayerSpawned -= OnLocalPlayerSpawned;
            THNetworkPlayer.OnLocalPlayerDespawned -= OnLocalPlayerDespawned;
        }

        #endregion

        #region Methods

        private void OnLocalPlayerSpawned()
        {
            RegisterEvents();
        }

        private void OnLocalPlayerDespawned()
        {
            UnregistedEvents();
        }

        private void RegisterEvents()
        {
            _ready.onClick.AddListener(OnClickReady);
        }

        private void UnregistedEvents()
        {
            _ready.onClick.RemoveListener(OnClickReady);
        }

        /// <summary>
        ///     Handles the <see cref="_ready"/> <see cref="Button.onClick"/> event.
        ///     
        ///     Sends a command to the server updating the ready status for the local player
        ///     and deactives the button.
        /// </summary>
        private void OnClickReady()
        {
            try
            {
                _ready.gameObject.SetActive(false); //Deactive to stop spamming server with update ready status calls, can be removed to enable 'unready' functionality
                var player = THNetworkPlayer.OwnedInstance;
                player.CmdChangeReadyState(!player.IsReady);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex, this);
                _ready.gameObject.SetActive(true); //Reactivate in case of exception for easier debugging.
                                                   //Incase of one time exception, will allow another attempt at readying up
            }
        }

        #endregion
    }
}