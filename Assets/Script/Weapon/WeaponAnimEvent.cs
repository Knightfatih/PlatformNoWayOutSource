using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimEvnt: UnityEvent<string>
{

}
public class WeaponAnimEvent : MonoBehaviour
{
    public AnimEvnt WeaponAnimEvnt = new AnimEvnt();
    public void OnAnimationEvent(string eventName)
    {
        WeaponAnimEvnt.Invoke(eventName);
    }
}
