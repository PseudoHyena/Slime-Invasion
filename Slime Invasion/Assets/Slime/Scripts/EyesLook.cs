using UnityEngine;

//Class allowing slimes to look at player
public class EyesLook : MonoBehaviour {

    [SerializeField] Transform leftEye;
    [SerializeField] Transform rightEye;

    Transform target;

    private void Update() {
        if (target == null) {
            target = Camera.main.transform;
        }

        LookAtTarget();
    }

    private void LookAtTarget() {
        if (target == null) {
            return;
        }

        leftEye.rotation = Quaternion.LookRotation(target.position - leftEye.position);
        rightEye.rotation = Quaternion.LookRotation(target.position - rightEye.position);
    }
}
