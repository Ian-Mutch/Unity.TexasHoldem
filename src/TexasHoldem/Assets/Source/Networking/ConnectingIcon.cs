using Common.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class ConnectingIcon : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private Image _image;
    [SerializeField]
    private Animation _animation;

    #endregion

    #region Unity Callbacks

    private void OnValidate()
    {
        if(_image == null)
            Debug.LogError($"No image component has been attached to {nameof(ConnectingIcon)}", this);

        if (_animation == null)
            Debug.LogError($"No animation component has been attached to {nameof(ConnectingIcon)}", this);
    }

    private void Reset()
    {
        _image = GetComponent<Image>();
        _animation = GetComponent<Animation>();
    }

    #endregion

    #region Methods

    public void Begin()
    {
        _animation?.Play();
        SetImageColorAlpha(1);
    }

    public void End()
    {
        _animation?.Stop();
        SetImageColorAlpha(0);
    }

    private void SetAnimationPlayingState(bool playing)
    {
        if (_animation == null)
        {
            Debug.LogError($"No animation component has been attached to {nameof(ConnectingIcon)}", this);
            return;
        }

        if (playing)
            PlayAnimation();
        else
            StopAnimation();
    }

    private void PlayAnimation()
    {
        _animation?.Play();
    }

    private void StopAnimation()
    {
        _animation?.Stop();
    }

    private void SetImageColorAlpha(float alpha)
    {
        _image?.SetColorAlpha(alpha);
    }

    #endregion
}
