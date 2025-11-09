using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotationSpeed = 10f; // controls smoothness
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 15f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private AudioSource shootAudio; // Plays shooting sound
    [SerializeField] private float shootAnimDuration = 0.15f; // how long the shooting anim flag stays true

    [Header("Audio Pitch Randomization")]
    [SerializeField, Range(0.8f, 1.2f)] private float minPitch = 0.95f; // min random pitch
    [SerializeField, Range(0.8f, 1.2f)] private float maxPitch = 1.05f; // max random pitch

    private Rigidbody2D rb;
    private Vector2 moveDir;
    private Animator animator;
    private bool isShoot = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (shootAudio == null)
            shootAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        moveDir = Vector2.zero;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            moveDir += Vector2.left;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            moveDir += Vector2.right;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            moveDir += Vector2.up;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            moveDir += Vector2.down;

        moveDir.Normalize();

        HandleRotation();
        HandleShooting();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
    }

    void HandleRotation()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position);
        direction.Normalize();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isShoot = true;
            Shoot();
            StartCoroutine(ResetShootAnimation());
        }
    }

    void Shoot()
    {
        if (!bulletPrefab) return;

        Vector3 spawnPos = firePoint ? firePoint.position : transform.position;
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, firePoint ? firePoint.rotation : transform.rotation);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 0;
            rb.velocity = (firePoint ? firePoint.up : transform.up) * bulletSpeed;
        }

        // Play shoot sound with random pitch variation
        if (shootAudio != null)
        {
            shootAudio.pitch = Random.Range(minPitch, maxPitch);
            shootAudio.Play();
        }

        // Trigger animation
        if (animator != null)
            animator.SetBool("isShoot", true);
    }

    IEnumerator ResetShootAnimation()
    {
        yield return new WaitForSeconds(shootAnimDuration);
        isShoot = false;

        if (animator != null)
            animator.SetBool("isShoot", false);
    }
}
