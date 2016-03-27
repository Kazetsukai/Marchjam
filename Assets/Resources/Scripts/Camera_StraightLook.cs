using UnityEngine;
using System.Collections;

public class Camera_StraightLook : MonoBehaviour
{
    [Header("Tracking")]
    [SerializeField] Camera camera;
    public Transform Target;    
    [SerializeField] float Distance;
    [SerializeField] float RotateRate = 20f;
    [SerializeField] float CameraVerticalOffsetAngle;

    [Header("Angles")]             
    [SerializeField] float MaxVertAngle = 178f;             
    [SerializeField] float MinVertAngle = 2f;
    [SerializeField] float CurrentHorizAngle;                 
    [SerializeField] float CurrentVertAngle;

    [Header("Turret Control")]
    public TurretController_Straight turret;

    [Header("Misc")]
    [SerializeField] UpdateType updateType = UpdateType.FixedUpdate;
    [SerializeField] bool F1UnlocksCursor = true;      
  
    void Start()
    {
        //Lock cursor to center of screen and hide
        LockCursor();
    }

    void FixedUpdate()
    {
        if (updateType == UpdateType.FixedUpdate)
        {
            UpdateCamera();
            UpdateTurret();
        }
    }
    
    void Update()
    {
        if (updateType == UpdateType.Update)
        {
            UpdateCamera();
            UpdateTurret();
        }
    }

    void LateUpdate()
    {
        if (updateType == UpdateType.LateUpdate)
        {
            UpdateCamera();
            UpdateTurret();
        }        
    }

    void UpdateCamera()
    {
        if (Target != null)
        {

            //Get mouse input, increment camera angles.
            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            CurrentHorizAngle -= mouseDelta.x * RotateRate * Time.deltaTime;
            CurrentVertAngle += mouseDelta.y * RotateRate * Time.deltaTime;
            CurrentVertAngle = Mathf.Clamp(CurrentVertAngle, MinVertAngle, MaxVertAngle);  //Clamp vertical angle

            //Move camera mount position (this object. Actual camera is child of this object)
            Vector3 offset = new Vector3(Mathf.Cos(Mathf.Deg2Rad * CurrentHorizAngle) * Mathf.Sin(Mathf.Deg2Rad * CurrentVertAngle), Mathf.Cos(Mathf.Deg2Rad * CurrentVertAngle), Mathf.Sin(Mathf.Deg2Rad * CurrentHorizAngle) * Mathf.Sin(Mathf.Deg2Rad * CurrentVertAngle)) * Distance;
            transform.position = Target.transform.position + offset;

            //Make camera look at target, then add extra rotation offset to camera on it's local axis
            camera.transform.LookAt(Target);
            camera.transform.localEulerAngles += new Vector3(CameraVerticalOffsetAngle, 0, 0);
        }
        //Unlock cursor
        if (F1UnlocksCursor)
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                if (Cursor.lockState == CursorLockMode.Locked)
                {
                    UnlockCursor();
                }
                else
                {
                    LockCursor();
                }
            }
        }
    }

    void UpdateTurret()
    {
        if (turret != null)
        {
            //Raycast from center of camera into world 
            RaycastHit mouseRayHitInfo;
            Ray mouseWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hit = Physics.Raycast(mouseWorldRay, out mouseRayHitInfo);

            //Rotate turret base to face camera direction
            if (hit)
            {
                turret.transform.rotation = Quaternion.LookRotation(mouseRayHitInfo.point - camera.transform.position, turret.ParentVehicle.transform.up);
            }
            else
            {
                turret.transform.rotation = Quaternion.LookRotation(camera.transform.forward, turret.ParentVehicle.transform.up);
            }
        }      
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}

enum UpdateType
{
    Update,
    FixedUpdate,
    LateUpdate
}
