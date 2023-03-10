using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D theRB;
    public float moveSpeed, JumpForce;

    private float velocity;

    private bool isGrounded;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;

    public Animator anim;

    public bool isKeyboard2;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.AddPlayer(this);
    }

    // Update is called once per frame
    void Update()
    {

        if (isKeyboard2)
        {
            velocity = 0f;

            if (Keyboard.current.lKey.isPressed)
            {
                velocity += 1f;
            }

            if (Keyboard.current.jKey.isPressed)
            {
                velocity = -1f;
            }

            if (isGrounded && Keyboard.current.iKey.wasPressedThisFrame)
            {
                theRB.velocity = new Vector2(theRB.velocity.x, JumpForce);
            }

            if (!isGrounded && Keyboard.current.iKey.wasReleasedThisFrame && theRB.velocity.y > 0)
            {
                theRB.velocity = new Vector2(theRB.velocity.x, theRB.velocity.y * .5f);
            }

        }

        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, .2f, whatIsGround);

        theRB.velocity = new Vector2(velocity * moveSpeed, theRB.velocity.y);

        /* if(Input.GetButtonDown("Jump"))
        {
            theRB.velocity = new Vector2(theRB.velocity.x, JumpForce);
        } */

        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("ySpeed", theRB.velocity.y);
        anim.SetFloat("speed", Mathf.Abs(theRB.velocity.x));

        if (theRB.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        } else if (theRB.velocity.x > 0)
        {
            transform.localScale = Vector3.one;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        velocity = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded)
        {
            theRB.velocity = new Vector2(theRB.velocity.x, JumpForce);
        }

        if (!isGrounded && context.canceled && theRB.velocity.y > 0f)
        {
            theRB.velocity = new Vector2(theRB.velocity.x, theRB.velocity.y * .5f);
        }
    }
}
