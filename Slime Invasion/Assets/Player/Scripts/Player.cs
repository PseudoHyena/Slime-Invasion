using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour, IDamageable {

    struct MovementPresset {
        public float ForwardSpeed;
        public float BackwardSpeed;
        public float StrafeSpeed;
        public float RunMultiplier;
        public float JumpForce;
    }

    [SerializeField] int maxHealth = 100;
    [SerializeField] Slider healthSlider;
    [SerializeField] Slider airSlider;

    float health;

    float waterHeight;
    float canBreathSec = 20f;
    float startDive;
    bool isUnderWater = false;

    GameManager manager;

    MovementPresset movementPresset;
    RigidbodyFirstPersonController controller;
    CapsuleCollider capsuleCollider;

    void Start() {
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;

        manager = FindObjectOfType<GameManager>();

        controller = GetComponent<RigidbodyFirstPersonController>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        waterHeight = GameManager.WaterHeight;
        airSlider.maxValue = canBreathSec;
        airSlider.value = canBreathSec;

        SetMovementPresset();
    }

    void Update() {
        CheckForOutOfMap();
        CheckForWaterImpact();
    }

    void SetMovementPresset() {
        movementPresset.ForwardSpeed = controller.movementSettings.ForwardSpeed;
        movementPresset.BackwardSpeed = controller.movementSettings.BackwardSpeed;
        movementPresset.StrafeSpeed = controller.movementSettings.StrafeSpeed;
        movementPresset.RunMultiplier = controller.movementSettings.RunMultiplier;
        movementPresset.JumpForce = controller.movementSettings.JumpForce;
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

    void CheckForWaterImpact() {
        if (transform.position.y - capsuleCollider.height / 2f + 0.1f < waterHeight) {
            controller.movementSettings.ForwardSpeed = movementPresset.ForwardSpeed / 2.5f;
            controller.movementSettings.BackwardSpeed = movementPresset.BackwardSpeed / 2.5f;
            controller.movementSettings.StrafeSpeed = movementPresset.StrafeSpeed / 2.5f;
            controller.movementSettings.RunMultiplier = 1.1f;
            controller.movementSettings.JumpForce = movementPresset.JumpForce / 2.5f;
        }
        else {
            controller.movementSettings.ForwardSpeed = movementPresset.ForwardSpeed;
            controller.movementSettings.BackwardSpeed = movementPresset.BackwardSpeed;
            controller.movementSettings.StrafeSpeed = movementPresset.StrafeSpeed;
            controller.movementSettings.RunMultiplier = movementPresset.RunMultiplier;
            controller.movementSettings.JumpForce = movementPresset.JumpForce;
        }

        if (transform.position.y + capsuleCollider.height / 2f + 0.1f < waterHeight) {
            if (!isUnderWater) {
                isUnderWater = true;
                startDive = Time.time;
                airSlider.gameObject.SetActive(true);
            }
        }
        else {
            isUnderWater = false;
            airSlider.gameObject.SetActive(false);
            airSlider.value = canBreathSec;
        }

        if (isUnderWater) {
            airSlider.value = startDive + canBreathSec - Time.time;
            if (Time.time > startDive + canBreathSec) {
                Die();
            }
        }
    }

    public void Die() {
        manager.GameOver();
    }
}