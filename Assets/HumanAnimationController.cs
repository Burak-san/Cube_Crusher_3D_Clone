using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAnimationController : MonoBehaviour
{

    private Animator _animator;

    private void Awake()
    {
        
        _animator = GetComponent<Animator>();
        Debug.Log("get component anim");
    }

    void Start()
    {
        
        _animator.Play("Running");
        Debug.Log("Animator play");
    }
}
