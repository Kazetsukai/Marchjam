using UnityEngine;
using System.Collections;

public class TurretController_Straight : MonoBehaviour
{
    [Header("Object")]
    public Rigidbody ParentVehicle;

    [Header("Firing")]
    [SerializeField] GameObject BulletPrefab;
    [SerializeField] Transform BulletSpawnTransform;
    [SerializeField] float BulletForce = 100f;
    [SerializeField] float FireCooldown = 0.5f;

    float cooldownElapsed;

    void Start()
    {
        //Allow player to shoot immediately when game starts
        cooldownElapsed = FireCooldown;
    }
    
    void Update()
    {
        //Fire if user presses button and cooldown time has elapsed    
        if (cooldownElapsed >= FireCooldown && Input.GetAxis("Fire1") > 0)
        {
            FireBullet();
            cooldownElapsed = 0;
        }
        else
        {
            //Player can't shoot yet. Wait longer.
            cooldownElapsed += Time.deltaTime;
        }

    }

    void FireBullet()
    {
        //Generate new bullet
        GameObject newBullet = (GameObject)GameObject.Instantiate(BulletPrefab, BulletSpawnTransform.position, BulletSpawnTransform.rotation);

        //Add initial force to bullet
        newBullet.GetComponent<Rigidbody>().AddForce(newBullet.transform.forward * BulletForce + ParentVehicle.velocity, ForceMode.Force);
    }
}
