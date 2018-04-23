using UnityEngine;

//Main class for dynimites
public class Dynamite : MonoBehaviour, IDamageable, IPickupable {

    [SerializeField] bool isActive;
    [SerializeField] float timeToExplode = 8f;
    [SerializeField] float explosionRadius = 15f;
    [SerializeField] float explosionForce = 10f;
    [SerializeField] int maxDamage = 200;
    [SerializeField] int minDamage = 50;
    [SerializeField] GameObject explosionEffect;
    [SerializeField] GameObject sparksEffect;
    [SerializeField] GameObject explosionSound;
    [SerializeField] GameObject wickSound;

    int health = 1;

    Slime parent;

    float startBurning;

    bool isCling;
    Transform target;
    Vector3 delta;

    public bool ImpactFromPlayer { get; private set; } = false;

    void Start() {
        if (isActive) {
            wickSound.SetActive(true);
            sparksEffect.SetActive(true);
        }

        startBurning = Time.time;

        parent = GetComponentInParent<Slime>(); 
        if (parent == null) {
            ImpactFromPlayer = true;
        }
    }

    void Update() {
        FollowTarget();
        CheckForUnderwater();
        WaitForExploding();
    }

    private void OnCollisionEnter(Collision collision) {
        GameObject go = collision.gameObject;
        if (go.tag == "Player") {
            PickUp(go.GetComponent<Player>());
        }
        else if (go.tag == "Slime" && go.GetComponent<Slime>().Level >= 0.5f) {
            Cling(go.transform);
        }
    }

    void WaitForExploding() {
        if (isActive && Time.time >= startBurning + timeToExplode) {
            Explode();
        }
    }

    public void Explode() {
        Instantiate(explosionEffect, transform.position, Quaternion.identity).SetActive(true);

        Destroy(Instantiate(explosionSound, transform.position, Quaternion.identity), 6f);

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
                damageableObj.TakeDamage(gameObject, (int)Mathf.Lerp(maxDamage, minDamage, (item.transform.position - transform.position).magnitude / explosionRadius));
            }
        }
    }

    public void TakeDamage(GameObject sender, int damage) {
        if (health <= 0) {
            return;
        }

        health -= damage;

        if (health <= 0) {
            if (sender.GetComponent<Player>() != null) {
                ImpactFromPlayer = true;
            }

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
        isCling = true;
        delta = slime.position - transform.position;
        target = slime;

        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().isTrigger = true;
    }

    void CheckForUnderwater() {
        if (transform.position.y <= GameManager.WaterHeight) {
            isActive = false;
            sparksEffect.SetActive(false);
            wickSound.SetActive(false);
        }
    }

    void FollowTarget() {
        if (!isCling || target == null) {
            return;
        }

        transform.position = target.position - delta;
    }
}
