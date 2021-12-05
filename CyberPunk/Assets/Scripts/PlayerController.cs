using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed, gravityModifier, jumpPower, runSpeed = 12.0f;
    public CharacterController charCon;

    private Vector3 moveInput;

    public Transform camTransform;
    public float mouseSensitivity;
    public bool invertX, invertY;

    private bool canJump, canDoubleJump;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;

    public Animator animator;

    public GameObject bullet;
    public Transform firePoint;
    void Start()
    {
        
    }
    
    void Update()
    {
        // moveInput.x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime; 
        // moveInput.z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        // store y velocity
        float yStore = moveInput.y;
        
        Vector3 vertMove = transform.forward * Input.GetAxis("Vertical");
        Vector3 horiMove = transform.right * Input.GetAxis("Horizontal");

        moveInput = horiMove + vertMove;
        moveInput.Normalize();

        if (Input.GetKey(KeyCode.LeftShift) && !canDoubleJump && canJump)
            moveInput = moveInput * runSpeed;
        else
            moveInput = moveInput * moveSpeed;
        
        moveInput.y = yStore;
        
        moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;
        
        if (charCon.isGrounded)
        {
            moveInput.y = Physics.gravity.y * gravityModifier * Time.deltaTime;
        }

        canJump = Physics.OverlapSphere(groundCheckPoint.position, 0.10f, whatIsGround).Length > 0;
        
        if (canJump)
        {
            canDoubleJump = false;
        }
        // Handle Jumping
        
        // Double Jump not working
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            moveInput.y = jumpPower;
            
            canDoubleJump = true;
        } else if (canDoubleJump && Input.GetKeyDown(KeyCode.Space))
        {
            moveInput.y = jumpPower;
            canDoubleJump = false;
        }
        charCon.Move(moveInput * Time.deltaTime);
        
        // control camera rotation
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y") * mouseSensitivity);

        if (invertX)
            mouseInput.x = -mouseInput.x;
        
        if (invertY)
            mouseInput.y = -mouseInput.y;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);
        camTransform.rotation = Quaternion.Euler(camTransform.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));

        //handle shooting
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if(Physics.Raycast(camTransform.position, camTransform.forward, out hit, 50f))  //50f is the distance they Raycast goes, change it accordingly
            {
                if(Vector3.Distance(camTransform.position, hit.point) > 2f)
                {
                    firePoint.LookAt(hit.point);
                }
            }
            else
            {
                firePoint.LookAt(camTransform.position + (camTransform.forward * 30f));
            }


            Instantiate(bullet, firePoint.position, firePoint.rotation);
        }

        animator.SetFloat("moveSpeed", moveInput.magnitude);
        animator.SetBool("onGround", canJump);

        
    }
}
