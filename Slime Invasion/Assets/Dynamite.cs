using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamite : MonoBehaviour {

    [SerializeField] float timeToExplode = 8f;
    [SerializeField] float explosionRadius = 10f;
    [SerializeField] float explosionForce = 10f;
    [SerializeField] int maxDamage = 200;

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
        //Effect 

        DamageAll();

        parent.Die(true);
    }

    void DamageAll() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (var item in colliders) {
            Rigidbody rb = item.GetComponent<Rigidbody>();

            if (rb != null) {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            IDamageable damageableObj = item.GetComponent<IDamageable>();

            if (damageableObj != null) {
                damageableObj.TakeDamage(maxDamage / ((int)(item.transform.position - transform.position).magnitude + 1));
            }
        }
    }
}
