using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPistol : MonoBehaviour
{
    public Transform firePoint;
    public Transform firePointCrouch;
    public Transform firePointJump;
    public Transform firePointRun;
    public GameObject bulletTracer;

    public Collider2D groundCheck;
    bool isGrounded = true;

    public Animator playerAnimator;
    public AudioClip pistolGunSound;
    public AudioClip pistolGunReload;
    public AudioSource playerAudioSource;

    private float horizontalMovement;

    public float pistolFirerate = 2.0f;
    public float nextTimeToFire = 0f;

    public int maxWeaponAmmo = 18;
    public int currentWeaponAmmo;
    public float weaponReloadTime = 1.5f;
    private bool isReloading = false;
    static Text ammoText;

    //Animation State
    const string Yuno_IDLE = "Yuno_Idle";
    const string Yuno_CROUCH_PISTOL = "Yuno_Crouch_pistol";
    const string Yuno_CROUCH_PISTOL_RELOAD = "Yuno_Crouch_pistol_reload";
    const string Yuno_CROUCH_PISTOL_SHOOT = "Yuno_Crouch_pistol_shoot";
    const string Yuno_JUMP_PISTOL_RELOAD = "Yuno_Jump_pistol_reload";
    const string Yuno_JUMP_PISTOL_SHOOT = "Yuno_Jump_pistol_shoot";
    const string Yuno_JUMP = "Yuno_Jump";
    const string Yuno_PISTOL_RELOAD = "Yuno_Pistol_reload";
    const string Yuno_PISTOL_SHOOT = "Yuno_Pistol_shoot";
    const string Yuno_RUN = "Yuno_Run";
    const string Yuno_RUN_PISTOL_SHOOT = "Yuno_Run_pistol_shoot";
    const string YUNO_RUN_PISTOL_RELOAD = "Yuno_Run_pistol_reload";

    private string currentAnimation;    
    private float shootingDelay = 0.3f;
    private float shootingDelayOnJump = 0.15f;
    private float ReloadDelay = 0.01f;
    private bool standShoot;
    private bool crouchShoot;
    private bool runShoot;
    private bool jumpShoot;
    private bool standReload;
    private bool crouchReload;
    private bool runReload;
    private bool jumpReload;

    private void Awake()
    {
        currentWeaponAmmo = maxWeaponAmmo;
    }
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        ammoText = GameObject.FindGameObjectWithTag("AmmoUI").GetComponent<Text>();
        if (currentWeaponAmmo > 0)
            return;       
    }

    void OnEnable()
    {
        isReloading = false;        
        //playerAnimator.SetBool("Reloading", false);
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMovement = Input.GetAxis("Horizontal");
        ammoText.text = currentWeaponAmmo.ToString();

        if (Time.time >= nextTimeToFire && currentWeaponAmmo != 0 && !isReloading)
        {
            if (Input.GetKeyDown(KeyCode.J) && isGrounded && Mathf.Abs(horizontalMovement) == 0 && !Input.GetKey(KeyCode.S))
            {
                Shoot();
                nextTimeToFire = Time.time + 1f / pistolFirerate;
                Invoke("ShootingComplete", shootingDelay);
            }
            if (Input.GetKeyDown(KeyCode.J) && Input.GetKey(KeyCode.S))
            {
                CrouchShoot();
                nextTimeToFire = Time.time + 1f / pistolFirerate;
                Invoke("ShootingComplete", shootingDelay);
            }
            if (Input.GetKeyDown(KeyCode.J) && isGrounded == false)
            {
                JumpShoot();                
                nextTimeToFire = Time.time + 1f / pistolFirerate;
                Invoke("ShootingComplete", shootingDelayOnJump);
            }
            if (Input.GetKeyDown(KeyCode.J) && Mathf.Abs(horizontalMovement) != 0 && isGrounded && crouchShoot != true)
            {
                RunShoot();
                nextTimeToFire = Time.time + 1f / pistolFirerate;
                Invoke("ShootingComplete", shootingDelay);
            }
                        
        }

        if (isReloading)
            return;
        if (currentWeaponAmmo <= 0)
        {
            StartCoroutine(weaponReloadOnEmpty());
            return;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (currentWeaponAmmo > 0 && currentWeaponAmmo < maxWeaponAmmo)
            {
                StartCoroutine(weaponReload());
                return;
            }
            else if (currentWeaponAmmo == 0)
            {
                StartCoroutine(weaponReloadOnEmpty());
                return;
            }
            else if (currentWeaponAmmo == maxWeaponAmmo)
            {
                //Do nothing
            }
        }
        //playerAnimator.SetBool("IsGround", isGrounded);
    }
   
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
            
        }
    }

    void Shoot()
    {
        currentWeaponAmmo--;
        playerAudioSource.PlayOneShot(pistolGunSound);
        ChangeAnimationState(Yuno_PISTOL_SHOOT);
        Instantiate(bulletTracer, firePoint.position, firePoint.rotation);
        standShoot = true;
    }

    void CrouchShoot()
    {
        currentWeaponAmmo--;
        playerAudioSource.PlayOneShot(pistolGunSound);
        ChangeAnimationState(Yuno_CROUCH_PISTOL_SHOOT);
        Instantiate(bulletTracer, firePointCrouch.position, firePointCrouch.rotation);
        crouchShoot = true;
    }

    void RunShoot()
    {
        currentWeaponAmmo--;
        ChangeAnimationState(Yuno_RUN_PISTOL_SHOOT);
        playerAudioSource.PlayOneShot(pistolGunSound);
        Instantiate(bulletTracer, firePointRun.position, firePointRun.rotation);
        runShoot = true;
    }

    void JumpShoot()
    {                
        currentWeaponAmmo--;
        ChangeAnimationState(Yuno_JUMP_PISTOL_SHOOT);
        playerAudioSource.PlayOneShot(pistolGunSound);
        Instantiate(bulletTracer, firePointJump.position, firePointJump.rotation);
        jumpShoot = true;
    }

    IEnumerator weaponReload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        playerAudioSource.PlayOneShot(pistolGunReload);
        //playerAnimator.SetBool("Reloading", true);

        if (isGrounded && Mathf.Abs(horizontalMovement) == 0 && !Input.GetKey(KeyCode.S))
        {
            ChangeAnimationState(Yuno_PISTOL_RELOAD);
            standReload = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            ChangeAnimationState(Yuno_CROUCH_PISTOL_RELOAD);
            crouchReload = true;
        }
        if (!isGrounded)
        {
            ChangeAnimationState(Yuno_JUMP_PISTOL_RELOAD);
            jumpReload = true;
        }
        if (Mathf.Abs(horizontalMovement) != 0 && isGrounded)
        {
            ChangeAnimationState(YUNO_RUN_PISTOL_RELOAD);
            runReload = true;
        }

        yield return new WaitForSeconds(weaponReloadTime - 0.25f);

        //playerAnimator.SetBool("Reloading", false);
        //yield return new WaitForSeconds(0.25f);        
        currentWeaponAmmo = maxWeaponAmmo;
        isReloading = false;
        ReloadComplete();
    }

    IEnumerator weaponReloadOnEmpty()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        playerAudioSource.PlayOneShot(pistolGunReload);
        //playerAnimator.SetBool("Reloading", true);

        if (isGrounded && Mathf.Abs(horizontalMovement) == 0 && !Input.GetKey(KeyCode.S))
        {
            ChangeAnimationState(Yuno_PISTOL_RELOAD);
            standReload = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            ChangeAnimationState(Yuno_CROUCH_PISTOL_RELOAD);
            crouchReload = true;
        }
        if (!isGrounded)
        {
            ChangeAnimationState(Yuno_JUMP_PISTOL_RELOAD);
            jumpReload = true;
        }
        if (Mathf.Abs(horizontalMovement) != 0 && isGrounded)
        {
            ChangeAnimationState(YUNO_RUN_PISTOL_RELOAD);
            runReload = true;
        }

        yield return new WaitForSeconds(weaponReloadTime - 0.25f);

        //playerAnimator.SetBool("Reloading", false);

        //yield return new WaitForSeconds(0.25f);

        currentWeaponAmmo = maxWeaponAmmo - 1;
        isReloading = false;
        ReloadComplete();

    }

    void ShootingComplete()
    {
        if (standShoot == true && isReloading != true)
        {
           ChangeAnimationState(Yuno_IDLE);
           standShoot = false;
            if (isReloading == true)
                ChangeAnimationState(Yuno_PISTOL_RELOAD);
        }
        if (crouchShoot == true && isReloading != true)
        {            
            ChangeAnimationState(Yuno_CROUCH_PISTOL);
            crouchShoot = false;
            if (isReloading == true)
                ChangeAnimationState(Yuno_CROUCH_PISTOL_RELOAD);
        }
        if (jumpShoot == true && isReloading != true)
        {
            jumpShoot = false;
            if(isGrounded == false)
            {
                ChangeAnimationState(Yuno_JUMP);
                if (isReloading == true && isGrounded == false)
                    ChangeAnimationState(Yuno_JUMP_PISTOL_RELOAD);                
            }           
        }
        if (runShoot == true && isReloading != true)
        {           
            ChangeAnimationState(Yuno_RUN);
            runShoot = false;
            if (isReloading == true)
                ChangeAnimationState(YUNO_RUN_PISTOL_RELOAD);
        }
        
    }
    void ReloadComplete()
    {
        Debug.Log("Reload Complete");
        if (standReload == true && isReloading == false)
        {            
            if (Input.GetKeyDown(KeyCode.S))
            {
                ChangeAnimationState(Yuno_CROUCH_PISTOL);
            }
            if (horizontalMovement != 0)
            {
                ChangeAnimationState(Yuno_RUN);
            }
            else
            {
                ChangeAnimationState(Yuno_IDLE);
            }
            standReload = false;
        }
        if (standReload == true && isReloading == false && Input.GetKeyDown(KeyCode.S))
        {            
                ChangeAnimationState(Yuno_CROUCH_PISTOL);
           
            standReload = false;
        }
            if (crouchReload == true && isReloading == false)
        {
            if (horizontalMovement == 0)
            {
                ChangeAnimationState(Yuno_CROUCH_PISTOL);
            }
            if (horizontalMovement != 0)
            {
                ChangeAnimationState(Yuno_RUN);
            }
            
            crouchReload = false;
        }
        if (jumpReload == true && !isGrounded && isReloading == false)
        {
            if (isGrounded == false)
            {
                ChangeAnimationState(Yuno_JUMP);
            }
            if (horizontalMovement == 0)
            {
                ChangeAnimationState(Yuno_IDLE);
            }
            if(horizontalMovement!= 0 )
            {
                ChangeAnimationState(Yuno_RUN);
            }
            jumpReload = false;
        }
        if (runReload == true && isReloading == false)
        {
            if (horizontalMovement == 0)
            {
                ChangeAnimationState(Yuno_IDLE);
            }
            if (horizontalMovement != 0)
            {
                ChangeAnimationState(Yuno_RUN);
            }            
            runReload = false;
        }
        //if(isGrounded == true && isReloading == false)
        //{
        //    ChangeAnimationState(Yuno_IDLE);
        //}
    }

    void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimation == newAnimation)
            return;
        playerAnimator.Play(newAnimation);
        currentAnimation = newAnimation;
    }
}
