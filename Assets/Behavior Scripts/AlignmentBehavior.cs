using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Alignment")]
public class AlignmentBehavior : FlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        // If no neighbors, we maintain our current alignment. 
        if (context.Count == 0)
        {
            return agent.transform.up;
        }

        // Otherwise, we take the average of all our points and perform calculations.

        /**
         * In this case, we want them to "align" themselves towards the direction
         * all the other birds are facing. So, we can just take the transform.up of
         * everybody else and add them to our alignment vector, similarly to how we
         * did cohesion previously. We don't have to normalize in this case because all
         * of the "up" vectors are simply equal to 1. */ 

        Vector2 alignmentMove = Vector2.zero;
        foreach (Transform item in context)
        {
            alignmentMove += (Vector2)item.transform.up;
        }

        return alignmentMove;

    }
}
