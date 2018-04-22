using UnityEngine;

public interface IPickupable {

    void PickUp(Player player);
    void Cling(Transform slime);
}
