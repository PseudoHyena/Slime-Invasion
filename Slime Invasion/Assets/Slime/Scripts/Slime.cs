using UnityEngine;
using UnityEngine.UI;

public class Slime : MonoBehaviour, IDamageable {

    public float Level { get; private set; }

    [SerializeField] GameObject regularPrefab;

    Slider healthSlider;

    GameManager manager;

    SlimeType slimeType;

    int maxHealth;
    int health;

    int damage;

    bool isInitialized = false;

    public void Initialize(int maxHealth, int damage, float level, SlimeType type = SlimeType.Regular) {
        if (isInitialized) {
            return;
        }

        this.maxHealth = maxHealth;
        this.damage = damage;
        Level = level;
        slimeType = type;
    }

    void Start() {
        healthSlider = gameObject.GetComponentInChildren<Slider>(true);
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;

        manager = FindObjectOfType<GameManager>();

        health = maxHealth;
    }

    void Update() {
        CheckForOutOfMap();    
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

    public void Die(bool byBoomer = false) {
        SpawnChildren(byBoomer);
    }

    void SpawnChildren(bool byBoomer) {
        int splitIndex = 2;

        if (Level == 0.25f) {
            Destroy(gameObject);

            return;
        }

        if (byBoomer) {
            if (Level == 0.5f) {
                Destroy(gameObject);

                return;
            }

            splitIndex = 4;
        }

        for (int i = 1; i <= 2; ++i) {
            Vector3 pos = transform.position
                    + new Vector3(Random.Range(-i * 2f, i * 2f), Random.Range(0, 1), Random.Range(-i * 2f, i * 2f));

            pos.x = Mathf.Clamp(pos.x, -GameManager.GameFieldLength, GameManager.GameFieldLength);
            pos.z = Mathf.Clamp(pos.z, -GameManager.GameFieldLength, GameManager.GameFieldLength);

            GameObject go = Instantiate(regularPrefab, pos, Quaternion.identity);
            go.name = $"Slime lvl:{Level / splitIndex}, type: {SlimeType.Regular.ToString()}";
            go.transform.localScale = Vector3.one * (Level / splitIndex);
            go.GetComponent<Slime>().Initialize(maxHealth / splitIndex, damage / splitIndex, Level / splitIndex);
        }

        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Slime") {
            return;
        }

        IDamageable damageableObj = collision.gameObject.GetComponent<IDamageable>();
        if (damageableObj != null) {
            damageableObj.TakeDamage(damage);
        }
    }
}

public enum SlimeType {
    Regular, Boomer
}
