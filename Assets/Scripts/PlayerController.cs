using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public CharacterController controller;
    public Animator anim;
    private Vector3 moveDirection;
    public float gravityScale;
    public Transform pivet;
    public float rotateSpeed;
    public GameObject playerModel;
    public float knockBackForce;
    public float knockBackTime;
    private float knockBackCounter;
    private int jumpCount;

    // Fall damage variables
    public float fallDamageThreshold = 9f; // Damage starts at 9 meters
    private float fallStartHeight;
    private bool isFalling;

    public bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        jumpCount = 2;
        fallStartHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            moveDirection = Vector3.zero;
            return;
        }
      
        // Detect falling
        if (!controller.isGrounded && moveDirection.y < 0)
        {
            if (!isFalling)
            {
                isFalling = true;
                fallStartHeight = transform.position.y;
                Debug.Log("[Fall Tracking] Started falling from height: " + fallStartHeight);
            }
        }
        else if (controller.isGrounded)
        {
            if (isFalling)
            {
                float fallDistance = fallStartHeight - transform.position.y;
                Debug.Log("[Fall Tracking] Landed. Fall distance: " + fallDistance);

                if (fallDistance > fallDamageThreshold)
                {
                    Debug.Log("[Fall Damage] Took 1 damage from fall");

                    // Apply fall damage
                    Vector3 hitDirection = Vector3.down; // Fall damage comes from below
                    FindObjectOfType<HealthManager>().hurtPlayer(1, hitDirection);
                }

                isFalling = false;
            }
        }

        if (knockBackCounter <= 0) // only move if not knocked back
        {
            float yStore = moveDirection.y; // this should fix the jumping issues
            moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
            moveDirection = moveDirection.normalized * moveSpeed;
            moveDirection.y = yStore;

            // jump allowed if player on ground 
            if (controller.isGrounded)
            {
                jumpCount = 2; // reset jumpcounter
                moveDirection.y = 0f;
                if (Input.GetButtonDown("Jump"))
                {
                    moveDirection.y = jumpForce;
                    jumpCount--;
                }
            }
            else
            {
                if (Input.GetButtonDown("Jump") && jumpCount > 0)
                {
                    moveDirection.y = jumpForce - 3;
                    jumpCount--;
                }
            }
        }
        //lower knockbackCounter 
        else
        {
            knockBackCounter -= Time.deltaTime;
        }
        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime); // this is to add gravity after a jump
        controller.Move(moveDirection * Time.deltaTime);
        //Move the player in different directions based on camera look direction
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            transform.rotation = Quaternion.Euler(0f, pivet.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }
        //anim.SetBool("isGrounded", controller.isGrounded);
        anim.SetFloat("Speed", (Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal"))));
    }
    public void knockBack(Vector3 direction)
    {
        knockBackCounter = knockBackTime;
        moveDirection = direction * knockBackForce;
        moveDirection.y = knockBackForce;
    }
}
//TODO fix strange slight right drift
//TODO fix animation jump issues