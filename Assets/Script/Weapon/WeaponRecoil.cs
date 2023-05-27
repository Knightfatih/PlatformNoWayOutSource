using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class WeaponRecoil : MonoBehaviour
{
    [HideInInspector] public CharacterAiming characterAiming;
    [HideInInspector] public CinemachineImpulseSource cameraShake;
    [HideInInspector] public Animator rig;

    public Vector2[] recoilPath;
    public float duration;
    public float recoilModify = 1.0f;

    float verticalRecoil;
    float horizontalRecoil;

    float time;
    int index;

    private void Awake()
    {
        cameraShake = GetComponent<CinemachineImpulseSource>();
    }

    public void Reset()
    {
        index = 0;
    }

    int NextIndex(int index)
    {
        return (index + 1) % recoilPath.Length;
    }

    public void GenerateRecoil(string weaponName)
    {
        time = duration;
        cameraShake.GenerateImpulse(Camera.main.transform.forward);

        horizontalRecoil = recoilPath[index].x;
        verticalRecoil = recoilPath[index].y;

        index = NextIndex(index);

        rig.Play("Recoil_" + weaponName, 1, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            characterAiming.yAxis.Value -= (((verticalRecoil/10) * Time.deltaTime) / duration) * recoilModify;
            characterAiming.xAxis.Value -= (((horizontalRecoil/10) * Time.deltaTime) / duration) * recoilModify;
            time -= Time.deltaTime;
        }
    }
}
