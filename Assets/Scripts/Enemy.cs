using System.Collections;
using System.Linq;

using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Health")]
    [SerializeField] private float health = 1f;
    [SerializeField] private string characterTag;
    [SerializeField] private string bulletTag;

    private float velocityCurrent;
    [SerializeField] private float velocityTarget = 2.5f;
    [SerializeField] private float velocitySmooth = 0.02f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool keepOnEdge = true;
    [SerializeField] private float angle = 60f;
    [SerializeField] private float distance = 2f;
    [SerializeField] private float sideLimit = 0.75f;

    private void Update() {
        velocityCurrent = Mathf.MoveTowards(velocityCurrent, velocityTarget, velocitySmooth);
        spriteRenderer.flipX = velocityTarget < 0;
    }
    private void FixedUpdate() {
        if (keepOnEdge) {
            if (Physics2D.Raycast(transform.position, Quaternion.Euler(0, 0, -angle) * Vector3.down, distance, groundLayer).collider == null) {
                velocityTarget = 2;
            }
            if (Physics2D.Raycast(transform.position, Quaternion.Euler(0, 0, angle) * Vector3.down, distance, groundLayer).collider == null) {
                velocityTarget = -2;
            }
        }
        if (Physics2D.Raycast(transform.position, Vector3.right, sideLimit, groundLayer).collider != null) {
            velocityTarget = -2;
        }
        if (Physics2D.Raycast(transform.position, Vector3.left, sideLimit, groundLayer).collider != null) {
            velocityTarget = 2;
        }

        rb.velocity = new Vector2(velocityCurrent, rb.velocity.y);
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag(characterTag)) {
            if (canDamagePlayer && Mathf.Abs(collision.contacts.First().normal.x) > 0.8f || collision.contacts.First().normal.y > 0.8f) {
                StartCoroutine(DamagePlayer(collision));
            } else if (collision.contacts.First().normal.y < -0.8f) {
                ApplyDamage(1);
            }
        }
        if (!string.IsNullOrEmpty(bulletTag) && collision.collider.CompareTag(bulletTag)) {
            ApplyDamage(1);
            Destroy(collision.gameObject);
        }
    }
    private bool canDamagePlayer = true;
    private IEnumerator DamagePlayer(Collision2D collision) {
        collision.gameObject.GetComponent<CharacterMovement>().ApplyDamage(1);
        velocityTarget *= -1;
        canDamagePlayer = false;
        yield return new WaitForSeconds(2f);
        canDamagePlayer = true;
    }
    protected void ApplyDamage(float value) {
        health -= value;
        if (health <= 0) {
            Destroy(gameObject);
        }
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + (Quaternion.Euler(0, 0, -angle) * Vector3.down * distance));
        Gizmos.DrawLine(transform.position, transform.position + (Quaternion.Euler(0, 0, angle) * Vector3.down * distance));
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * sideLimit);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * sideLimit);
    }
}