using UnityEngine;
using UnityEngine.UI;

public class Slime : MonoBehaviour, IDamageable {

    public float Level { get; private set; }

    [SerializeField] GameObject regularPrefab;
    [SerializeField] AudioClip hurtSound;

    AudioSource audioSource;

    Slider healthSlider;

    Player player;

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

        health = maxHealth;

        audioSource = GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
    }

    void Update() {
        CheckForOutOfMap();    
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Slime") {
            return;
        }

        if (collision.gameObject.tag == "Dynamite") {
            return;
        }

        IDamageable damageableObj = collision.gameObject.GetComponent<IDamageable>();
        if (damageableObj != null) {
            damageableObj.TakeDamage(gameObject, damage);
        }
    }

    public void TakeDamage(GameObject sender, int damage) {
        if (health <= 0) {
            return;
        }

        health = Mathf.Clamp(health - damage, 0, maxHealth);

        Debug.Log($"{gameObject.name}, id:{gameObject.GetInstanceID()} take {damage} damage, {health} remain");

        audioSource.clip = hurtSound;
        audioSource.Play();

        healthSlider.value = health;

        if (health <= 0) {
            if (sender.GetComponent<Player>() != null ||
                sender.GetComponent<Dynamite>()?.ImpactFromPlayer == true) {
                player.Score += maxHealth;
            }

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

        if (byBoomer == false && slimeType == SlimeType.Boomer) {
            DropDynamite();
        }
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

            GameObject go = Instantiate(regularPrefab, pos, Quaternion.identity, transform.parent);
            go.name = $"Slime lvl:{Level / splitIndex}, type: {SlimeType.Regular.ToString()}";
            go.transform.localScale = Vector3.one * (Level / splitIndex);
            go.GetComponent<Slime>().Initialize(maxHealth / splitIndex, damage / splitIndex, Level / splitIndex);

            if (Level == 1f) {
                Spawner.MediumSlimesCount++;
            }
            else if (Level == 0.5f) {
                Spawner.SmallSlimesCount++;
            }
        }

        if (Level == 1f) {
            Spawner.BigSlimesCount--;
        }
        else if (Level == 0.5f) {
            Spawner.MediumSlimesCount--;
        }
        else {
            Spawner.SmallSlimesCount--;
        }

        Destroy(gameObject);
    }

    void DropDynamite() {
        Dynamite dynamite = GetComponentInChildren<Dynamite>();

        dynamite.transform.parent = null;
        dynamite.GetComponent<Rigidbody>().isKinematic = false;
        dynamite.GetComponent<Collider>().isTrigger = false;
    }
}

public enum SlimeType {
    Regular, Boomer
}
