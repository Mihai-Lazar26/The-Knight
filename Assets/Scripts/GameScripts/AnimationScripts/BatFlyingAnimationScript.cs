using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatFlyingAnimationScript : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.SetBool("isMoving", true);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.SetBool("isMoving", false);
    }
}
