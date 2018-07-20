using UnityEngine;

//Class allowing health bar to look at player
public class LookAtPlayer : MonoBehaviour {

    Transform mainCam;

    void Start() {
        mainCam = Camera.main.gameObject.transform;    
    }

    void Update () {
		if (mainCam != null && !PauseMenuManager.IsGamePaused) {
            transform.rotation = Quaternion.LookRotation(transform.position - mainCam.position);
        }
	}
}
