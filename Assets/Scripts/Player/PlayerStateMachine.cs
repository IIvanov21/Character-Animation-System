using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    /*
     * The input rreader provides the player input data.
     */
    [field: SerializeField]
    public InputReader InputReader {  get; private set; }
    
    public float playerSpeed = 5.0f;

    private void Start()
    {
        SwitchState(new PlayerTestState(this));
    }
}
