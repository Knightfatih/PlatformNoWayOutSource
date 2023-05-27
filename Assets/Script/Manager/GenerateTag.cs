using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTag : MonoBehaviour
{
    [SerializeField] public Transform parent;

    // Update is called once per frame
    void Update()
    {
        transform.tag = parent.tag;
    }
}
