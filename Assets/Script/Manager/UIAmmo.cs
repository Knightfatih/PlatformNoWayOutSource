using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAmmo : MonoBehaviour
{
    public Text ammoText;
    public Text clipText;

    public void Refresh(int ammoCount, int clipCount)
    {
        ammoText.text = ammoCount.ToString();
        clipText.text = clipCount.ToString();
    }
}
