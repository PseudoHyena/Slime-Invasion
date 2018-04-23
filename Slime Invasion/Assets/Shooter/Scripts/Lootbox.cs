using UnityEngine;

public class Lootbox : MonoBehaviour, IDamageable {

    [SerializeField] GameObject loot;
    [SerializeField] GameObject crackBox;
    [SerializeField] float timeToDestroyFlinders;
    [SerializeField] int health;

    AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();    
    }

    public void TakeDamage(GameObject sender, int damage) {
        if (health <= 0) {
            return;
        }

        health -= damage;

        audioSource.Play();

        if (health <= 0) {
            SpawnLoot();

            Crack();
        }
    }

    void SpawnLoot() {
        Instantiate(loot, transform.position, transform.rotation, GameObject.FindGameObjectWithTag("Environment").transform);
    }

    void Crack() {
        Destroy(Instantiate(crackBox, transform.position, transform.rotation, GameObject.FindGameObjectWithTag("Environment").transform), 
            timeToDestroyFlinders);

        LootboxSpawner.Count--;

        Destroy(gameObject);
    }
}
