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

    private void Reset()
    {
        _image = GetComponent<Image>();
        _animation = GetComponent<Animation>();
    }

    #endregion

    #region Methods

    public void Begin()
    {
        if (_animation == null)
            Debug.LogWarning($"No animation component has been attached to {nameof(RoomLauncherHUD)}", this);

        _animation.Play();
        SetColorAlpha(1);
    }

    public void End()
    {
        _animation.Stop();
        SetColorAlpha(0);
    }

    private void SetAnimationPlayingState(bool playing)
    {
        if (_animation == null)
        {
            Debug.LogError($"No animation component has been attached to {nameof(RoomLauncherHUD)}", this);
            return;
        }

        if (playing)
            PlayAnimation();
        else
            StopAnimation();
    }

    private void PlayAnimation()
    {
        if (_animation.isPlaying)
            Debug.LogWarning($"Redundant call to start playing animation for {nameof(ConnectingIcon)}. The animation is already being played", this);
        else
            _animation.Play();
    }

    private void StopAnimation()
    {
        if (!_animation.isPlaying)
            Debug.LogWarning($"Redundant call to stop playing animation for {nameof(ConnectingIcon)}. No animation is being played", this);
        else
            _animation.Stop();
    }

    private void SetColorAlpha(float alpha)
    {
        if (_animation == null)
            Debug.LogWarning($"No animation component has been attached to {nameof(RoomLauncherHUD)}", this);

        _image.SetColorAlpha(alpha);
    }

    #endregion
}
