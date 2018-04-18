using UnityEngine;
using UnityEngine.UI;

public class Slime : MonoBehaviour {

    [SerializeField] int maxHealth = 100;

    Slider healthSlider;

    GameManager manager;

    int health;

    void Start() {
        healthSlider = gameObject.GetComponentInChildren<Slider>(true);
        healthSlider.value = maxHealth;

        manager = FindObjectOfType<GameManager>();

        health = maxHealth;
    }

    void Update() {
    }

    public void TakeDamage(int damage) {

        health = Mathf.Clamp(health - damage, 0, maxHealth);

        Debug.Log($"{gameObject} take {damage} damage, {health} remain");

        healthSlider.value = health;

        if (health <= 0) {
            Die();
        }
    }

    void Die() {
        //Stop jump anim and play death anim
        //Play some effects
        //split slime
        //Destroy slime

        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player") {
            manager.GameOver();
        }
    }
}
