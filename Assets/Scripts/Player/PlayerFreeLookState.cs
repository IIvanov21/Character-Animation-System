using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
    }

    public override void Exit()
    {
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
        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation,
            Quaternion.LookRotation(stateMachine.MovementVector), deltaTime * stateMachine.RotationDamping);
    }
}
