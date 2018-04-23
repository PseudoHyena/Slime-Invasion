using UnityEngine;

//Class that play water sounds
public class WaterListner : MonoBehaviour {

    [SerializeField] AudioClip waterSound;
    [SerializeField] AudioClip underwaterSound;

    AudioSource audioSource;
    Player player;
    Transform playerTransform;

    void Start() {
        player = GetComponentInParent<Player>();
        playerTransform = player.transform;
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        if (player.IsUnderWater) {
            audioSource.clip = underwaterSound;
            audioSource.volume = 1f;
            if (!audioSource.isPlaying) {
                audioSource.Play();
            }
        }
        else {
            audioSource.clip = waterSound;
            float maxDistanceFromZero = Mathf.Max(Mathf.Abs(playerTransform.position.x), Mathf.Abs(playerTransform.position.z));
            audioSource.volume = Mathf.Lerp(1f, 0.1f, maxDistanceFromZero / 10f);

            if (!audioSource.isPlaying) {
                audioSource.Play();
            }
        }
    }
}
