using UnityEngine;

public class Dynamite : MonoBehaviour, IDamageable, IPickupable {

    [SerializeField] bool isActive;
    [SerializeField] float timeToExplode = 8f;
    [SerializeField] float explosionRadius = 15f;
    [SerializeField] float explosionForce = 10f;
    [SerializeField] int maxDamage = 200;
    [SerializeField] int minDamage = 50;
    [SerializeField] GameObject explosionEffect;
    [SerializeField] GameObject sparksEffect;
    int health = 1;

    Slime parent;

    float startBurning;

    void Start() {
        if (isActive) {
            sparksEffect.SetActive(true);
        }

        startBurning = Time.time;

        parent = GetComponentInParent<Slime>(); 
    }

    void Update() {
        CheckForUnderwater();
        WaitForExploding();
    }

    private void OnCollisionEnter(Collision collision) {
        GameObject go = collision.gameObject;
        if (go.tag == "Player") {
            PickUp(go.GetComponent<Player>());
        }
        else if (go.tag == "Slime" && go.GetComponent<Slime>().Level == 1f) {
            Cling(go.transform);
        }
    }

    void WaitForExploding() {
        if (isActive && Time.time >= startBurning + timeToExplode) {
            Explode();
        }
    }

    public void Explode() {
        if (explosionEffect != null) {
            Instantiate(explosionEffect, transform.position, Quaternion.identity).SetActive(true);
        }

        DamageAll();

        Destroy(gameObject);
        if (parent != null) {
            parent.Die(true);
        }
    }

    void DamageAll() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (var item in colliders) {
            if (item == GetComponent<Collider>() || (parent != null && item == parent.GetComponent<Collider>())) {
                continue;
            }

            Rigidbody rb = item.GetComponent<Rigidbody>();

            if (rb != null) {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            IDamageable damageableObj = item.GetComponent<IDamageable>();

            if (damageableObj != null) {
                damageableObj.TakeDamage((int)Mathf.Lerp(maxDamage, minDamage, (item.transform.position - transform.position).magnitude / explosionRadius));
            }
        }
    }

    public void TakeDamage(int damage) {
        if (health <= 0) {
            return;
        }

        health -= damage;

        if (health <= 0) {
            Explode();
        }
    }

    public void PickUp(Player player) {
        if (isActive) {
            return;
        }

        player.DynamitCount++;
        Destroy(gameObject);
    }

    public void Cling(Transform slime) {
        transform.parent = slime;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().isTrigger = true;
    }

    void CheckForUnderwater() {
        if (transform.position.y <= GameManager.WaterHeight) {
            isActive = false;
            sparksEffect.SetActive(false);
        }
    }
}
