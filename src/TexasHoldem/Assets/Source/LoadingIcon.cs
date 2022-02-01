using System;
using UnityEngine;
using UnityEngine.UI;

public class LoadingIcon : MonoBehaviour
{
    [SerializeField] private Animation _animation;
    [SerializeField] private Image _refreshImage;
    [SerializeField] private CanvasGroup _buttonsCanvasGroup;
    [SerializeField] private Button _retry;
    [SerializeField] private Button _back;

    private void OnEnable()
    {
        RoomLauncher.OnStartSearch += RoomLauncher_OnStartSearch;
        RoomLauncher.OnFailToConnect += RoomLauncher_OnFailToConnect;

        _retry.onClick.AddListener(OnClickRetry);
        _back.onClick.AddListener(OnClickBack);
    }

    private void OnDisable()
    {
        RoomLauncher.OnStartSearch -= RoomLauncher_OnStartSearch;
        RoomLauncher.OnFailToConnect -= RoomLauncher_OnFailToConnect;

        _retry.onClick.RemoveListener(OnClickRetry);
        _back.onClick.RemoveListener(OnClickBack);
    }

    private void Reset()
    {
        _animation = GetComponent<Animation>();
        _refreshImage = GetComponent<Image>();
    }

    private void RoomLauncher_OnStartSearch()
    {
        if(_animation == null)
        {
            Debug.LogWarning($"No animation component has been attached to {nameof(LoadingIcon)}", this);
            return;
        }

        _buttonsCanvasGroup.alpha = 0;
        _buttonsCanvasGroup.interactable = false;

        _animation.Play();

        var color = _refreshImage.color;
        color.a = 1;
        _refreshImage.color = color;
    }

    private void RoomLauncher_OnFailToConnect()
    {
        _animation.Stop();
        var color = _refreshImage.color;
        color.a = 0;
        _refreshImage.color = color;

        _buttonsCanvasGroup.alpha = 1;
        _buttonsCanvasGroup.interactable = true;
    }

    private void OnClickRetry() => RoomLauncher.Instance.Retry();

    private void OnClickBack()
    {
        throw new NotImplementedException();
    }
}
