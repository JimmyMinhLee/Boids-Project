using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Avoidance")]
public class AvoidanceBehavior : FlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        // If no neighbors, we don't do anything. 
        if (context.Count == 0)
        {
            return Vector2.zero;
        }

        // Otherwise, we take the average of all our points and perform calculations.

        /** Now, we want to avoid things. Birds don't like to fly too closely to other birds.
         * So, we keep a tracker of the amount of things we need to avoid. If the distance
         * between this particular bird and all the other birds is less than their avoidance radius,
         * we're going to tell this bird to avoid the other ones. */ 

        int numberAvoid = 0; 

        Vector2 avoidingMove = Vector2.zero;
        foreach (Transform item in context)
        {
            // If this bird is too close to the other birds
            if (Vector2.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadius)
            {
                // We're going to try to avoid this one bird 
                numberAvoid += 1;

                // By moving in the opposite direction 
                avoidingMove += (Vector2)(agent.transform.position - item.position);

            }
        }

        // If we have any amount of birds to avoid 
        if (numberAvoid > 0)
        {
            // We want to avoid them and normalize the resulting vector by the amount of things we're trying to avoid
            // We make note that, if we weren't avoiding anything, the vector would just be 0 since we don't
            // update our initial zero vector. Moreover, since we're going in the opposite direction based on
            // the amount of birds we're avoiding, we can normalize by dividing the movement vector by how many things
            // we're trying to avoid. We just don't want to go avoiding in weird directions.

            avoidingMove = avoidingMove / numberAvoid; 
        }

        return avoidingMove;

    }
}
