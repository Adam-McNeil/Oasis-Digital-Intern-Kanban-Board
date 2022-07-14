using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationController : NetworkBehaviour
  {
  [SerializeField] private Animator animator = null;
  private string currentState;
  private Quaternion rotation;
  private bool isRunning;

  //Player animation Consts
   const string PLAYER_IDLE = "idle";
   const string PLAYER_WALK_FORWARD = "walking";
   const string PLAYER_JUMP = "jump";
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
            changeAnimationState(PLAYER_WALK_FORWARD);
          }
          if (Input.GetKey("space"))
          {
            changeAnimationState(PLAYER_JUMP);
            return;
          }
          if (Input.GetKey("w"))
          {
            isRunning = true;
            changeAnimationState(PLAYER_WALK_FORWARD);
            return;
          } else
          {
            isRunning = false;
          }
          if (Input.GetKey("s"))
          {
            changeAnimationState(PLAYER_WALK_FORWARD);
            return;
          }
        //   if (Input.GetKey("a"))
        //   {
        //     changeAnimationState(PLAYER_STRAFE_LEFT);
        //     return;
        //   }
        //   if (Input.GetKey("d"))
        //   {
        //     changeAnimationState(PLAYER_STRAFE_RIGHT);
        //     return;
        //   }
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
