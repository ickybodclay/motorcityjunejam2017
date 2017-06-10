using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    //private GameObject m_Canvas;
    public GameObject Player { get; private set; }

    [SerializeField] private GameObject dialogUi;
    [SerializeField] private Text dialogText;
    [SerializeField] public float dialogTypeSpeed = 0.01f;

    private int currentLine = 0;
    private int endAtLine = 0;
    private string[] lines;

    private bool isTyping;
    private bool cancelTyping;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        InitGame();
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        Debug.Log("Level Loaded");
        Debug.Log(scene.name);
        Debug.Log(mode);

        if (this != Instance) return;

        InitGame();
    }

    private void InitGame() {
        //m_Canvas = GameObject.Find("Canvas");
        Player = GameObject.Find("Player");
    }

    private void Update() {
        if (IsDialogShowing()) {
            if (CrossPlatformInputManager.GetButtonDown("Submit")) {
                if (!isTyping) {
                    currentLine++;
                    if (currentLine > endAtLine) {
                        CloseDialog();
                    }
                    else {
                        StartCoroutine(TextScroll(lines[currentLine]));
                    }
                }
                else if (isTyping && !cancelTyping) {
                    cancelTyping = true;
                }
            }
        }
    }

    public void ShowDialog(params string[] lines) {
        if (lines.Length > 0) {
            dialogUi.SetActive(true);
            this.lines = lines;
            currentLine = 0;
            endAtLine = lines.Length - 1;

            StartCoroutine(TextScroll(lines[currentLine]));
        }
    }

    public void CloseDialog() {
        dialogUi.SetActive(false);
        lines = null;
    }

    private IEnumerator TextScroll(string text) {
        int letter = 0;
        dialogText.text = "";
        isTyping = true;
        cancelTyping = false;
        while (isTyping && !cancelTyping && letter < text.Length - 1) {
            dialogText.text += text[letter];
            letter++;
            yield return new WaitForSeconds(dialogTypeSpeed);
        }
        dialogText.text = text;
        isTyping = false;
        cancelTyping = false;
    }

    public bool IsDialogShowing() {
        return dialogUi.activeSelf;
    }
}
