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

    // --- Changed ---
    private Rigidbody2D rb;
    private Vector2 moveDir; // Store input for FixedUpdate

    // --- Changed ---
    void Start()
    {
        // Get the Rigidbody2D component on this player
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // --- Changed ---
        // Get input in Update() for responsiveness, but store it for FixedUpdate
        moveDir = Vector2.zero; // Use Vector2 for 2D

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            moveDir += Vector2.left;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            moveDir += Vector2.right;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            moveDir += Vector2.up;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            moveDir += Vector2.down;

        // Normalize to prevent faster diagonal movement
        moveDir.Normalize();

        // These are fine in Update()
        HandleRotation();
        HandleShooting();
    }

    // --- Changed ---
    void FixedUpdate()
    {
        // Apply physics movement in FixedUpdate
        HandleMovement();
    }

    void HandleMovement()
    {
        // --- Changed ---
        // This is the FIX: Use rb.MovePosition() for kinematic collisions
        // Use Time.fixedDeltaTime inside FixedUpdate
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
            Shoot();
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
    }
}