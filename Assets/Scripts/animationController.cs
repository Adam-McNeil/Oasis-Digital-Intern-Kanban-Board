using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class animationController : NetworkBehaviour
  {
  [SerializeField] private Animator animator = null;
  private string currentState;
  private Quaternion rotation;
  private bool isRunning;

  //Player animation Consts
   const string PLAYER_IDLE = "idle";
   const string PLAYER_RUN_FORWARD = "running";
   const string PLAYER_RUN_BACKWARDS = "backwards";
   const string PLAYER_STRAFE_LEFT = "left";
   const string PLAYER_STRAFE_RIGHT = "right";
   const string PLAYER_JUMP = "jump";
   const string PLAYER_FORWARD_RIGHT = "forward_right";
   const string PLAYER_FORWARD_LEFT = "forward_left";
   const string PLAYER_BACKWARDS_LEFT = "backwards_left";
   const string PLAYER_BACKWARDS_RIGHT = "backwards_right";   
   bool isGrounded;
  //

   Vector3 currentPos;
   Vector3 begginingFrameLocation;

  private void Start()
    {
      currentPos = this.transform.position;
      begginingFrameLocation = transform.position;
    }

  void Update()
    {
    if (isLocalPlayer)
      {
        if (!AnimatorIsPlaying(PLAYER_JUMP))
        {
          if (Input.GetKey("w"))
          {
            if (Input.GetKey("d"))
            {
              changeAnimationState(PLAYER_FORWARD_RIGHT);
              return;
            }
          }
          if (Input.GetKey("w"))
          {
            if (Input.GetKey("a"))
            {
              changeAnimationState(PLAYER_FORWARD_LEFT);
              return;
            }
          }
          if (Input.GetKey("s"))
          {
            if (Input.GetKey("d"))
            {
              changeAnimationState(PLAYER_BACKWARDS_RIGHT);
              return;
            }
          }
          if (Input.GetKey("s"))
          {
            if (Input.GetKey("a"))
            {
              changeAnimationState(PLAYER_BACKWARDS_LEFT);
              return;
            }
          }
          if (Input.GetKey("space"))
          {
            changeAnimationState(PLAYER_JUMP);
            return;
          }
          if (Input.GetKey("w"))
          {
            isRunning = true;
            changeAnimationState(PLAYER_RUN_FORWARD);
            return;
          } else
          {
            isRunning = false;
          }
          if (Input.GetKey("s"))
          {
            changeAnimationState(PLAYER_RUN_BACKWARDS);
            return;
          }
          if (Input.GetKey("a"))
          {
            changeAnimationState(PLAYER_STRAFE_LEFT);
            return;
          }
          if (Input.GetKey("d"))
          {
            changeAnimationState(PLAYER_STRAFE_RIGHT);
            return;
          }
          if (currentPos == this.transform.position)
          {
            changeAnimationState(PLAYER_IDLE);
          }
          currentPos = this.transform.position;
          isGrounded = false;
        }
      }
    }

    private void changeAnimationState(string newState)
    {
      if (currentState == newState)
        return;

      animator.Play(newState);

      currentState = newState;
    }

    bool AnimatorIsPlaying(){
     return animator.GetCurrentAnimatorStateInfo(0).length > animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
    bool AnimatorIsPlaying(string stateName){
      return AnimatorIsPlaying() && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
  }
