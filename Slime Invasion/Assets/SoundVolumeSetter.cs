using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundVolumeSetter : MonoBehaviour {

    [SerializeField] float maxDistance;

    AudioSource audioSource;

    Transform player;

    float sqrMaxDistance;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        sqrMaxDistance = maxDistance * maxDistance;
    }

    void Update() {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            return;
        }

        float sqrDistance = Vector3.SqrMagnitude(player.position - transform.position);
        audioSource.volume = Mathf.Lerp(1f, 0f, sqrDistance / sqrMaxDistance);
    }
}
