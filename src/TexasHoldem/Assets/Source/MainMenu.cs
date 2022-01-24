using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    public static event Action OnPlayClicked;

    private static MainMenu _instance;

    [SerializeField]
    private UIDocument _uiDocument;
    private Button _play;
    private Button _quit;

    private void Reset()
    {
        _uiDocument = GetComponent<UIDocument>(); // Attempts to sets the UI document component if this script is attached to a object that has one
    }

    private void OnValidate()
    {
        // A reminder to attach a UI document component if one hasn't been assigned
        if (_uiDocument == null)
            Debug.LogError($"No UI document has been assigned to the {nameof(MainMenu)}", this);
    }

    private void OnEnable()
    {
        if (_uiDocument == null)
            throw new System.InvalidOperationException($"A UI document as no been assigned to {nameof(MainMenu)}");

        if (_play == null)
        {
            _play = _uiDocument.rootVisualElement.Q<Button>("Play"); // Queries the UI document for a Button named Play
            if (_play == null)
                Debug.LogError("Failed to get reference to a 'Play' button in UI document", this);
        }

        if(_quit == null)
        {
            _quit = _uiDocument.rootVisualElement.Q<Button>("Quit"); // Queries the UI document for a Button named Quit
            if (_quit == null)
                Debug.LogError("Failed to get reference to a 'Quit' button in UI document", this);
        }

        if(_play != null)
            _play.clicked += OnPlayButtonClicked;

        if(_quit != null)
            _quit.clicked += OnQuitButtonClicked;
    }

    private void OnDisable()
    {
        if (_play != null)
            _play.clicked -= OnPlayButtonClicked;

        if (_quit != null)
            _quit.clicked -= OnQuitButtonClicked;
    }

    private void Awake()
    {
        // Destroy the gameobject if an existing instance of this component exists already
        if(_instance != null && _instance != this)
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
        OnPlayClicked?.Invoke();
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
