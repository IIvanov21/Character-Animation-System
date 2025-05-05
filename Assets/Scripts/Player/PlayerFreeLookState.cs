using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    /*
     * Animation variables
     */
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private readonly int FreeLookBlendTree = Animator.StringToHash("FreeLookBlendTree");
    private const float AnimatorDampTime = 0.1f;

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTree, AnimatorDampTime);
        //Bind our Target event to the Input Reader
        stateMachine.InputReader.TargetEvent += OnTarget;
    }

    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent -= OnTarget;
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.MovementVector = CalculateMovement();

        stateMachine.Controller.Move(stateMachine.MovementVector*deltaTime*stateMachine.playerSpeed);

        FaceMovementDirection(deltaTime);
    }

    Vector3 CalculateMovement()
    {
        //Get the forward direction of the camera
        Vector3 cameraForward = stateMachine.MainCameraTransform.forward;

        //Zero out the Y property to keep movement on the horizontal plane
        cameraForward.y = 0f;

        //Normalize the forward vector to ensure it has a magnitude of 1
        cameraForward.Normalize();

        //Get the right direction of the camera
        Vector3 cameraRight= stateMachine.MainCameraTransform.right;

        //Zero out the Y property to keep movement on the horizontal plane
        cameraRight.y = 0f;

        //Normalize the right vector to ensure it has a magnitude of 1
        cameraRight.Normalize();

        return cameraForward*stateMachine.InputReader.MovementValue.y 
                + cameraRight*stateMachine.InputReader.MovementValue.x;
    }

    private void FaceMovementDirection(float deltaTime)
    {
        //Animation Handling
        //Check if there is no movement input
        if(stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(FreeLookSpeedHash,0, AnimatorDampTime,deltaTime);
            return;//Exit early because there is no movement
        }

        //If there is movement
        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampTime,deltaTime);

        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation,
            Quaternion.LookRotation(stateMachine.MovementVector), deltaTime * stateMachine.RotationDamping);
    }

    private void OnTarget()
    {
        Debug.Log("Trying to target");
        if (!stateMachine.Targeter.SelectTarget())
        {
            Debug.Log("Failed");
            return;
        }

        stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        
    }
}
