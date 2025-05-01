using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    public PlayerTargetingState(PlayerStateMachine stateMachine): base(stateMachine) { }
    /*
     * Animation References
     */
    private readonly int TargetingBlendTree = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetHorizontal = Animator.StringToHash("Horizontal");
    private readonly int TargetVertical = Animator.StringToHash("Vertical");
    private const float AnimatorDampTime = 0.1f;


    public override void Enter()
    {
        //Transition to Targeting Animation Blend Tree
        stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTree, AnimatorDampTime);
        //Subscribe to the Input Reader target event


    }

    public override void Exit()
    {
        //Unsubscribing from the Input Reader target event
    }

    public override void Tick(float deltaTime)
    {

    }

    private void OnCancel()
    {
        //Tidy up Targeting behaviour
        stateMachine.Targeter.Cancel();

        //Switch to normal Free Look
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }
}
