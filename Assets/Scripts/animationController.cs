using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class animationController : NetworkBehaviour
  {
  [SerializeField] private Animator animator = null;
  void Update()
    {
    if (isLocalPlayer)
      {

      if (Input.GetKey("w"))
        {
        animator.SetBool("isRunning", true);
        }
      if (!Input.GetKey("w"))
        {
        animator.SetBool("isRunning", false);
        }

      if (Input.GetKey("s"))
        {
        animator.SetBool("isRunningBackwards", true);
        }
      if (!Input.GetKey("s"))
        {
        animator.SetBool("isRunningBackwards", false);
        }

      if (Input.GetKey("d"))
        {
        animator.SetBool("isRunningRight", true);
        }
      if (!Input.GetKey("d"))
        {
        animator.SetBool("isRunningRight", false);
        }

      if (Input.GetKey("a"))
        {
        animator.SetBool("isRunningLeft", true);
        }
      if (!Input.GetKey("a"))
        {
        animator.SetBool("isRunningLeft", false);
        }
      }
    }
  }
