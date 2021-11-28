using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed, gravityModifier, jumpPower;
    public CharacterController charCon;

    private Vector3 moveInput;

    public Transform camTransform;
    public float mouseSensitivity;
    public bool invertX, invertY;

    private bool canJump, canDoubleJump;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;
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
        moveInput = moveInput * moveSpeed;
        
        moveInput.y = yStore;
        
        moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;
        
        if (charCon.isGrounded)
        {
            moveInput.y = Physics.gravity.y * gravityModifier * Time.deltaTime;
        }

        canJump = Physics.OverlapSphere(groundCheckPoint.position, 0.25f, whatIsGround).Length > 0;
        
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
            Debug.Log("Can Double Jump");
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
    }
}
