using UnityEngine;

public class Dynamite : MonoBehaviour, IDamageable {

    [SerializeField] float timeToExplode = 8f;
    [SerializeField] float explosionRadius = 15f;
    [SerializeField] float explosionForce = 10f;
    [SerializeField] int maxDamage = 200;
    [SerializeField] int minDamage = 50;
    [SerializeField] GameObject explosionEffect;
    int health = 1;

    Slime parent;

    float startBurning;

    void Start() {
        startBurning = Time.time;

        parent = GetComponentInParent<Slime>(); 
    }

    void Update() {
        WaitForExploding();
    }

    void WaitForExploding() {
        if (Time.time >= startBurning + timeToExplode) {
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
}
