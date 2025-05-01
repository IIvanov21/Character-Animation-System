using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;


public class Targeter : MonoBehaviour
{
    /*
     * A list which contrains all the available targets.
     * A simple index telling us which is the current target.
     */
    private List<Target> targets = new List<Target>();
    public Target CurrentTarget {  get; private set; }

    /*
     * References to both cameras, to help with targeting calculations.
     */
    [SerializeField] private CinemachineTargetGroup cinemachineTargetGroup;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    //Method to select the closest target within the camera's viewport
    public bool SelectTarget()
    {
        if(targets.Count==0) return false;

        //Variables to track the closest target and its distance from the centre of the viewport
        Target closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;

        foreach (Target target in targets) 
        { 
            //Get the target's position in the camera'sviewport coordinates
            Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);

            //Skip the target if it's outside the viewport
            if (viewPos.x < 0.0f || viewPos.x > 1.0f 
                || viewPos.y < 0.0f || viewPos.y > 1.0f) continue;

            //Calculate the squared distane from the viewport
            Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f);

            //If this target is close to the center than the previous closest target,
            //update the closest target
            if(toCenter.sqrMagnitude<closestTargetDistance)
            {
                closestTarget = target;
                closestTargetDistance = toCenter.sqrMagnitude;
            }
            
        }

        //If no valid target was found, return false
        if (closestTarget == null) return false;

        //If there is a target
        CurrentTarget = closestTarget;

        cinemachineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);

        return true;
    }

    public void Cancel()
    {
        //If there is no target selected, exit method early.
        if (CurrentTarget == null) return;

        //If we have a target remove it from the current cinemachine target list.
        cinemachineTargetGroup.RemoveMember(CurrentTarget.transform);
        //Release the current target reference
        CurrentTarget = null;
    }

    //Tidy up method, when done using a target.
    public void RemoveTarget(Target target)
    {
        //If target passed in is the same as Current Target, tidy up the current target reference.
        if (CurrentTarget == target)
        {
            cinemachineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }

        //Using the target passed in tidy up list and event references.
        target.OnDestroyed -= RemoveTarget;
        targets.Remove(target);
    }
}
