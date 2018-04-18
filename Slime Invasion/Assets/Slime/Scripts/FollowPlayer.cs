using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FollowPlayer : MonoBehaviour {

    [SerializeField] float viewDistance = 30f;
    [SerializeField] float AttackDistance = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float maxJumpAngle = 20f;
    [SerializeField] float minJumpAngle = 10f;
    [SerializeField] float jumpRate = 1f;

    Transform player;
    Rigidbody rb;

    float nextJump;

    float sqrViewDistance;
    float stayTreshold = 0.05f;

    bool isPlayerVisible;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        sqrViewDistance = viewDistance * viewDistance;

        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        CheckVisibility();
        ChooseTarget();
    }

    void FixedUpdate() {
    }

    void CheckVisibility() {
        //For best performance
        Vector3 fromSlimeToPlayer = player.position - transform.position;
        float sqrDistatnce = fromSlimeToPlayer.x * fromSlimeToPlayer.x + fromSlimeToPlayer.z * fromSlimeToPlayer.z;

        isPlayerVisible = sqrDistatnce <= sqrViewDistance ? true : false;
    }

    bool CheckCollisionWithFloor() {
        Ray ray = new Ray(transform.position - new Vector3(0f, 1f, 0f), Vector3.down);

        if (Physics.Raycast(ray, stayTreshold)) {
            return true;
        }

        return false;
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

        rb.AddForce(fromSlimeToPlayer.normalized * jumpForce, ForceMode.Impulse);
    }
}
