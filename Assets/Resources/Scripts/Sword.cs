using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour
{
    [Header("Sword Properties")]
    public GameObject SwordWielder;
    public float KnockbackForce = 100f;
    public float Damage = 34f;
    public float StunDuration = 0.4f;
    public int BleedsInflicted = 10;                //How many blood particles does this sword cause 
    public float CollisionWindowSeconds = 0.2f;     //How long since player hit attack button are collisions with sword considered attacks?

    float timeSinceUse;

    Vector3 lastPosition;
    Vector3 velocity;

    void Start()
    {

    }    
    
    void Update()
    {
        velocity = (transform.localPosition - lastPosition).normalized;

        //Count time elapsed since sword was used
        if (timeSinceUse < CollisionWindowSeconds)
        {
            timeSinceUse += Time.deltaTime;
        }

        lastPosition = transform.localPosition;
    }

    public void UseSword()
    {
        //Reset sword use timer. This makes it consider any collisions as attacks, until CollisionWindowSeconds has elapsed.
        timeSinceUse = 0;
    }

    void OnTriggerEnter(Collider collider)
    {
        //Don't do anything if sword isn't being swung by user
        if (timeSinceUse > CollisionWindowSeconds)
        {
            return;
        }
       
        Rigidbody rigidbody = collider.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            //Knockback rigidbodies from the wielder that struck them
            rigidbody.AddForce(SwordWielder.transform.forward * KnockbackForce, ForceMode.Impulse);

            //Add spin to rigidbody after being cut. Determine CW or CCW based on sword move direction           
            rigidbody.AddTorque(10000f, 10000f, 10000f, ForceMode.Impulse);
           
        }

        //Apply damage to enemy
        Enemy enemy = collider.GetComponent<Enemy>();
        if (enemy != false)
        {
            enemy.TakeDamage(this, Damage);
        }
    }
}
