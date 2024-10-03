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
    private SoundManager soundManager;
    public AudioClip openEffect;
    public AudioClip closeEffect;

    // Start is called before the first frame update
    void Start()
    {
        mydoor = this.GetComponent<Animator>();
        open = true;
        soundManager = FindObjectOfType<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= rotateInterval && open)
        {
            StartCoroutine(Waitbeforeplay2());
            mydoor.Play(animationNames[1], 0, 0.0f);
            timer = 0f;
            open = false;
        }
        else if (timer >= rotateInterval && !open) 
        {
            StartCoroutine(Waitbeforeplay());
            mydoor.Play(animationNames[0], 0, 0.0f);
            timer = 0f;
            open = true;
        }
    }

    private IEnumerator Waitbeforeplay()
    {
        yield return new WaitForSeconds(1f);
        soundManager.PlaySoundFXclip(openEffect, this.transform, 0.1f);
    }

    private IEnumerator Waitbeforeplay2()
    {
        yield return new WaitForSeconds(1f);
        soundManager.PlaySoundFXclip(closeEffect, this.transform, 0.1f);
    }
}
