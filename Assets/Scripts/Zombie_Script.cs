using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_Script : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private float GRAVITY;

    private bool isGrounded = false;

    private Transform zombieTransform;
    private Rigidbody zombieRigidbody;
    private Collider zombieCollider;
    private Animator zombieAnimator;

    private GameObject logic;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        logic = GameObject.FindGameObjectWithTag("Logic");

        zombieTransform = GetComponent<Transform>();
        zombieRigidbody = GetComponent<Rigidbody>();
        zombieCollider = GetComponent<CapsuleCollider>();
        zombieAnimator = GetComponentInChildren<Animator>();

        float size = Random.Range(2,4);
        speedMultiplier = -480 * (size) + 2200; //Linear function for speed

        zombieAnimator.SetFloat("SpeedMult", 7 / Mathf.Sqrt(size));
        zombieTransform.localScale = new Vector3(size, size, size);

        StartCoroutine(Death());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerDir = player.transform.position - zombieTransform.position;
        float distance = playerDir.magnitude;
        playerDir = playerDir.normalized;

        zombieRigidbody.velocity = new Vector3(playerDir.x * speedMultiplier * Time.fixedDeltaTime, zombieRigidbody.velocity.y, playerDir.z * speedMultiplier * Time.fixedDeltaTime); 

        // Jump when close
        if (isGrounded && distance <= 10 && player.transform.position.y > (zombieTransform.position.y + 3))
        {
            // Jump
            zombieRigidbody.AddForce(Vector3.up * jumpMultiplier, ForceMode.VelocityChange);
            isGrounded = false;
        }

        if (!isGrounded)
        {
            zombieRigidbody.velocity += Vector3.down * GRAVITY * Time.fixedDeltaTime;
        }

        Vector3 playerFloor = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        zombieTransform.LookAt(playerFloor);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        else if (collision.gameObject.CompareTag("DashWall"))
        {
            Physics.IgnoreCollision(collision.collider, zombieCollider, true);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            logic.SendMessage("OnZombieContact");
            Destroy(this.gameObject);
        }
    }

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(30);
        Destroy(this.gameObject);
    }
}
