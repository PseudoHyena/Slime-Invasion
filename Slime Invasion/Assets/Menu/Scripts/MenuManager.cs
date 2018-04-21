using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    [SerializeField] InputField seedInputField;

    public static int Seed { get; private set; }

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
