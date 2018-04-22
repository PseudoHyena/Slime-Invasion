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
    [SerializeField] Text dynamiteCountText;

    [SerializeField] GameObject dynamite;
    [SerializeField] Transform dynamiteSpawnPoint;
    [SerializeField] float throwRate = 0.5f;
    [SerializeField] float throwForce = 10f;

    int dynamiteCount;
    public int DynamitCount {
        get {
            return dynamiteCount;
        }
        set {
            dynamiteCountText.text = $"Dynamite: {value}";
            dynamiteCount = value;
        }
    }

    float nextThrow;

    float health;

    float waterHeight;
    float canBreathSec = 20f;
    float startDive;
    bool isUnderWater = false;

    GameManager manager;

    MovementPresset movementPresset;
    RigidbodyFirstPersonController controller;
    CapsuleCollider capsuleCollider;

    Transform cam;

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

        cam = GetComponentInChildren<Camera>().transform;

        SetMovementPresset();
    }

    void Update() {
        CheckForOutOfMap();
        CheckForWaterImpact();
        CheckForDynamiteThrow();
    }

    void SetMovementPresset() {
        movementPresset.ForwardSpeed = controller.movementSettings.ForwardSpeed;
        movementPresset.BackwardSpeed = controller.movementSettings.BackwardSpeed;
        movementPresset.StrafeSpeed = controller.movementSettings.StrafeSpeed;
        movementPresset.RunMultiplier = controller.movementSettings.RunMultiplier;
        movementPresset.JumpForce = controller.movementSettings.JumpForce;
    }

    public void TakeDamage(GameObject sender, int damage) {
        if (health <= 0) {
            return;
        }

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
                GunShoot.CanShoot = false;
                isUnderWater = true;
                startDive = Time.time;
                airSlider.gameObject.SetActive(true);
            }
        }
        else {
            GunShoot.CanShoot = true;
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

    void CheckForDynamiteThrow() {
        if (Input.GetButton("Fire2") && Time.time > nextThrow && DynamitCount > 0) {
            nextThrow = Time.time + throwRate;
            ThrowDynamite();
        }
    }

    void ThrowDynamite() {
        GameObject go = Instantiate(dynamite, dynamiteSpawnPoint.position, Quaternion.identity);
        Rigidbody rb = go.GetComponent<Rigidbody>();

        rb.AddForce(dynamiteSpawnPoint.forward * throwForce, ForceMode.Impulse);
        DynamitCount--;
    }

    public void Die() {
        manager.GameOver();
    }
}