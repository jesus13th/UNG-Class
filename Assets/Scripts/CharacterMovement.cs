using UnityEngine;

public class CharacterMovement : MonoBehaviour {
    [Header("Componentes")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Health")]
    [SerializeField] private int health = 1;

    [Header("Velocity")]
    [SerializeField] private float velocityTargetWalking = 4;
    private float velocityCurrent;
    [SerializeField] private bool canRun = false;
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift;
    [SerializeField] private bool isRunning = false;
    [SerializeField] private float velocityTargetRunning = 8;
    [SerializeField] private float velocitySmooth = 0.25f;
    [SerializeField] private string velocityPropertyInAnimator = "Velocity";
    private float refVelocity;
    private Vector2 axis;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 4;
    private bool isOnGround = false;
    [SerializeField] private float groundDetectionRadius = 0.1f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private bool jumpPlaySound = false;
    [SerializeField] private AudioClip jumpAudio;

    /*[Header("Weapon")]
    [SerializeField] private bool haveWeapon = false;
    [SerializeField] private KeyCode shotKey = KeyCode.X;
    [SerializeField] private Transform tSpawnBullets;
    [SerializeField] private GameObject pBullet;*/

    void Update() {
        //Walk Code
        axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        isRunning = canRun && Input.GetKey(runKey);
        velocityCurrent = Mathf.SmoothDamp(velocityCurrent, (isRunning ? velocityTargetRunning : velocityTargetWalking) * axis.x, ref refVelocity, velocitySmooth);

        if (Mathf.Abs(axis.x) > 0)
            spriteRenderer.flipX = Input.GetAxis("Horizontal") < 0;

        //Jump Code
        isOnGround = Physics2D.OverlapCircle(transform.position - Vector3.up * (spriteRenderer.bounds.size.y / 2f), groundDetectionRadius, groundMask);
        if (isOnGround && Input.GetKeyDown(jumpKey)) {
            rb.AddForce(Vector2.up * jumpForce * rb.gravityScale * rb.mass, ForceMode2D.Impulse);
            if (jumpPlaySound) {
                AudioManager.Instance.PlaySound(jumpAudio);
            }
        }

        //Weapon Code
        /*if (haveWeapon) {
            if (Input.GetKeyDown(shotKey)) {
                //
            }
        }*/

    }
    private void FixedUpdate() => rb.velocity = new Vector2(velocityCurrent, rb.velocity.y);
    private void LateUpdate() => animator.SetFloat(velocityPropertyInAnimator, Mathf.Abs(velocityCurrent / (isRunning ? velocityTargetRunning : velocityTargetWalking)));
    private void OnTriggerEnter2D(Collider2D collision) {
        if (GameManager.Instance.canGrabCoins && collision.CompareTag(GameManager.Instance.coinTag)) {
            GameManager.Instance.GrabCoin(collision.gameObject);
        }
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position - Vector3.up * (spriteRenderer.bounds.size.y / 2f), groundDetectionRadius);
    }
    public void ApplyDamage(int value) {
        health -= value;
        if (health <= 0) {
            GameManager.Instance.GameOver();
        }
    }
}