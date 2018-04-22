using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour {

    [SerializeField] TMP_InputField seedInputField;

    public static int Seed { get; private set; }

    void Start() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SetSeedFromInput() {
        Seed = seedInputField.text.GetHashCode();
    }

    public void StartLevel() {
        SceneManager.LoadScene("GeneratedLevel");
    }

    public void Quit() {
        Application.Quit();
    }
}
