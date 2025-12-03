using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // ? NEW: needed for Text

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;
    public float walkspeed = 1f;
    public float boostMultiplier = 3f;

    // ? NEW: sprint timer
    public float maxSprintTime = 5f;
    private float sprintTimer;

    // ? NEW: UI text to show when boost is active
    public Text boostText;

    Animator m_Animator;
    Rigidbody m_Rigidbody;

    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();

        sprintTimer = maxSprintTime;

        // start with the text hidden
        if (boostText != null)
        {
            boostText.enabled = false;
            boostText.text = "BOOST ACTIVE";
        }
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;

        m_Animator.SetBool("IsWalking", isWalking);

        Vector3 desiredForward = Vector3.RotateTowards(
            transform.forward,
            m_Movement,
            turnSpeed * Time.deltaTime,
            0f
        );
        m_Rotation = Quaternion.LookRotation(desiredForward);

        // -------- SPRINT LOGIC (Shift, 5s max) --------
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && sprintTimer > 0f && isWalking;

        if (isSprinting)
        {
            walkspeed = boostMultiplier;
            sprintTimer -= Time.deltaTime;
        }
        else
        {
            walkspeed = 1f;
        }

        // reset timer when Shift is released
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            sprintTimer = maxSprintTime;
        }

        // -------- TEXT DISPLAY --------
        if (boostText != null)
        {
            boostText.enabled = isSprinting;   // only visible while sprinting
        }
    }

    private void OnAnimatorMove()
    {
        // use walkspeed to scale movement
        m_Rigidbody.MovePosition(
            m_Rigidbody.position +
            m_Movement * m_Animator.deltaPosition.magnitude * walkspeed
        );
        m_Rigidbody.MoveRotation(m_Rotation);
    }
}
