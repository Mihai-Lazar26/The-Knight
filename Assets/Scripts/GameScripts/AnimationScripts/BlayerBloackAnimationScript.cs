using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlayerBloackAnimationScript : StateMachineBehaviour
{
    private PlayerCombat _playerCombat;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       _playerCombat = animator.GetComponent<PlayerCombat>();
       _playerCombat.shieldCollider.enabled = true;
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       _playerCombat.shieldCollider.enabled = false;
    }
}
