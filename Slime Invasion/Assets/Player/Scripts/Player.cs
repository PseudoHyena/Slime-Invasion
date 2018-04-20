﻿using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour, IDamageable {

    [SerializeField] int maxHealth = 100;
    [SerializeField] Slider healthSlider;

    float health;

    float waterHeight;

    GameManager manager;

    void Start() {
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;

        manager = FindObjectOfType<GameManager>();

        waterHeight = GameManager.WaterHeight;
    }

    void Update() {
        CheckForOutOfMap();
        CheckForUnderWater();
    }

    public void TakeDamage(int damage) {
        health = Mathf.Clamp(health - damage, 0, maxHealth);

        Debug.Log($"{gameObject.name}, id:{gameObject.GetInstanceID()} take {damage} damage, {health} remain");

        healthSlider.value = health;

        if (health <= 0) {
            Die();
        }
    }

    void CheckForOutOfMap() {
        if (transform.position.y < GameManager.GameBottomBorder) {
            Die();
        }
    }

    void CheckForUnderWater() {
        if (transform.position.y < waterHeight) {
            Debug.Log("Water!");
        }
    }

    public void Die() {
        manager.GameOver();
    }
}
