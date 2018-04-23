using UnityEngine;

//Interface for all damageable entities
public interface IDamageable {

    void TakeDamage(GameObject sender, int damage);
}
