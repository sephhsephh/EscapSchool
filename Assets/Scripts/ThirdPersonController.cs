using Unity.Netcode;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ThirdPersonController : NetworkBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 10f;
    public float gravity = 9.8f;
    public float stepHeight = 0.5f;
    public Transform cameraTransform;


    private CharacterController controller;
    private Vector3 moveDirection;
    private float yVelocity;
    private Animator animator;

    private bool isPushing = false;
    private GameObject pushableObject;

   
    public override void OnNetworkSpawn()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (!IsOwner) return;

        // Start the camera assignment coroutine
        StartCoroutine(AssignCamera());

        if (!IsOwner)
        {
            // Disable the character controller and animator for non-owners
            controller.enabled = false;
            animator.enabled = false;
        }
        else
        {
            // Enable the character controller and animator for the owner
            controller.enabled = true;
            animator.enabled = true;
        }
    }



    //void Start()
    //{

    //}

    void Update()
    {
        if (!IsOwner) return;

        if (!IsAnyUIActive())
        {
            MovePlayer();
            HandlePush();
        }
        else
        {
            // Stop movement when UI is active
            animator.SetInteger("AnimKey", 0);
        }
    }



    IEnumerator AssignCamera()
    {
        Camera mainCamera = null;
        float timeout = 1f;
        float elapsedTime = 0f;

        while (mainCamera == null && elapsedTime < timeout)
        {
            mainCamera = Camera.main;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (mainCamera != null)
        {
            cameraTransform = mainCamera.transform;
            SmoothSideScrollCamera cameraScript = mainCamera.GetComponent<SmoothSideScrollCamera>();
            if (cameraScript != null)
            {
                Debug.Log($"Assigning camera to player: {transform.name}");
                cameraScript.AssignPlayer(transform);
            }
        }
    }

    void MovePlayer()
    {
        if (isPushing || cameraTransform == null) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 desiredMove = (forward * vertical + right * horizontal).normalized;

        if (desiredMove.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(desiredMove);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            animator.SetInteger("AnimKey", 1);
        }
        else
        {
            animator.SetInteger("AnimKey", 0);
        }

        // Stair Climbing Logic
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, transform.forward, out hit, 1f))
        {
            // Detect the stair collision and ensure we climb immediately
            if (hit.collider.CompareTag("Stairs") && hit.point.y - transform.position.y <= stepHeight)
            {
                // Adjust the character's Y position to the stair height instantly, preventing delay
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);

                // Allow immediate upward movement when stepping on stairs
                yVelocity = 0.9f;  // Small upward force to simulate climbing
            }
        }

        // Handle gravity
        if (controller.isGrounded)
        {
            yVelocity = -0.5f;  // Small force to ensure the player stays grounded
        }
        else
        {
            yVelocity -= gravity * Time.deltaTime;  // Apply gravity if not grounded
        }

        moveDirection = desiredMove * moveSpeed;
        moveDirection.y = yVelocity;

        // Move the character controller
        controller.Move(moveDirection * Time.deltaTime);
    }

    private bool IsAnyUIActive()
    {
        // Find all GameObjects with the "BlockMovement" tag
        GameObject[] blockingUIs = GameObject.FindGameObjectsWithTag("QuizUI");

        foreach (GameObject ui in blockingUIs)
        {
            Canvas canvas = ui.GetComponent<Canvas>();
            if (canvas != null && canvas.isActiveAndEnabled)
            {
                return true;
            }
        }
        return false;
    }

    void HandlePush()
    {
        if (pushableObject == null) return;

        if (Input.GetKey(KeyCode.E))
        {
            isPushing = true;
            animator.SetInteger("AnimKey", 2);
            pushableObject.transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        else
        {
            isPushing = false;
            animator.SetInteger("AnimKey", 0);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pushable"))
        {
            pushableObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pushable"))
        {
            pushableObject = null;
        }
    }
}