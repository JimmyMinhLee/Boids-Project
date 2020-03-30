using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehavior behavior;

    // Instantiation for Flock 
    [Range(10, 500)]
    public int startingCount = 250;
    const float agentDensity = 0.08f;

    // Behavior variables
    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maximumSpeed = 5f;

    // Neighbor variables
    [Range(1f, 10f)]
    public float neighborRadius = 5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = .5f;

    // Utility based on maxSpeed and radius - avoiding math
    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;

    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    // Start is called before the first frame update
    void Start()
    {
        squareMaxSpeed = maximumSpeed * maximumSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for (int i = 0; i < startingCount; i ++)
        {
            FlockAgent newAgent = Instantiate(
            agentPrefab,
            Random.insideUnitCircle * startingCount * agentDensity,
            Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
            this.transform);

            newAgent.name = "Agent: " + i;
            agents.Add(newAgent); 
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Going through and getting neighbors 
        foreach (FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);

            // Coolness!
            agent.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, context.Count / 6f); 
            Vector2 move = behavior.CalculateMove(agent, context, this);
            // Make speed more speedy! 
            move *= driveFactor;

            if (move.sqrMagnitude > squareMaxSpeed)
            {
                // If we're going any faster than we want, we normalize and bring it back to the max speed. 
                move = move.normalized * maximumSpeed; 
            }

            // Make our agents move. 
            agent.Move(move);

        }
    }

    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> nearby = new List<Transform>();
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);

        foreach (Collider2D c in nearbyColliders)
        {
            if (c != agent.AgentCollider)
            {
                nearby.Add(c.transform); 
            }
        }

        return nearby;
    
    }


}
