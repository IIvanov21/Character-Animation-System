using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    /*
     * The input reader provides the player input data.
     */
    [field: SerializeField] public InputReader InputReader {  get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    public Vector3 MovementVector;
    public float playerSpeed = 5.0f;

    //Camera movement variables
    [field: SerializeField]public float RotationDamping { get; private set; }
    [field: SerializeField]public float FreeLookMovementSpeed { get; private set; }
    public Transform MainCameraTransform { get; private set; }

    //Animation variables
    [field: SerializeField] public Animator Animator { get; private set; }

    //Targeting References
    [field: SerializeField]public Targeter Targeter { get; private set; }
    [field: SerializeField]public float TargetingMovementSpeed {  get; private set; }

    private void Start()
    {
        MainCameraTransform = Camera.main.transform;

        SwitchState(new PlayerFreeLookState(this));
    }
}
