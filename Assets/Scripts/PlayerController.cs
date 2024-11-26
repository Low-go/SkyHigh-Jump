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

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        jumpCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        //moveDirection = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, moveDirection.y, Input.GetAxis("Vertical") * moveSpeed);

        if (knockBackCounter <= 0) // only move if not knocked back
        {
            float yStore = moveDirection.y; // this shuld fix the jumping issues
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
                if(Input.GetButtonDown("Jump") && jumpCount > 0)
                {
                    moveDirection.y = jumpForce -3;
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
