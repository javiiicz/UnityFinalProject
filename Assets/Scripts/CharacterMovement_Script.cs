using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement_Script : MonoBehaviour
{
    [SerializeField] private Transform cameraPosition;

    [SerializeField] private float GRAVITY;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private float fallMultiplier = 2.0f;
    [SerializeField] private float dashMultiplier;
    [SerializeField] private float rotationalSpeed;
    [SerializeField] private float timeTillDash = 1;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float repelMultiplier = 100;

    [SerializeField] private GameObject dashHitbox;
    [SerializeField] private GameObject indicator;
    [SerializeField] private ParticleSystem particles;

    private Rigidbody playerRigidbody;
    private bool isGrounded = true;
    private bool canDash = true;
    private bool isDashing = false;
    private bool isRepelling = false;
    private Vector3 dashVector;

    [SerializeField] private GameObject dashWall;
    private Vector3 startPoint;
    private Vector3 endPoint;
    [SerializeField] private float wallHeight;
    [SerializeField] private float wallThickness;

    private bool frozen = true;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        StartCoroutine(Countdown());
    }

    private void Update()
    {
        // Jump Logic
        if (!frozen && Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isGrounded = false;
            animator.SetBool("isJumping", true);
            playerRigidbody.velocity += Vector3.up * jumpMultiplier;
        }


        // Dash logic
        if (!frozen && canDash && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetMouseButtonDown(0)))
        {

            canDash = false;
            isDashing = true;

            dashHitbox.SetActive(true);
            indicator.SetActive(false);
            startPoint = transform.position;

            Vector3 playerForward = playerRigidbody.gameObject.transform.forward.normalized;
            dashVector = playerForward * dashMultiplier;
            playerRigidbody.velocity += dashVector;

            StartCoroutine(Dash());
            StartCoroutine(DashCooldown());
        }

    }

    void FixedUpdate()
    {

        // I want the movement to be relative to the camera so get the vector where
        // the camera is facing and get the sideways one via cross product
        Vector3 forward = new Vector3(cameraPosition.forward.x, 0, cameraPosition.forward.z).normalized;
        Vector3 sideways = Vector3.Cross(Vector3.up, forward).normalized;
        Vector2 input = Vector2.zero;

        if (!frozen)
        {
            //Store user input as a movement vector
            input = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
        }

        // Black magic happening here. (Just multiplying the movement by the respective input)
        Vector3 movement = new Vector3((forward.x * input.x) + (sideways.x * input.y), 0, (forward.z * input.x) + (sideways.z * input.y));

        if (!isDashing && !isRepelling)
        {
            playerRigidbody.velocity = new Vector3(movement.x * Time.fixedDeltaTime * speedMultiplier, playerRigidbody.velocity.y, movement.z * Time.fixedDeltaTime * speedMultiplier);
        }


        if (movement != Vector3.zero)
        {
            animator.SetBool("isMoving", true);
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            playerRigidbody.rotation = Quaternion.Slerp(playerRigidbody.rotation, targetRotation, rotationalSpeed * Time.fixedDeltaTime);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        // Gravity
        if (playerRigidbody.velocity.y <= 0.5)
        {
            // Falls Quicker
            playerRigidbody.velocity += Vector3.down * GRAVITY * fallMultiplier * Time.fixedDeltaTime;
        }
        else
        {
            playerRigidbody.velocity += Vector3.down * GRAVITY * Time.fixedDeltaTime;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("DashWall"))
        {
            isRepelling = true;
            Vector3 wallDir = collision.GetContact(0).normal;
            Vector3 repellingForce = wallDir * repelMultiplier;
            playerRigidbody.AddForce(repellingForce, ForceMode.Impulse);
            StartCoroutine(Repel());
        }

    }

    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(timeTillDash);
        canDash = true;
        indicator.SetActive(true);

    }

    private IEnumerator Dash()
    {
        particles.Play();
        yield return new WaitForSeconds(dashDuration);

        playerRigidbody.velocity -= dashVector * 0.7f;
        endPoint = transform.position;

        yield return new WaitForSeconds(0.1f);

        CreateWall(startPoint, endPoint);

        isDashing = false;
        dashHitbox.SetActive(false);
    }

    private IEnumerator Repel()
    {
        yield return new WaitForSeconds(0.3f);
        isRepelling = false;
        playerRigidbody.velocity = Vector3.zero;
    }

    public void CreateWall(Vector3 p1, Vector3 p2)
    {
        Vector3 position = (p1 + p2) / 2;
        Quaternion rotation = Quaternion.LookRotation(p2 - p1);
        float length = Vector3.Distance(p1, p2);

        GameObject wall = Instantiate(dashWall, position + new Vector3(0, 1, 0), rotation);
        wall.transform.localScale = new Vector3(wallThickness, wallHeight, length);
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(4);
        frozen = false;
    }
}
