using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    private static readonly int hashMove = Animator.StringToHash("Move");

    public float moveSpeed = 5f;

    private PlayerInput input;
    private Animator animator;

    private Rigidbody rb;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
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
    }
}
