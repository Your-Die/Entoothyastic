using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceController : MonoBehaviour
{

    private Animator animator;

    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    public void SetSad()
    {
        animator.SetBool("isSad", true);
        animator.SetBool("isAngry", false);
    }

    public void SetAngry()
    {
        animator.SetBool("isSad", false);
        animator.SetBool("isAngry", true);
    }

    public void SetNormal()
    {
        animator.SetBool("isSad", false);
        animator.SetBool("isAngry", false);
    }
}
