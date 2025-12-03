using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour // class declaration

{
    public float turnSpeed = 20f; // This is to change turn speed.
    public float walkspeed = 1f;
    public float boostMultiplier = 3f;
    public float maxSprintTime = 5f;
    private float sprintTimer;
    public Text boostText;
    Animator m_Animator; // This is used to access the Animator component, and will be used throughtout the whole class.

    Rigidbody m_Rigidbody;

    Vector3 m_Movement; // This creates a varible that has a CLASS scope, meaning it can be used anywhere in the public class.
                        // the "m_" is a naming convention for MEMBER variables, which represent varibles that are used fo
    Quaternion m_Rotation = Quaternion.identity; // Quaternions store rotations better than Vectors.







    // Start is called before the first frame update
    void Start() // This is a method. Think of it like a machine inside a factory (Factory is the class)
    {
        m_Animator = GetComponent<Animator>(); // GetComponent is part of the MonoBehavior, that's why it isn't a class.
        // The code is the method declaration, which is how a method works. Think of it as what the factory machine does.
        // Calling a method is activiating it to do a task.

        m_Rigidbody = GetComponent<Rigidbody>();
        sprintTimer = maxSprintTime;
        if (boostText != null)
        {
            boostText.gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
      
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            m_Movement.Set(horizontal, 0f, vertical);
            m_Movement.Normalize();

            bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
            bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
            bool isWalking = hasHorizontalInput || hasVerticalInput;

            m_Animator.SetBool("IsWalking", isWalking);

            Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
            m_Rotation = Quaternion.LookRotation(desiredForward);

            // --- SHIFT SPEED BOOST ---
            if (Input.GetKey(KeyCode.LeftShift) && sprintTimer > 0f && isWalking)
            {
                walkspeed = 1f * boostMultiplier;  // Sprint speed
                sprintTimer -= Time.deltaTime; // countdown
                if (boostText != null)
                {
                    boostText.gameObject.SetActive(true);
                    
                }
            }
            else
            {
                walkspeed = 1f; // Normal speed
                if (boostText != null)
                {
                    boostText.gameObject.SetActive(false);
                }
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                sprintTimer = maxSprintTime; // reset sprint time
            }

        }


    }

    private void OnAnimatorMove()
    {
       
    {
            Vector3 newPosition = m_Rigidbody.position + m_Movement * walkspeed * Time.deltaTime;
            m_Rigidbody.MovePosition(newPosition);
            m_Rigidbody.MoveRotation(m_Rotation);
        }

}
}
