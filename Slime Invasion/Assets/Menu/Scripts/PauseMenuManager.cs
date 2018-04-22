using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour {

    [SerializeField] GameObject pauseManu;
    public static bool IsGamePaused { get; private set; } = false;

    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            if (IsGamePaused) {
                Resume();
            }
            else {
                Pause();
            }
        }
    }

    public void Resume() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pauseManu.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;
    }

    void Pause() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        pauseManu.SetActive(true);
        Time.timeScale = 0f;
        IsGamePaused = true;
    }

    public void BackToMenu() {
        SceneManager.LoadScene("Menu");
    }
}
