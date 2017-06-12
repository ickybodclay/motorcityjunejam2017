using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public void StartButtonClicked() {
        SceneManager.LoadScene(1);
    }

    public void ContinueButtonClicked(int index) {
        SceneManager.LoadScene(index);
    }

    public void QuitButtonClicked() {
        Application.Quit();
    }
}
