using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public bool EndGame { get; private set; } = false;

    public static float GameBottomBorder { get; set; } = -10f;
    public static float GameFieldLength { get; set; }
    public static float WaterHeight { get; private set; }

    [SerializeField] Text status;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform water;

    void Start() {
        WaterHeight = water.transform.position.y;
        GameFieldLength = LevelGenerator.singleton.Settings.size;
        LevelGenerator.singleton.OnEndOfLevelGeneration += SpawnPlayer;
    }

    void Update() {
        CheckForBackToMenu();    
    }

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    } 

    public void SpawnPlayer() {
        GameObject player = Instantiate(playerPrefab, new Vector3(0f, 12f, 0f), Quaternion.identity);
        player.name = "Player";

        float[,] map = LevelGenerator.singleton.HeightMap;

        int maxY = 0;
        int maxX = 0;
        float maxHeight = float.MinValue;

        int size = map.GetLength(0);
        for (int y = 25; y < size; y++) {
            for (int x = 25; x < size; x++) {
                if (map[y, x] > maxHeight) {
                    maxY = y;
                    maxX = x;

                    maxHeight = map[y, x];
                }
            }
        }

        player.transform.position = new Vector3(maxX - size / 2, maxHeight + 1f,  size / 2 - maxY);
    }

    //Temporarily
    void CheckForBackToMenu() {
        if (Input.GetButton("Cancel")) {
            //SceneManager.LoadScene("Menu");
        }
    }
}
