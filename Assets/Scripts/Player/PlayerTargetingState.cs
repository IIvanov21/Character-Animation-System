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
        //Subscribe to the Input Reader cancel target event
        stateMachine.InputReader.CancelTargetEvent += OnCancel;

    }

    public override void Exit()
    {
        //Unsubscribing from the Input Reader target event
        stateMachine.InputReader.CancelTargetEvent -= OnCancel;
    }

    public override void Tick(float deltaTime)
    {
        if(stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }

        stateMachine.MovementVector = CalculateMovement();
        stateMachine.Controller.Move(stateMachine.MovementVector * deltaTime 
            * stateMachine.TargetingMovementSpeed);

        FaceTarget();

        UpdateAnimator(deltaTime);
    }

    private void OnCancel()
    {
        //Tidy up Targeting behaviour
        stateMachine.Targeter.Cancel();

        //Switch to normal Free Look
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    private Vector3 CalculateMovement()
    {
        Vector3 movement = new Vector3();
        movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
        movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;

        return movement;
    }

    private void FaceTarget()
    {
        //If there is no target, exit early
        if(stateMachine.Targeter.CurrentTarget == null) return;

        //If there is a target make the player always face that target
        Vector3 facingVector = stateMachine.Targeter.CurrentTarget.transform.position - 
                               stateMachine.transform.position;

        facingVector.y = 0.0f;

        stateMachine.transform.rotation=Quaternion.LookRotation(facingVector);
    }

    private void UpdateAnimator(float deltaTime)
    {
        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(TargetHorizontal, 0, AnimatorDampTime, deltaTime);
            stateMachine.Animator.SetFloat(TargetVertical, 0, AnimatorDampTime, deltaTime);

            return;//Exit early because there is no movement
        }

        //If there is movement
        stateMachine.Animator.SetFloat(TargetHorizontal, stateMachine.InputReader.MovementValue.x, AnimatorDampTime, deltaTime);
        stateMachine.Animator.SetFloat(TargetVertical, stateMachine.InputReader.MovementValue.y, AnimatorDampTime, deltaTime);

    }
}
