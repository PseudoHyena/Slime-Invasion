using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Class respondenting for player spawn and level restart
public class GameManager : MonoBehaviour {

    [SerializeField] Text status;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform water;

    public bool EndGame { get; private set; } = false;

    public static float GameBottomBorder { get; set; } = -10f;
    public static float GameFieldLength { get; set; }
    public static float WaterHeight { get; private set; }

    void Start() {
        WaterHeight = water.transform.position.y;
        GameFieldLength = LevelGenerator.singleton.Settings.size;
        LevelGenerator.singleton.OnEndOfLevelGeneration += SpawnPlayer;
    }

    public void GameOver() {
        EndGame = true;

        status.text = "GAVE OVER";
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
}
