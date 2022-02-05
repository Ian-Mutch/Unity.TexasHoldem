using Mirror;
using System;
using System.Collections;
using UnityEngine;

public class RoomLauncher : MonoBehaviour
{
    #region Events

    public event Action OnStartSearch;
    public event Action OnFailToConnect;

    #endregion

    #region Properties

    public static RoomLauncher Instance
    {
        get
        {
            if(_instance == null)
                _instance = FindObjectOfType<RoomLauncher>();

            if (_instance == null)
                throw new Exception($"No instance of {nameof(RoomLauncher)} could be found");

            return _instance;
        }
    }

    #endregion

    #region Fields

    private static RoomLauncher _instance;

    [SerializeField, Tooltip("The maximum amount of time to search for a game (in seconds)")] 
    private int _searchTimeout = 30;
    private float _elapsedSearchTime; // in seconds
    private WaitUntil _searchTimeoutFunc;
    private Coroutine _connectCoroutine;

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        _searchTimeoutFunc = new WaitUntil(IsConnectingOrTimeOut);
    }

    private void Start()
    {
        Connect();
    }

    #endregion

    #region Methods

    public void Retry()
    {
        if (NetworkClient.active)
            return;

        Connect();
    }

    private void Connect()
    {
        if (NetworkClient.active)
            return;

        if(_connectCoroutine != null)
            StopCoroutine(_connectCoroutine);

        _connectCoroutine = StartCoroutine(GetEnumerator_Connect());
    }

    private IEnumerator GetEnumerator_Connect()
    {
        OnStartSearch?.Invoke();

        NetworkManager.singleton.StartClient();

        _elapsedSearchTime = 0;
        yield return _searchTimeoutFunc;

        if (!NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopClient();
            OnFailToConnect?.Invoke();
        }
    }

    private bool IsConnectingOrTimeOut()
    {
        if (!NetworkClient.active)
            return true;

        _elapsedSearchTime += Time.deltaTime;
        return _elapsedSearchTime >= _searchTimeout;
    }

    #endregion
}
