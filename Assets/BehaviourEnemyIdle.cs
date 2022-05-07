using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourEnemyIdle : StateMachineBehaviour {
    private Transform player;
    private Transform enemy;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        enemy = animator.gameObject.transform.parent.transform;
        player = GameManager.Instance.player.transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        float distance = Vector3.Distance(enemy.position, player.position);
        if (distance < 2f) {
            animator.SetFloat("Speed", 5f);
            enemy.LookAt(player.position);
        }
    }

}
