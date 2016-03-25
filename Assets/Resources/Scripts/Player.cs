using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [Header("Properties")]
    public int PlayerNumber = 1;
    public Color PlayerColor;

    [Header("Movement")]
    [SerializeField] float MoveSpeed = 2f;
    [SerializeField] float RotationSpeed = 10f;
    [SerializeField] float DashSpeed = 3f;

    [Header("Combat")]
    [SerializeField] Sword Sword;
    [SerializeField] Transform SwordMover;
    [SerializeField] Animator Anim_WeaponArm;
    [SerializeField] float AttackCooldown = 0.2f;    

    //Movement
    Rigidbody rb;
    Vector3 MoveDir = new Vector3();

    //Combat
    bool attacking;
    bool dashing;
    float timeSinceAttack;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //Set player color
        Material mat = GetComponent<MeshRenderer>().materials[0];
        mat.color = PlayerColor;
    }

    void FixedUpdate()
    {
        UpdateInput();

        //Apply input to movement
        rb.AddForce(MoveDir * MoveSpeed * Time.deltaTime, ForceMode.Impulse);        
    }

    void Update()
    {
        timeSinceAttack += Time.deltaTime;

        //Rotate towards movement direction     
        if ((MoveDir.magnitude > 0.01f) && (timeSinceAttack > AttackCooldown))          //Prevent other Y-rotation if player is still engaged in attack
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(MoveDir, Vector3.up), RotationSpeed * Time.deltaTime);
        }

        UpdateAttack();

        //Prevent roll
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);        
    }

    void UpdateInput()
    {
        MoveDir.x = Input.GetAxis("L_XAxis_" + PlayerNumber);
        MoveDir.z = -Input.GetAxis("L_YAxis_" + PlayerNumber);       
    }

    void UpdateAttack()
    {
        //Reset attack state
        attacking = false;

        //Get attack input
        attacking = Input.GetButtonDown("X_" + PlayerNumber);

        //Do sword swing animation
        Anim_WeaponArm.SetBool("attack", attacking);

        //Do other stuff
        if (attacking)
        {
            //Tell sword it is now in use
            Sword.UseSword();

            //Do dash
            rb.AddForce(transform.forward * DashSpeed * Time.deltaTime, ForceMode.Impulse);

            timeSinceAttack = 0;                                
        }        
    }

    /*
    IEnumerator Dash()
    {
        dashing = true;
        float elapsed = 0;
        float duration = TimeVsDashSpeed.Duration();

        do
        {
            elapsed += Time.deltaTime;

            //Move player in direction of dash
            c_Controller.SimpleMove(transform.forward * TimeVsDashSpeed.Evaluate(elapsed) * Time.deltaTime);

            yield return null;
        }
        while (elapsed / duration < 1f);
        dashing = false;
    }*/
}
