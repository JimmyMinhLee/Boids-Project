using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/SteeredCohesion")]
public class SteeredCohesionBehavior : FlockBehavior
{

    Vector2 currentVelocity;
    public float agentSmoothTime = .5f; 
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        // If no neighbors, we don't do anything. 
        if (context.Count == 0)
        {
            return Vector2.zero;
        }

        // Otherwise, we take the average of all our points and perform calculations.


        /** In particular, we want to emulate cohesion - when birds fly towards each other
            they try to stay near each other. So, for all of the other birds surrounding this guy,
            have him go towards the other people's position. We can accomplish this linearly by
            adding their positions and normalizing the resulting vector. */

        Vector2 cohesiveMove = Vector2.zero;
        foreach (Transform item in context)
        {
            cohesiveMove += (Vector2)item.position;
        }

        cohesiveMove = cohesiveMove / context.Count;

        // Now, we have to create an offset to our individual bird's position.
        cohesiveMove -= (Vector2)agent.transform.position;
        // Use Unity's physics engine to smooth out flickering (if any). 
        cohesiveMove = Vector2.SmoothDamp(agent.transform.up, cohesiveMove, ref currentVelocity, agentSmoothTime); 

        return cohesiveMove;

    }
}
