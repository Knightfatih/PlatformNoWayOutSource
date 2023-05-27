using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip mainTheme;
    public AudioClip menuTheme;

    private void Start()
    {
        AudioManager.instance.PlayMusic(mainTheme, 2);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M)){
           AudioManager.instance.PlayMusic(menuTheme, 3);
        }
    }
}
