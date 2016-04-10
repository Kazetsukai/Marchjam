using UnityEngine;
using System.Collections;
using System;

public class TurretController_Straight : MonoBehaviour, IWeaponController
{
    [Header("Object")]
    public Rigidbody ParentVehicle;

    [Header("Firing")]
    [SerializeField] GameObject BulletPrefab;
    [SerializeField] Transform BulletSpawnTransform;
    [SerializeField] float BulletForce = 100f;
    [SerializeField] float FireCooldown = 0.5f;

    float cooldownElapsed;
    Collider bulletCollider;
    private bool _isLocal;

    void Start()
    {
        bulletCollider = GetComponentInChildren<Collider>();

        //Allow player to shoot immediately when game starts
        cooldownElapsed = FireCooldown;

        _isLocal = PlayerNetworkCommands.LocalInstance.IsLocal(gameObject);
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
        // If we don't check this, every player fires a bullet from every vehicle on the server.
        if (_isLocal)
            PlayerNetworkCommands.LocalInstance.CmdFireWeapon(BulletSpawnTransform.position, BulletSpawnTransform.rotation * Vector3.forward * BulletForce + ParentVehicle.velocity);
    }

    public void FireWeapon(Vector3 position, Vector3 direction, float serverTime)
    {
        //Generate new bullet
        GameObject newBullet = (GameObject)GameObject.Instantiate(BulletPrefab, position, Quaternion.FromToRotation(Vector3.forward, direction.normalized));

        //Add initial force to bullet
        newBullet.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Force);

        //Set bullet to ignore all colliders of parent vehicle 
        foreach (Collider parentCol in ParentVehicle.GetComponentsInChildren<Collider>())
        {
            Physics.IgnoreCollision(parentCol, bulletCollider, true);
        }
    }
}
