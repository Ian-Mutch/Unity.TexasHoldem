using Mirror;
using System;
using System.Collections;
using UnityEngine;

public class RoomLauncher : MonoBehaviour
{
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

    public static event Action OnStartSearch;
    public static event Action OnFailToConnect;

    private static RoomLauncher _instance;

    [SerializeField] private int _searchTimeout = 30;

    private Coroutine _coroutine;

    public void Retry()
    {
        if (NetworkClient.active)
            return;

        if(_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(GetEnumerator_Connect());
    }

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    private void Start()
    {
        _coroutine = StartCoroutine(GetEnumerator_Connect());
    }

    private IEnumerator GetEnumerator_Connect()
    {
        OnStartSearch?.Invoke();

        NetworkManager.singleton.StartClient();

        var t = 0f;
        yield return new WaitUntil(() =>
        {
            if (NetworkClient.isConnected)
                return true;

            t += Time.deltaTime;
            return t >= _searchTimeout;
        });

        if (!NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopClient();
            OnFailToConnect?.Invoke();
        }
    }
}
