using UnityEngine;

//Class responsible for the movement of slimes 
[RequireComponent(typeof(Rigidbody))]
public class SlimeMovement : MonoBehaviour {

    [SerializeField] float viewDistance = 30f;
    [SerializeField] float AttackDistance = 5f;
    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float maxJumpAngle = 20f;
    [SerializeField] float minJumpAngle = 10f;
    [SerializeField] float jumpRate = 1f;

    [SerializeField] AudioClip jumpSound;

    AudioSource audioSource;

    Slime slime;
    Transform player;
    Rigidbody rb;

    float nextJump;

    float sqrViewDistance;
    float stayTreshold = 0.1f;

    bool isPlayerVisible;

    float jumpForcePresset;

    void Start() {
        slime = GetComponent<Slime>();

        sqrViewDistance = viewDistance * viewDistance * slime.Level * slime.Level;
        jumpForcePresset = jumpForce;

        audioSource = GetComponent<AudioSource>();

        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        if (PauseMenuManager.IsGamePaused) {
            return;
        }

        if (player == null) {
            CheckForPlayer();
            return;
        }

        CheckForWaterImpact();
        CheckVisibility();
        ChooseTarget();
        LookAtPlayer();
    }

    void CheckForPlayer() {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (go != null) {
            player = go.transform;
        }
    }

    void CheckVisibility() {
        Vector3 fromSlimeToPlayer = player.position - transform.position;
        float sqrDistatnce = fromSlimeToPlayer.x * fromSlimeToPlayer.x + fromSlimeToPlayer.z * fromSlimeToPlayer.z;

        isPlayerVisible = sqrDistatnce <= sqrViewDistance ? true : false;
    }

    bool CheckCollisionWithFloor() {
        Vector3 point1 = transform.position;
        Vector3 point2 = point1;
        point2.y += stayTreshold;

        RaycastHit hit;

        if (Physics.CapsuleCast(point1, point2, slime.Level, Vector3.down, out hit, stayTreshold)) {
            if (hit.collider.GetComponent<Collider>() == GetComponent<Collider>()) {
                return false;
            }
            return true;
        }

        return false;
    }

    void CheckForWaterImpact() {
        if (transform.position.y - slime.Level + (slime.Level / 10f) < GameManager.WaterHeight) {
            jumpForce = jumpForcePresset / 2f;
        }
        else {
            jumpForce = jumpForcePresset;
        }
        
    }

    void ChooseTarget() {
        if (Time.time >= nextJump && CheckCollisionWithFloor()) {
            nextJump = Time.time + jumpRate;

            if (isPlayerVisible) {
                Follow(player.position);
            }
            else {
                Vector3 dest = transform.position
                    + new Vector3(Random.Range(-viewDistance, viewDistance), 0f, Random.Range(-viewDistance, viewDistance));

                dest.x = Mathf.Clamp(dest.x, -GameManager.GameFieldLength, GameManager.GameFieldLength);
                dest.z = Mathf.Clamp(dest.z, -GameManager.GameFieldLength, GameManager.GameFieldLength);
                dest.y = 1f;

                Follow(dest);
            }
        }
    }

    void Follow(Vector3 pos) {
        Vector3 fromSlimeToPlayer = (pos - transform.position);
        fromSlimeToPlayer.y = Mathf.Lerp(minJumpAngle, maxJumpAngle, fromSlimeToPlayer.magnitude / viewDistance / AttackDistance);

        audioSource.clip = jumpSound;
        audioSource.Play();

        rb.AddForce(fromSlimeToPlayer.normalized * jumpForce, ForceMode.Impulse);
    }

    void LookAtPlayer() {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
    }
}
