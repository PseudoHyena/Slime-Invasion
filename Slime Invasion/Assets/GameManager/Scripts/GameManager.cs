﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public bool EndGame { get; private set; } = false;

    public static float GameFieldLength { get; set; } = 28f;
    public static float GameBottomBorder { get; set; } = -10f;

    [SerializeField] Text status;
    [SerializeField] GameObject playerPrefab;

    void Start() {
        LevelGenerator.singleton.OnEndOfLevelGeneration += SpawnPlayer;
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

    void SpawnPlayer() {
        Instantiate(playerPrefab, new Vector3(0f, 12f, 0f), Quaternion.identity).name = "Player";
    }
}
