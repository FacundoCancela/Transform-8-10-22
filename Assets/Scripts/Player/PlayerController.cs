using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private Animator playerAnimator;

    private bool isMoving = false;
    private bool isJumping = false;
    private bool isGrounded = true;

    private float horizontalMoveDir = 0;

    [Header("Modifiers")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    public static PlayerController instance; 

    //################ #################
    //------------UNITY F--------------
    //################ #################
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
        ListenForMoveInput();
        ListenForJumpInput();
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.25f);
    }

    //################ #################
    //----------CLASS METHODS-----------
    //################ #################

    //----------------------INPUT------------------

    //---------------
    //-----MOVE------
    private void ListenForMoveInput()
    {
        horizontalMoveDir = Input.GetAxisRaw("Horizontal");

        if (horizontalMoveDir != 0)
        {
            isMoving = true;
            if (horizontalMoveDir < 0) transform.rotation = Quaternion.Euler(0, 180, 0);
            else if (horizontalMoveDir > 0) transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else isMoving = false;
    }
    private void MovePlayer()
    {
        playerAnimator.SetBool("isRunning", isMoving);
        playerRb.velocity = new Vector2(horizontalMoveDir * moveSpeed * Time.deltaTime, playerRb.velocity.y);
    }

    //---------------
    //-----JUMP------
    private bool CheckIfPlayerIsGrounded()
    {
        //CirclCast para que agarre bien las esquinas.
        int targetLayer = LayerMask.GetMask("Ground");
        int targetLayer2 = LayerMask.GetMask("Enemy");
        isGrounded = Physics2D.CircleCast(transform.position, 0.25f, Vector2.down, 0.2f, targetLayer | targetLayer2);
        return isGrounded;
    }
    private void ListenForJumpInput()
    {
        isGrounded = CheckIfPlayerIsGrounded();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
            isGrounded = false;
            AudioManager.instance.PlaySFX("FormiJumpSFX");
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        //Aterrizaje
        else if (isJumping && isGrounded && playerRb.velocity.y <= 0)
        {
            OnJumpLand();
        }

        playerAnimator.SetBool("isJumping", isJumping);
    }
    private void OnJumpLand()
    {
        isJumping = false;
        playerAnimator.SetTrigger("playJumpLand");
        AudioManager.instance.PlaySFX("FormiLandSFX");
    }
}
