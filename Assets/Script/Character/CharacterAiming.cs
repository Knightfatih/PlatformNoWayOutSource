using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAiming : MonoBehaviour
{
    public float turnSpeed = 15;
    public float aimDuration = 0.3f;
    public Transform cameraLookAt;
    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;
    public Cinemachine.CinemachineVirtualCamera cinemachineVirtualCamera;
    public bool isAiming;

    Camera mainCamera;
    WeaponActivate weaponActivate;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        weaponActivate = GetComponent<WeaponActivate>();
        weaponActivate = GetComponent<WeaponActivate>();
    }

    private void Update()
    {
        isAiming = Input.GetMouseButton(1);
        

        var weapon = weaponActivate.GetActiveWeapon();
        if (weapon)
        {
            if (isAiming)
            {
                ZoomIn();
            }
            weapon.recoil.recoilModify = isAiming ? 0.3f : 1.0f;
            ZoomOut();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        xAxis.Update(Time.fixedDeltaTime);
        yAxis.Update(Time.fixedDeltaTime);

        cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);

        float yAxisCamera = mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yAxisCamera, 0), turnSpeed * Time.deltaTime);
    }


    void ZoomIn()
    {
        if (isAiming)
        {
            cinemachineVirtualCamera.m_Lens.FieldOfView = 30f;
        }
    }

    void ZoomOut()
    {
        if (!isAiming)
        {
            cinemachineVirtualCamera.m_Lens.FieldOfView = 40f;
        }
    }

}
