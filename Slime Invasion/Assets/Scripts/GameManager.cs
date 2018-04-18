using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public bool EndGame { get; private set; } = false;

    [SerializeField] Text status;

    public void GameOver() {
        EndGame = true;

        status.text = "GAVE OVER";
        Invoke("Restart", 2f);
    }

    public void GameWin() {
        EndGame = true;

        status.text = "YOU WIN";
        Invoke("Restart", 2f);
    }

    void Restart() {
        SceneManager.LoadScene(0);
    } 
}
