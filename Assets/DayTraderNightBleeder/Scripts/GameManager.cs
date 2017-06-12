using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    public GameObject Player { get; private set; }
    public int EnemyCount { get; set; }

    [SerializeField] private GameObject dialogUi;
    [SerializeField] private Text dialogText;
    [SerializeField] public float dialogTypeSpeed = 0.01f;

    [SerializeField] private GameObject gameOverUi;

    private int currentLine = 0;
    private int endAtLine = 0;
    private string[] lines;

    private bool isTyping;
    private bool cancelTyping;

    private bool isGameOver = false;

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
        Player = GameObject.FindGameObjectWithTag("Player");
        Player.GetComponent<PlayerMotor>().ResetPlayer();

        if (dialogUi == null) {
            dialogUi = GameObject.Find("DialogPanel");
            dialogText = GameObject.Find("DialogText").GetComponent<Text>();
            CloseDialog();
        }

        if (gameOverUi == null) {
            gameOverUi = GameObject.Find("GameOverPanel");
        }
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

        if (isGameOver) {
            if (CrossPlatformInputManager.GetButtonDown("Submit")) {
                //ResetGame(); FIXME broken, temporarily disabled
                Application.Quit();
            }
        }
    }

    private void ResetGame() {
        isGameOver = false;
        isTyping = false;
        cancelTyping = false;
        dialogUi = null;
        dialogText = null;
        EnemyCount = 0;
        SceneManager.LoadScene("Game");
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

    public void ShowGameOver() {
        CloseDialog();

        gameOverUi.SetActive(true);

        isGameOver = true;
    }

    public void ShowWinScreen() {
        SceneManager.LoadScene(5);
    }
}
