using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(RoomLauncher))]
public class RoomLauncherHUD : MonoBehaviour
{
    #region Fields

    [SerializeField, Tooltip("The animated icon to display while attempting to connect to the server")]
    private ConnectingIcon _connectingIcon;
    [SerializeField, Tooltip("A canvas group containing the cancel button as a child")]
    private CanvasGroup _connectingButtonsCanvasGroup;
    [SerializeField, Tooltip("A canvas group containing the retry and back buttons as children")]
    private CanvasGroup _failedToConnectButtonsCanvasGroup;
    [SerializeField]
    private Button _cancel;
    [SerializeField]
    private Button _retry;
    [SerializeField]
    private Button _back;
    [SerializeField, Scene, Tooltip("The scene to load when the back button is clicked")]
    private string _backScene;
    private RoomLauncher _launcher;

    #endregion

    #region Unity Callbacks

    private void OnValidate()
    {
        if (_cancel == null)
            Debug.LogError($"No cancel button has been assigned to {nameof(RoomLauncherHUD)}", this);

        if (_retry == null)
            Debug.LogError($"No cancel button has been assigned to {nameof(RoomLauncherHUD)}", this);

        if (_back == null)
            Debug.LogError($"No cancel button has been assigned to {nameof(RoomLauncherHUD)}", this);

        if (string.IsNullOrEmpty(_backScene))
            Debug.LogError($"No scene to load when the back button is clicked has been assigned to {nameof(RoomLauncherHUD)}", this);
    }

    private void OnEnable()
    {
        _launcher.OnStartSearch += RoomLauncher_OnStartSearch;
        _launcher.OnFailToConnect += RoomLauncher_OnFailToConnect;

        _cancel.onClick?.AddListener(OnClickCancel);
        _retry.onClick?.AddListener(OnClickRetry);
        _back.onClick?.AddListener(OnClickBack);
    }

    private void OnDisable()
    {
        _launcher.OnStartSearch -= RoomLauncher_OnStartSearch;
        _launcher.OnFailToConnect -= RoomLauncher_OnFailToConnect;

        _cancel.onClick?.RemoveListener(OnClickCancel);
        _retry.onClick?.RemoveListener(OnClickRetry);
        _back.onClick?.RemoveListener(OnClickBack);
    }

    private void Awake()
    {
        _launcher = GetComponent<RoomLauncher>();
    }

    #endregion

    #region Methods

    private void RoomLauncher_OnStartSearch()
    {
        SetButtonGroupConnectingDisplay(true);
        SetButtonGroupFailedToConnectDisplay(false);
        _connectingIcon?.Begin();
    }

    private void RoomLauncher_OnFailToConnect()
    {
        _connectingIcon?.End();
        SetButtonGroupConnectingDisplay(false);
        SetButtonGroupFailedToConnectDisplay(true);
    }

    private void SetButtonGroupConnectingDisplay(bool display)
    {
        if (_connectingButtonsCanvasGroup == null)
            return;

        _connectingButtonsCanvasGroup.alpha = display ? 1 : 0;
        _connectingButtonsCanvasGroup.interactable = display;
    }

    private void SetButtonGroupFailedToConnectDisplay(bool display)
    {
        if (_failedToConnectButtonsCanvasGroup == null)
            return;

        _failedToConnectButtonsCanvasGroup.alpha = display ? 1 : 0;
        _failedToConnectButtonsCanvasGroup.interactable = display;
    }

    private void OnClickCancel()
    {
        Debug.Log("Stopping search...");

        NetworkManager.singleton.StopClient();
    }

    private void OnClickRetry()
    {
        Debug.Log("Starting search...");

        RoomLauncher.Instance.Retry();
    }

    private void OnClickBack()
    {
        Debug.Log("Backing out of scene...");

        SceneManager.LoadScene(_backScene);
    }

    #endregion
}
