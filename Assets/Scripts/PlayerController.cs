using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

// Gabrielle Toutin
// - Animation blending
// - Control reading

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    public float speed = 1;
    public float rotationSpeed = 10f;

    // these two are needed for the walk blend tree
    private float velx = 0f;
    private float vely = 0f;


    InputAction moveAction;
    InputAction jumpAction;

    private void ResetAllBools()
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(parameter.name, false);
            }
        }
    }

    void Start()
    {
        // references to rigid body and animator
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        if (!animator)
        {
            Debug.LogError("Missing Animator");
        }
        ResetAllBools();
        animator.SetBool("Idle", true);

        // Initialize control system
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        // read move control input
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        // split into h and v for convenience
        float h = moveValue[0];
        float v = moveValue[1];

        // source: Unity tutorial: https://learn.unity.com/pathway/unity-essentials/unit/programming-essentials/tutorial/add-a-movement-script
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = transform.forward * moveVertical * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        float turn = Input.GetAxis("Horizontal") * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);

        animator.SetFloat("velx", h);
        animator.SetFloat("vely", v);

        // Jump
        if (jumpAction.IsPressed())
        {
            animator.SetTrigger("Jump");
        }
    }

}

