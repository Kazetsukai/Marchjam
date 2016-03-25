using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{ 
    [Header("Properties")]
    [SerializeField] float Health = 100f;
    [SerializeField] float DespawnTimeAfterDeath = 2f;      //How long after dropping dead does enemy despawn?

    [Header("Movement")]
    [SerializeField] float MoveSpeed = 10f;
    [SerializeField] float RotationSpeed = 10f;

    [Header("AI")]
    public GameObject Target;

    [Header("Objects")]
    [SerializeField] GameObject Torso;
    [SerializeField] GameObject Legs;

    Rigidbody rb;
    Vector3 MoveDir;

    //Effects
    EllipsoidParticleEmitter bloodEmitter;

    float stunElapsed;
    float currentStunDuration;
        
    public bool Alive
    {
        get
        {
            return Health > 0 ? true : false;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bloodEmitter = GetComponentInChildren<EllipsoidParticleEmitter>();
    }
    
    void Update()
    {      
        //If dead, do nothing
        if (!Alive)
        {
            return;
        }

        //Update stun
        stunElapsed += Time.deltaTime;
        if (stunElapsed < currentStunDuration)
        {
            //Do nothing if stunned (allows enemy to be spun on Y axis and pushed around)
            return;
        }
        
        //Find target if there is none
        if (Target == null)
        {
            //Find closest player
            float closest = 9999999f;
            foreach (Player player in GameObject.FindObjectsOfType<Player>())
            {
                if (Vector3.Distance(transform.position, player.transform.position) < closest)
                {
                    Target = player.gameObject;
                }
            }
        }

        if (Target != null)
        {
            //Get direction to target
            MoveDir = (Target.transform.position - transform.position).normalized;

            //Apply input to movement
            rb.AddForce(MoveDir * MoveSpeed * Time.deltaTime, ForceMode.Impulse);
        }

        //Rotate towards movement direction
        if (MoveDir.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(MoveDir, Vector3.up), RotationSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage(Sword attackingSword, float damage)
    {
        if (Health > 0)
        {
            Health -= damage;

            if (bloodEmitter != null)
            {
                bloodEmitter.Emit(attackingSword.BleedsInflicted);
            }

            stunElapsed = 0;
            currentStunDuration = attackingSword.StunDuration;
        }
        
        if (Health <= 0)
        {
            Health = 0;
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        //Remove rigidbody constraints on death so that enemy falls over/rolls around
        rb.constraints = RigidbodyConstraints.None;

        //Add small torque to make enemy fall over
        rb.AddRelativeTorque(new Vector3(0, 10f, 0));

        //Split enemy in half
        Torso.AddComponent<Rigidbody>();
        Torso.GetComponent<Rigidbody>().velocity = rb.velocity;
        Torso.GetComponent<Rigidbody>().angularVelocity = rb.angularVelocity;

        Legs.AddComponent<Rigidbody>();
        Legs.GetComponent<Rigidbody>().velocity = rb.velocity;
        Legs.GetComponent<Rigidbody>().angularVelocity = rb.angularVelocity;

        Torso.GetComponent<MeshCollider>().enabled = true;
        Legs.GetComponent<MeshCollider>().enabled = true;

        //Remove rigidbody and collider
        Destroy(rb);
        Destroy(GetComponent<Collider>());

        float elapsed = 0;
        do
        {
            elapsed += Time.deltaTime;
            
            //Do other death stuff that occurs over time            
                      
            yield return null;
        }
        while (elapsed < DespawnTimeAfterDeath);

        //Despawn after dying is finished.
        Destroy(this.gameObject);      
    }
}
