using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    [SerializeField] InputField seedInputField;

    public static string Seed { get; private set; }

	public void SetSeedFromInput() {
        Seed = seedInputField.text;
    }

    public void Quit() {
        Application.Quit();
    }
}
