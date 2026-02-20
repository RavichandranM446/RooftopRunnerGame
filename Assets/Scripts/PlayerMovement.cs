//using UnityEngine;
//using System.Collections;

//public class PlayerMovement : MonoBehaviour
//{
//    [Header("Forward")]
//    public float forwardSpeed = 6f;

//    [Header("Lane")]
//    public float laneDistance = 2f;
//    public float laneSmoothSpeed = 10f;

//    [Header("Jump")]
//    public float jumpForce = 8f;
//    public float gravity = -28f;

//    [Header("Slide")]
//    public float slideTime = 0.8f;

//    [Header("Animation")]
//    public Animator animator;

//    private CharacterController controller;
//    private Rigidbody rb;
//    private Vector3 velocity;

//    private int currentLane = 1;
//    private float targetX;

//    private bool isSliding;
//    private bool isDead;
//    private bool glassTriggered;

//    // Jump buffer
//    private float jumpBufferTime = 0.15f;
//    private float jumpBufferCounter;

//    void Awake()
//    {
//        controller = GetComponent<CharacterController>();
//        rb = GetComponent<Rigidbody>();

//        if (!animator)
//            animator = GetComponentInChildren<Animator>();
//    }

//    void Start()
//    {
//        if (rb)
//        {
//            rb.isKinematic = true;
//            rb.useGravity = false;
//        }

//        targetX = transform.position.x;

//        // 🔥 Always run (Blend Tree)
//        if (animator)
//            animator.SetFloat("Speed", 1f);
//    }

//    void Update()
//    {
//        if (isDead) return;

//        HandleInput();
//        Move();
//        CheckGlassBelow();
//    }

//    // ---------------- INPUT ----------------
//    void HandleInput()
//    {
//        if (Input.GetKeyDown(KeyCode.A) || SwipeManager.swipeLeft)
//            ChangeLane(-1);

//        if (Input.GetKeyDown(KeyCode.D) || SwipeManager.swipeRight)
//            ChangeLane(1);

//        if (Input.GetKeyDown(KeyCode.Space) || SwipeManager.swipeUp)
//            jumpBufferCounter = jumpBufferTime;

//        if ((Input.GetKeyDown(KeyCode.S) || SwipeManager.swipeDown) && !isSliding)
//            StartCoroutine(Slide());
//    }

//    void ChangeLane(int dir)
//    {
//        currentLane = Mathf.Clamp(currentLane + dir, 0, 2);
//        targetX = (currentLane - 1) * laneDistance;

//        AudioManager.instance?.PlaySFX(AudioManager.instance.swipe);
//    }

//    // ---------------- SLIDE ----------------
//    IEnumerator Slide()
//    {
//        isSliding = true;

//        // 🔥 Slide animation
//        animator.SetTrigger("Slide");
//        AudioManager.instance?.PlaySFX(AudioManager.instance.slide);

//        float h = controller.height;
//        Vector3 c = controller.center;

//        controller.height = h * 0.5f;
//        controller.center = c - Vector3.up * (h * 0.25f);

//        yield return new WaitForSeconds(slideTime);

//        controller.height = h;
//        controller.center = c;

//        isSliding = false;
//    }

//    // ---------------- MOVEMENT ----------------
//    void Move()
//    {
//        Vector3 move = Vector3.zero;

//        // Forward movement
//        move.z = forwardSpeed * Time.deltaTime;

//        // Lane smooth movement
//        float newX = Mathf.Lerp(transform.position.x, targetX, laneSmoothSpeed * Time.deltaTime);
//        move.x = newX - transform.position.x;

//        // Jump buffer
//        if (jumpBufferCounter > 0)
//        {
//            jumpBufferCounter -= Time.deltaTime;

//            if (controller.isGrounded)
//            {
//                velocity.y = jumpForce;

//                // 🔥 Jump animation
//                animator.SetTrigger("Jump");
//                AudioManager.instance?.PlaySFX(AudioManager.instance.jump);

//                jumpBufferCounter = 0;
//            }
//        }

//        // Gravity
//        if (controller.isGrounded)
//        {
//            if (velocity.y < 0)
//                velocity.y = -2f;
//        }
//        else
//        {
//            velocity.y += gravity * Time.deltaTime;
//        }

//        move.y = velocity.y * Time.deltaTime;
//        controller.Move(move);
//    }

//    // ---------------- GLASS FALL ----------------
//    void CheckGlassBelow()
//    {
//        if (glassTriggered || isDead) return;

//        if (Physics.Raycast(transform.position + Vector3.up * 0.3f,
//            Vector3.down, out RaycastHit hit, 2f))
//        {
//            if (hit.collider.CompareTag("Glass"))
//            {
//                glassTriggered = true;
//                isDead = true;

//                // 🔥 Fall animation (Game Over)
//                animator.SetTrigger("Fall");
//                AudioManager.instance?.PlaySFX(AudioManager.instance.glassBreak);

//                StartCoroutine(ForceGlassFall());
//            }
//        }
//    }

//    // ---------------- OBSTACLE HIT ----------------
//    void OnControllerColliderHit(ControllerColliderHit hit)
//    {
//        if (isDead) return;

//        if (hit.gameObject.CompareTag("JumpObstacle") ||
//            hit.gameObject.CompareTag("SlideObstacle"))
//        {
//            isDead = true;

//            // 🔥 Hit animation (Game Over)
//            animator.SetTrigger("Hit");
//            AudioManager.instance?.PlaySFX(AudioManager.instance.gameOver);

//            StartCoroutine(DelayedGameOver());
//        }
//    }

//    IEnumerator DelayedGameOver()
//    {
//        yield return new WaitForSeconds(1.2f);
//        ShowGameOverUI();
//    }

//    IEnumerator ForceGlassFall()
//    {
//        controller.enabled = false;
//        rb.isKinematic = false;
//        rb.velocity = Vector3.down * 1.4f;

//        yield return new WaitForSeconds(1.2f);
//        ShowGameOverUI();
//    }

//    void ShowGameOverUI()
//    {
//        FindObjectOfType<GameOverUIManager>()
//            ?.ShowGameOver(ScoreManager.instance.score);
//    }
//}
using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Forward")]
    public float forwardSpeed = 6f;

    [Header("Lane")]
    public float laneDistance = 2f;
    public float laneSmoothSpeed = 10f;

    [Header("Jump")]
    public float jumpForce = 8f;
    public float gravity = -28f;

    [Header("Slide")]
    public float slideTime = 0.8f;

    [Header("Animation")]
    public Animator animator;

    private CharacterController controller;
    private Rigidbody rb;
    private Vector3 velocity;

    private int currentLane = 1;
    private float targetX;

    private bool isSliding;
    private bool isDead;
    private bool glassTriggered;
    private bool hasJumped; // 🔥 jump spam prevent

    // Jump buffer
    private float jumpBufferTime = 0.15f;
    private float jumpBufferCounter;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();

        if (!animator)
            animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        if (rb)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        targetX = transform.position.x;

        // 🔥 Always run (Blend Tree)
        if (animator)
            animator.SetFloat("Speed", 1f);
    }

    void Update()
    {
        if (isDead) return;

        HandleInput();
        Move();
        CheckGlassBelow();
    }

    // ---------------- INPUT ----------------
    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || SwipeManager.swipeLeft)
            ChangeLane(-1);

        if (Input.GetKeyDown(KeyCode.D) || SwipeManager.swipeRight)
            ChangeLane(1);

        if (Input.GetKeyDown(KeyCode.Space) || SwipeManager.swipeUp)
            jumpBufferCounter = jumpBufferTime;

        if ((Input.GetKeyDown(KeyCode.S) || SwipeManager.swipeDown) && !isSliding)
            StartCoroutine(Slide());
    }

    void ChangeLane(int dir)
    {
        currentLane = Mathf.Clamp(currentLane + dir, 0, 2);
        targetX = (currentLane - 1) * laneDistance;

        AudioManager.instance?.PlaySFX(AudioManager.instance.swipe);
    }

    // ---------------- SLIDE ----------------
    IEnumerator Slide()
    {
        isSliding = true;

        if (animator)
            animator.SetTrigger("Slide");

        AudioManager.instance?.PlaySFX(AudioManager.instance.slide);

        float h = controller.height;
        Vector3 c = controller.center;

        controller.height = h * 0.5f;
        controller.center = c - Vector3.up * (h * 0.25f);

        yield return new WaitForSeconds(slideTime);

        controller.height = h;
        controller.center = c;

        isSliding = false;
    }

    // ---------------- MOVEMENT ----------------
    void Move()
    {
        Vector3 move = Vector3.zero;

        // Forward
        move.z = forwardSpeed * Time.deltaTime;

        // Lane smooth
        float newX = Mathf.Lerp(transform.position.x, targetX, laneSmoothSpeed * Time.deltaTime);
        move.x = newX - transform.position.x;

        // Jump buffer
        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;

            if (controller.isGrounded && !hasJumped)
            {
                velocity.y = jumpForce;
                hasJumped = true;

                if (animator)
                    animator.SetTrigger("Jump");

                AudioManager.instance?.PlaySFX(AudioManager.instance.jump);
                jumpBufferCounter = 0;
            }
        }

        // 🔥 GROUND SNAP FIX (FLOATING FIX)
        if (controller.isGrounded)
        {
            if (velocity.y < 0)
                velocity.y = -10f; // ⬅ force stick to ground

            hasJumped = false;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        move.y = velocity.y * Time.deltaTime;
        controller.Move(move);
    }

    // ---------------- GLASS FALL ----------------
    void CheckGlassBelow()
    {
        if (glassTriggered || isDead) return;

        if (Physics.Raycast(transform.position + Vector3.up * 0.3f,
            Vector3.down, out RaycastHit hit, 2f))
        {
            if (hit.collider.CompareTag("Glass"))
            {
                glassTriggered = true;
                isDead = true;

                if (animator)
                    animator.SetTrigger("Fall");

                AudioManager.instance?.PlaySFX(AudioManager.instance.glassBreak);
                StartCoroutine(ForceGlassFall());
            }
        }
    }

    // ---------------- OBSTACLE HIT ----------------
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (isDead) return;

        if (hit.gameObject.CompareTag("JumpObstacle") ||
            hit.gameObject.CompareTag("SlideObstacle"))
        {
            isDead = true;

            if (animator)
                animator.SetTrigger("Hit");

            AudioManager.instance?.PlaySFX(AudioManager.instance.gameOver);
            StartCoroutine(DelayedGameOver());
        }
    }

    IEnumerator DelayedGameOver()
    {
        yield return new WaitForSeconds(1.2f);
        ShowGameOverUI();
    }

    IEnumerator ForceGlassFall()
    {
        controller.enabled = false;
        rb.isKinematic = false;
        rb.velocity = Vector3.down * 1.4f;

        yield return new WaitForSeconds(1.2f);
        ShowGameOverUI();
    }

    void ShowGameOverUI()
    {
        FindObjectOfType<GameOverUIManager>()
            ?.ShowGameOver(ScoreManager.instance.score);
    }
}

