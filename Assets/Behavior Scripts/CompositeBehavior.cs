using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Composite")]
public class CompositeBehavior : FlockBehavior
{
    public FlockBehavior[] behaviors;
    public float[] behaviorWeights; 
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        // Making sure that we have the same amount of weights as there are for our behaviors 
        if (behaviorWeights.Length != behaviors.Length)
        {
            Debug.Log("Error: data mismatch in: " + name);
            return Vector2.zero; 
        }

        Vector2 moveVector = Vector2.zero;

        // Go through behaviors
        for (int i = 0; i < behaviors.Length; i++) 
        {
            Vector2 partialMovement = behaviors[i].CalculateMove(agent, context, flock) * behaviorWeights[i];

            // If we're actually making some movement
            if (partialMovement != Vector2.zero)
            {
                // Check to see if we're doing too much movement 
                if (partialMovement.sqrMagnitude > behaviorWeights[i] * behaviorWeights[i])
                {
                    partialMovement.Normalize();
                    partialMovement *= behaviorWeights[i]; 
                }

                // Add everything to our movement vector
                moveVector += partialMovement;
            }
        }

        return moveVector; 
    }
}
