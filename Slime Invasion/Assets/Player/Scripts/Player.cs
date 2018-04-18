using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    [SerializeField] int maxHealth = 100;
    [SerializeField] Slider healthSlider;

    float health;

    GameManager manager;

    void Start() {
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
        manager.GameOver();
    }
}
