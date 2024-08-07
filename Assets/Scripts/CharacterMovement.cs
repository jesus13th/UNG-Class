using System.Collections;

using UnityEngine;

public class CharacterMovement : MonoBehaviour {
    [Header("Componentes")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Health")]
    [SerializeField] private int health = 5;
    [SerializeField] private string damageParameter;
    [SerializeField] private string isdeadParameter;

    [Header("Velocity")]
    [SerializeField] private float velocityTargetWalking = 4;
    private float velocityCurrent;
    [SerializeField] private bool canRun = false;
    [ShowIf("canRun"), SerializeField] private KeyCode runKey = KeyCode.LeftShift;
    [ShowIf("canRun"), SerializeField] private bool isRunning = false;
    [ShowIf("canRun"), SerializeField] private float velocityTargetRunning = 8;
    [ShowIf("canRun"), SerializeField] private string isRunningParameter;
    [SerializeField] private float velocitySmooth = 0.25f;
    [SerializeField] private string velocityParameter = "Velocity";
    private float refVelocity;
    private Vector2 axis;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 4;
    private bool isOnGround = false;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool canDoubleJump;
    private bool doubleJump;
    [SerializeField] private float groundDetectionRadius = 0.1f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private AudioClip jumpAudio;
    [ShowIf("canRun"), SerializeField] private string jumpParameter;

    [Header("Weapon")]
    [SerializeField] private bool haveWeapon = false;
    [ShowIf("haveWeapon"), SerializeField] private KeyCode shotKey = KeyCode.X;
    [ShowIf("haveWeapon"), SerializeField] private Transform tSpawnBullets;
    private float spawnBulletsX;
    [ShowIf("haveWeapon"), SerializeField] private GameObject pBullet;
    [ShowIf("haveWeapon"), SerializeField] private float bulletForce;
    [ShowIf("haveWeapon"), SerializeField] private float lifeTime = 3f;
    private float timer;
    [ShowIf("haveWeapon"), SerializeField] private float delay;
    [ShowIf("haveWeapon"), SerializeField] private string shotParameter;
    [ShowIf("haveWeapon"), SerializeField] private AudioClip shotAudio;

    private void Start() {
        HealthSystem.Instance.CurrentHealth = health;
        spawnBulletsX = tSpawnBullets.position.x;
    }
    void Update() {
        //Walk Code
        axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        isRunning = canRun && Input.GetKey(runKey);
        velocityCurrent = Mathf.SmoothDamp(velocityCurrent, (isRunning ? velocityTargetRunning : velocityTargetWalking) * axis.x, ref refVelocity, velocitySmooth);

        if (Mathf.Abs(axis.x) > 0) {
            spriteRenderer.flipX = Input.GetAxis("Horizontal") < 0;
            tSpawnBullets.position = new Vector3(transform.position.x + spawnBulletsX * (spriteRenderer.flipX ? -1 : 1), tSpawnBullets.position.y, tSpawnBullets.position.z);
        }

        //Jump Code
        isOnGround = Physics2D.OverlapCircle(transform.position + offset - Vector3.up * (spriteRenderer.bounds.size.y / 2f), groundDetectionRadius, groundMask);
        if ((isOnGround || doubleJump && canDoubleJump) && Input.GetKeyDown(jumpKey)) {
            if (!isOnGround && canDoubleJump) {
                doubleJump = false;
            }
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce * rb.gravityScale * rb.mass, ForceMode2D.Impulse);
            setTriggerParameter(jumpParameter);
            if (jumpAudio) {
                AudioManager.Instance.PlaySound(jumpAudio);
            }
        }
        if (isOnGround && canDoubleJump) {
            doubleJump = true;
        }

        //Weapon Code
        if (haveWeapon) {
            timer += Time.deltaTime;
            if (Input.GetKeyDown(shotKey) && timer > delay) {
                setTriggerParameter(shotParameter);
                var bullet = Instantiate(pBullet, tSpawnBullets.position, tSpawnBullets.rotation);
                bullet.GetComponent<Rigidbody2D>().AddForce(transform.right * bulletForce * (spriteRenderer.flipX ? -1 : 1));
                Destroy(bullet, lifeTime);
                timer = 0;
                if (shotAudio) {
                    AudioManager.Instance.PlaySound(shotAudio);
                }
            }
        }
    }
    private void FixedUpdate() => rb.velocity = new Vector2(velocityCurrent, rb.velocity.y);
    private void LateUpdate() {
        setFloatParameter(velocityParameter, Mathf.Abs(velocityCurrent / (isRunning ? velocityTargetRunning : velocityTargetWalking)));
        setBoolParameter(velocityParameter, isRunning);
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (GameManager.Instance.canGrabCoins && collision.CompareTag(GameManager.Instance.coinTag)) {
            GameManager.Instance.GrabCoin(collision.gameObject);
        }
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + offset - Vector3.up * (spriteRenderer.bounds.size.y / 2f), groundDetectionRadius);
    }
    public void ApplyDamage(int value) {
        health -= value;
        setTriggerParameter(damageParameter);
        HealthSystem.Instance.CurrentHealth = health;
        if (health <= 0)
            StartCoroutine(DeadCoroutine());
    }
    private IEnumerator DeadCoroutine() {
        setTriggerParameter(isdeadParameter);
        yield return new WaitForSeconds(2f);
        GameManager.Instance.GameOver();
    }
    private void setFloatParameter(string parameter, float value) {
        if (!string.IsNullOrEmpty(parameter)) {
            animator.SetFloat(parameter, value);
        }
    }
    private void setBoolParameter(string parameter, bool value) {
        if (!string.IsNullOrEmpty(parameter)) {
            animator.SetBool(parameter, value);
        }
    }
    private void setTriggerParameter(string parameter) {
        if (!string.IsNullOrEmpty(parameter)) {
            animator.SetTrigger(parameter);
        }
    }
}