﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamite : MonoBehaviour, IDamageable {

    [SerializeField] float timeToExplode = 8f;
    [SerializeField] float explosionRadius = 15f;
    [SerializeField] float explosionForce = 10f;
    [SerializeField] int maxDamage = 200;
    [SerializeField] int minDamage = 50;

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
        //Effect 
        DamageAll();

        Destroy(gameObject);
        parent.Die(true);
    }

    void DamageAll() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (var item in colliders) {
            if (item == GetComponent<Collider>() || item == parent.GetComponent<Collider>()) {
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
        health -= damage;

        if (health <= 0) {
            Explode();
        }
    }
}
