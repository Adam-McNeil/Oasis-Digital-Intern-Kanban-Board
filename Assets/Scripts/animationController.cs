using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class animationController : NetworkBehaviour
  {
  [SerializeField] private Animator animator = null;
  private Quaternion rotation;
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
      if (Input.GetKey("space"))
        {
        animator.SetBool("isJumping", true);
        }
      if (!Input.GetKey("space"))
        {
        animator.SetBool("isJumping", false);
        }
      if (Input.GetKeyDown("w") && Input.GetKeyDown("d"))
        {
          rotation = this.transform.rotation;
          this.transform.Rotate(0, 45, 0);
        }
      if (Input.GetKeyUp("w") && Input.GetKeyUp("d"))
        {
          rotation = this.transform.rotation;
        }
      }
    }
  }
