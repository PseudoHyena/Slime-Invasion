﻿using UnityEngine;
using UnityEngine.UI;

public class Slime : MonoBehaviour {

    public float Level { get; private set; }

    [SerializeField] GameObject prefab;

    Slider healthSlider;

    GameManager manager;

    int maxHealth;
    int health;

    int damage;

    bool isInitialized = false;

    public void Initialize(int maxHealth, int damage, float level) {
        if (isInitialized) {
            return;
        }

        this.maxHealth = maxHealth;
        this.damage = damage;
        Level = level;
    }

    void Start() {
        healthSlider = gameObject.GetComponentInChildren<Slider>(true);
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;

        manager = FindObjectOfType<GameManager>();

        health = maxHealth;
    }

    public void TakeDamage(int damage) {
        health = Mathf.Clamp(health - damage, 0, maxHealth);

        Debug.Log($"{gameObject.name}, id:{gameObject.GetInstanceID()} take {damage} damage, {health} remain");

        healthSlider.value = health;

        if (health <= 0) {
            Die();
        }
    }

    void Die() {
        SpawnChildren();

        Destroy(gameObject);
    }

    void SpawnChildren() {
        if (Level == 0.25f) {
            return;
        }

        for (int i = 1; i <= 2; ++i) {
            Vector3 pos = transform.position
                    + new Vector3(Random.Range(-i * 2f, i * 2f), 0f, Random.Range(-i * 2f, i * 2f));

            pos.x = Mathf.Clamp(pos.x, -GameManager.GameFieldLength, GameManager.GameFieldLength);
            pos.z = Mathf.Clamp(pos.z, -GameManager.GameFieldLength, GameManager.GameFieldLength);
            pos.y = 1f;

            GameObject go = Instantiate(prefab, pos, Quaternion.identity);
            go.name = $"Slime lvl:{Level / 2f}";
            go.transform.localScale = Vector3.one * (Level / 2f);
            go.GetComponent<Slime>().Initialize(maxHealth / 2, damage / 2, Level / 2f);
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
    }
}
