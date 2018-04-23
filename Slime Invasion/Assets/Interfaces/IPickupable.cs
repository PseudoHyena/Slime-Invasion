using UnityEngine;

//Interface for all pickupable objects
public interface IPickupable {

    void PickUp(Player player);
    void Cling(Transform slime);
}
