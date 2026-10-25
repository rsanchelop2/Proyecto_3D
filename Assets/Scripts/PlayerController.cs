using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    private Transform cameraTransform;

    [Header("Parámetros de Movimiento")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jump = 8f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Detección de Suelo y Caída")]
    [SerializeField] private float groundCheckDistance = 0.3f;
    [SerializeField] private float fallThreshold = -0.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // Referencia a la cámara principal activa para el movimiento relativo
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        float inputHorizontal = Input.GetAxisRaw("Horizontal");
        float inputVertical = Input.GetAxisRaw("Vertical");

        // --- DETECCIÓN DE SUELO ---
        // Elevamos un poco el origen del Raycast para no chocar con los colisionadores de las piernas
        Vector3 rayOrigin = transform.position + Vector3.up * 0.2f;
        bool isGrounded = Physics.Raycast(rayOrigin, Vector3.down, groundCheckDistance + 0.2f);

        // Línea visual en la pestaña Scene (Verde = en suelo, Rojo = en el aire)
        Debug.DrawRay(rayOrigin, Vector3.down * (groundCheckDistance + 0.2f), isGrounded ? Color.green : Color.red);

        // Detección de estado de caída (en el aire y descendiendo)
        bool isFalling = !isGrounded && rb.velocity.y < fallThreshold;

        // Controles de movimiento
        bool hasInput = (Mathf.Abs(inputHorizontal) > 0.1f || Mathf.Abs(inputVertical) > 0.1f);
        bool isMovingOnGround = hasInput && isGrounded;

        // --- CÁLCULO DE MOVIMIENTO RELATIVO A LA CÁMARA ---
        Vector3 moveDirection = Vector3.zero;

        if (cameraTransform != null)
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0f;
            camRight.y = 0f;

            camForward.Normalize();
            camRight.Normalize();

            moveDirection = camForward * inputVertical + camRight * inputHorizontal;
        }
        else
        {
            moveDirection = new Vector3(inputHorizontal, 0f, inputVertical);
        }

        Vector3 targetVelocity = moveDirection * speed;
        rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);

        // --- LÓGICA DE SALTO ---
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jump, rb.velocity.z);

            if (animator != null)
            {
                // Obtenemos la dirección local en la que se mueve el personaje
                Vector3 localVel = transform.InverseTransformDirection(rb.velocity);

                if (!hasInput)
                {
                    animator.SetTrigger("JumpIdle");
                }
                else if (localVel.z < -0.5f)
                {
                    animator.SetTrigger("JumpBackward");
                }
                else
                {
                    animator.SetTrigger("JumpRun");
                }
            }
        }

        // --- ROTACIÓN SUAVE DEL PERSONAJE ---
        if (hasInput && inputVertical >= 0)
        {
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        // --- ENVIAR DATOS AL ANIMATOR ---
        if (animator != null)
        {
            Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);

            animator.SetBool("isRunning", isMovingOnGround);
            animator.SetBool("isFalling", isFalling);
            animator.SetFloat("InputX", localVelocity.x / speed);
            animator.SetFloat("InputZ", localVelocity.z / speed);
        }

        // --- MENSAJE DE DEPURACIÓN EN CONSOLA ---
        if (!isGrounded)
        {
            Debug.Log($"En el Aire | Vel Y: {rb.velocity.y:F2} | isFalling: {isFalling}");
        }
    }
}