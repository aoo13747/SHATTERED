using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region OldCode
    /*
    public float movementSpeed = 10f;
    public float jumpForce = 50f;

    bool isJumping = false;
    bool isCrouch = false;
    bool isGrounded = false;
    bool isDead = false;

    bool isFaceRight = true;

    public float horizontalMovement;

    public int health = 100;
    public int currentPlayerHP;

    public GameObject deadUI;

    public HealthBar healthBar;

    Animator anim;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();        
        anim = GetComponent<Animator>();

        currentPlayerHP = health;
        healthBar.SetMaxHealth(health);

        deadUI.SetActive(false);

        anim.SetBool("IsDead", isDead);
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMovement = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.K))
        {
            if (isGrounded == true)
            {
                isJumping = true;
                isGrounded = false;
                anim.SetTrigger("Jump");
            }
            else
            {
                return;
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            if (isCrouch == false && isGrounded == true)
            {
                isCrouch = true;
                anim.SetTrigger("Crouch");
                anim.SetBool("IsCrouch", isCrouch);
            }
            else
            {
                return;
            }
        }

        anim.SetBool("IsGround", isGrounded);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalMovement * movementSpeed * Time.deltaTime, rb.velocity.y);
        Flip(horizontalMovement);
        anim.SetFloat("Horizontal", Mathf.Abs(horizontalMovement));

        if (isJumping == true)
        {
            rb.AddForce(new Vector2(0.0f, jumpForce));
            isJumping = false;
        }
        if (isCrouch == true)
        {
            isCrouch = false;
            anim.SetBool("IsCrouch", isCrouch);
        }
    }

    private void Flip(float horizontalValue)
    {
        if (horizontalValue < 0 && isFaceRight || horizontalValue > 0 && !isFaceRight)
        {
            isFaceRight = !isFaceRight;
            transform.Rotate(0f, 180f, 0f);            
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentPlayerHP > 0)
        {
            currentPlayerHP -= damage;
            healthBar.SetHealth(currentPlayerHP);
            anim.SetTrigger("Hurt");
        }

        if (currentPlayerHP <= 0)
        {
            isDead = true;
            anim.SetBool("IsDead", isDead);
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        this.enabled = false;
        yield return new WaitForSeconds(1);
        deadUI.SetActive(true);
    }
    */
    #endregion

    public float movementSpeed = 10f;
    public float jumpForce = 50f;
    public float horizontalMovement;

    bool isJumping = false;
    bool isCrouch = false;
    bool isGrounded = false;
    bool isDead = false;
    bool isFaceRight = true;
    bool isRun = false;
    
    
    public int health = 100;
    public int currentPlayerHP;
    private string currentAnimation;    

    public GameObject deadUI;
    public HealthBar healthBar;
    private Animator animator;
    private Rigidbody2D rb;

    //Animation State
    const string Yuno_IDLE = "Yuno_Idle";
    const string Yuno_CROUCH_PISTOL = "Yuno_Crouch_pistol";
    const string Yuno_HURT = "Yuno_Hurt";
    const string Yuno_JUMP = "Yuno_Jump";    
    const string Yuno_RUN = "Yuno_Run";

    
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentPlayerHP = health;
        healthBar.SetMaxHealth(health);
        deadUI.SetActive(false);        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMovement = Input.GetAxis("Horizontal");

        //=====================================JUMP================================================================//
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (isGrounded == true)
            {
                isJumping = true;
                isGrounded = false;

                if (isJumping == true)
                {
                    rb.AddForce(new Vector2(0.0f, jumpForce));

                    isJumping = false;
                }
                if (isGrounded == false)
                {
                    ChangeAnimationState(Yuno_JUMP);
                }


            }
            else
            {
                return;
            }
        }

        //==================================CROUCH================================================================//
        if (Input.GetKey(KeyCode.S))
        {
            if (isCrouch == false && isGrounded == true)
            {
                ChangeAnimationState(Yuno_CROUCH_PISTOL);
                isCrouch = true;
            }
            else
            {
                return;
            }
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            isCrouch = false;
            if (isRun == false)
                ChangeAnimationState(Yuno_IDLE);
            if (isRun == true)
                ChangeAnimationState(Yuno_RUN);
        }
        
    }

    private void FixedUpdate()
    {        
        if (isCrouch == false)
        {
            rb.velocity = new Vector2(horizontalMovement * movementSpeed * Time.deltaTime, rb.velocity.y);
        }
        if (isGrounded == true && isCrouch == false)
        {
            if (horizontalMovement > 0 || horizontalMovement < 0)
            {
                isRun = true;
                ChangeAnimationState(Yuno_RUN);
            }
            else
            {
                isRun = false;
                ChangeAnimationState(Yuno_IDLE);
            }
        }
        Flip(horizontalMovement);
    }
    
    private void HurtComplete()
    {
        if (isGrounded == true && isCrouch == false)
        {
            if (horizontalMovement > 0 || horizontalMovement < 0)
            {
                isRun = true;
                ChangeAnimationState(Yuno_RUN);
            }
            else
            {
                isRun = false;
                ChangeAnimationState(Yuno_IDLE);
            }
        }
        if(isCrouch == true || Input.GetKeyDown(KeyCode.S))
        {
            ChangeAnimationState(Yuno_CROUCH_PISTOL);
        }
        if(isJumping == true)
        {
            ChangeAnimationState(Yuno_JUMP);
        }        
    }
    
    private void Flip(float horizontalValue)
    {
        if (horizontalValue < 0 && isFaceRight || horizontalValue > 0 && !isFaceRight)
        {
            isFaceRight = !isFaceRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentPlayerHP > 0)
        {
            currentPlayerHP -= damage;
            healthBar.SetHealth(currentPlayerHP);
            ChangeAnimationState(Yuno_HURT);
            Invoke("HurtComplete", 0.1f); 
        }

        if (currentPlayerHP <= 0)
        {
            isDead = true;            
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(1);
        deadUI.SetActive(true);
        Time.timeScale = 0;
    }
    void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimation == newAnimation) 
            return;
        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
        ChangeAnimationState(Yuno_IDLE);
    }
}
