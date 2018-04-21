using UnityEngine;

public class LookAtPlayer : MonoBehaviour {

    Transform mainCam;

    void Start() {
        mainCam = Camera.main.gameObject.transform;    
    }

    void Update () {
		if (mainCam != null) {
            transform.rotation = Quaternion.LookRotation(transform.position - mainCam.position);
        }
	}
}
