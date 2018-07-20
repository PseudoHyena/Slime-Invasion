using UnityEngine;

//Class that set volume depending on the distance to the player
[RequireComponent(typeof(AudioSource))]
public class SoundVolumeSetter : MonoBehaviour {

    [SerializeField] float maxDistance;

    AudioSource audioSource;

    Transform cam;

    float sqrMaxDistance;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        sqrMaxDistance = maxDistance * maxDistance;
    }

    void Update() {
        if (cam == null) {
            cam = Camera.main.transform;
            return;
        }

        float sqrDistance = Vector3.SqrMagnitude(cam.position - transform.position);
        audioSource.volume = Mathf.Lerp(1f, 0f, sqrDistance / sqrMaxDistance);
    }
}
