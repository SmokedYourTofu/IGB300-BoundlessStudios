using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    // Start is called before the first frame update

    private AudioSource mySource;
    void Start()
    {
        mySource = this.GetComponent<AudioSource>();
    }

    public void Endgame()
    {
        mySource.Play();
        //do score and such
        Debug.Log("Game Over");
    }
}
