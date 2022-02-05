using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private static MainMenu _instance;

    [SerializeField]
    private Button _play;
    [SerializeField, Scene]
    private string _playScene;
    [SerializeField]
    private Button _quit;

    private void OnValidate()
    {
        // A reminder to assign serializable properties
        if (_play == null)
            Debug.LogError($"No 'Play' button has been assigned to the {nameof(MainMenu)}", this);

        if (_quit == null)
            Debug.LogError($"No 'Quit' button has been assigned to the {nameof(MainMenu)}", this);
    }

    private void OnEnable()
    {
        _play?.onClick.AddListener(OnPlayButtonClicked);
        _quit?.onClick.AddListener(OnQuitButtonClicked);
    }

    private void OnDisable()
    {
        _play?.onClick.RemoveListener(OnPlayButtonClicked);
        _quit?.onClick.RemoveListener(OnQuitButtonClicked);
    }

    private void Awake()
    {
        // Destroy the gameobject if an existing instance of this component exists already
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    /// <summary>
    ///     Raises the <see cref="OnPlayClicked"/> event when the 'Play' button has been clicked
    /// </summary>
    private void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(_playScene);
    }

    /// <summary>
    ///     Quits the game when the 'Quit' button has been clicked.
    ///     
    ///     If called in editor, will exit play mode instead.
    /// </summary>
    private void OnQuitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
