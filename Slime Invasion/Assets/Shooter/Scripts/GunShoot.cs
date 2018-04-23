using UnityEngine;

[RequireComponent(typeof(Slime))]
public class GunShoot : MonoBehaviour {

	[SerializeField] float fireRate = 0.25f;										
	[SerializeField] float weaponRange = 50f;
    [SerializeField] int damage = 20;

	[SerializeField] Transform gunEnd;
	[SerializeField] ParticleSystem muzzleFlash;
	[SerializeField] ParticleSystem cartridgeEjection;

    public static bool CanShoot { get; set; } = true;

    AudioSource audioSource;

    float nextFire;												
	Animator anim;

    Camera cam;

	void Start () {
		anim = GetComponent<Animator> ();
        audioSource = GetComponent<AudioSource>();
        cam = Camera.main;
	}

	void Update () {
        if (PauseMenuManager.IsGamePaused) {
            return;
        }

        Fire();
	}

    void Fire() {
        if (CanShoot && Input.GetButtonDown("Fire1") && Time.time > nextFire) {
            nextFire = Time.time + fireRate;

            audioSource.Play();

            muzzleFlash.Play();
            cartridgeEjection.Play();

            anim.SetTrigger("Fire");

            Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            RaycastHit hit;
            Ray ray = new Ray(rayOrigin, cam.transform.forward);

            Debug.DrawRay(rayOrigin, cam.transform.forward * weaponRange, Color.red, 2f, true);

            if (Physics.Raycast(ray, out hit, weaponRange)) {
                IDamageable damageableObj = hit.collider.GetComponent<IDamageable>();
                if (damageableObj != null) {
                    damageableObj.TakeDamage(GetComponentInParent<Player>().gameObject, damage);
                }
            }
        }
    }
}