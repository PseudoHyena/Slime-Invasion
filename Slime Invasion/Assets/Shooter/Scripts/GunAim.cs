using UnityEngine;

//Class allowing to view health bar of slimes when look at they
public class GunAim : MonoBehaviour {

    [SerializeField] float ShowHealthRange = 50f;

    Camera cam;
    GameObject lastSlimeCanvas;

    void Start() {
        cam = Camera.main;    
    }

    void Update () {
        ShowHealthOfSlime();
	}

    void ShowHealthOfSlime() {
        Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

        RaycastHit hit;
        Ray ray = new Ray(rayOrigin, cam.transform.forward);

        if (Physics.Raycast(ray, out hit, ShowHealthRange)) {
            if (hit.collider.tag == "Slime") {
                lastSlimeCanvas = hit.collider.GetComponentInChildren<Canvas>(true).gameObject;
                lastSlimeCanvas.SetActive(true);
            }
        }
        else {
            if (lastSlimeCanvas != null) {
                lastSlimeCanvas.SetActive(false);
            }
        }
    }
}
