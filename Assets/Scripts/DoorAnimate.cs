using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimate : MonoBehaviour
{
    [SerializeField] private Animator mydoor;
    
    [SerializeField] private float timer = 0f;
    [SerializeField] public float rotateInterval;

    public List<string> animationNames = new List<string>();

    private bool open;

    // Start is called before the first frame update
    void Start()
    {
        mydoor = this.GetComponent<Animator>();
        open = true;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= rotateInterval && open)
        {
            mydoor.Play(animationNames[1], 0, 0.0f);
            timer = 0f;
            open = false;
        }
        else if (timer >= rotateInterval && !open) 
        {
            mydoor.Play(animationNames[0], 0, 0.0f);
            timer = 0f;
            open = true;
        }
    }
}
