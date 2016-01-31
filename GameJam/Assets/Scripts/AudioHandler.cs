using UnityEngine;
using System.Collections;

public class AudioHandler : MonoBehaviour {
    public AudioClip water;
    public AudioClip fire;
    public AudioClip earth;
    public AudioClip destruction;
    public AudioSource source;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void Request(int num)
    {
        if (num == 1)
        {
            source.PlayOneShot(fire);
        }
        if (num == 2)
        {
            source.PlayOneShot(water);
        }
        if (num == 3)
        {
            source.PlayOneShot(earth);
        }
        if (num == 4)
        {
            source.PlayOneShot(destruction);
        }
    }
}
