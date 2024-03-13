using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceAnimationController : MonoBehaviour
{
    // Start is called before the first frame update
    private RuntimeAnimatorController runAni;
    private Animator ani;
    float timeTaken;

    void Start()
    {
       ani = gameObject.GetComponent<Animator>();
        runAni = ani.runtimeAnimatorController;
        timeTaken = ani.GetCurrentAnimatorClipInfo(0).Length;
    }

    
    // Update is called once per frame
    void Update()
    {
        startAniLoop();

    }

    private void startAniLoop()
    {

        
        int randomClip = Random.Range(0, runAni.animationClips.Length);
        timeTaken -= Time.deltaTime;

        if (timeTaken <= 0 )
        {
            ani.Play(runAni.animationClips[randomClip].name);
            timeTaken = runAni.animationClips[randomClip].length;

        }
       
       
    }
}
