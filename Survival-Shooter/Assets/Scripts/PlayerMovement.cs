using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    private static readonly int hashMove = Animator.StringToHash("Move");

    public float moveSpeed = 5f;

    private PlayerInput input;
    private Animator animator;

    private Rigidbody rb;

    public Camera mainCamera;

    private PlayerHealth playerHealth;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
       
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }


    private void FixedUpdate()
    {
        if (playerHealth.IsDead)
            return;


        Vector3 movement = new Vector3(input.MoveHorizontal, 0f, input.MoveVertical);

        rb.linearVelocity = movement * moveSpeed;

        float move = Math.Abs(input.MoveVertical + input.MoveHorizontal);

        if ((input.MoveVertical > 0f && input.MoveHorizontal < 0f) || (input.MoveVertical < 0f && input.MoveHorizontal > 0f))
        {
            animator.SetFloat(hashMove, 1);
        }
        else
        {
            animator.SetFloat(hashMove, move);
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            Vector3 targetPosition = hit.point;

            Vector3 direction = targetPosition - transform.position;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                Quaternion lookRatition = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Euler(0f, lookRatition.eulerAngles.y, 0f);
            }
        }
    }
}
