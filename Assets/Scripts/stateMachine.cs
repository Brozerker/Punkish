using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum STATES { IDLE, SEEK, PATHFOLLOW, ATTACK}

public class stateMachine {
    private STATES state;

    public stateMachine() {
        state = STATES.IDLE; 
    }
    public void Update(enemyManager agent, GameObject[] nodes) {
        GameObject player = GameObject.FindWithTag("Player");
        Debug.Log(state, agent);
        switch (state) {
            case STATES.IDLE:
                Idle(agent, player); // checks if player is within range -> switches to seek
                break;
            case STATES.SEEK:
                //Seek(agent, nodes[Random.Range(0, nodes.Length)]);
                Seek(agent, player);
                if (Vector3.Distance(agent.transform.position, player.transform.position) < 3)
                    state = STATES.ATTACK;
                if (!agent.lineOfSight(player))
                    state = STATES.PATHFOLLOW;
                break;
            case STATES.PATHFOLLOW:
                GameObject[] path = new GameObject[nodes.Length];
                if (Astar(agent.gameObject, nodes, path, player, 0)) {
                    Debug.Log("Astar");
                    PathFollow(agent, path);
                }
                break;
            case STATES.ATTACK:
                if (Vector3.Distance(agent.transform.position, player.transform.position) > 3)
                    state = STATES.SEEK;
                break;
        }
    }
    void exit() {

    }

    void Idle(enemyManager agent, GameObject player) {
        if (Vector2.Distance(agent.transform.position, player.transform.position) < agent.sensorRange && agent.lineOfSight(player)) {
            Debug.Log("Seek");
            state = STATES.SEEK;
        }
    }

    void Seek(enemyManager agent, GameObject targetThing) {
        if (agent.myTarget == null || Vector3.Distance(agent.transform.position, agent.myTarget.transform.position) < 0.5f)
            agent.myTarget = targetThing;
        Transform target = agent.myTarget.transform;
        agent.transform.position = Vector2.MoveTowards(agent.transform.position, target.position, agent.speed * Time.deltaTime);
        Vector2 moveDirection = agent.GetComponent<Rigidbody2D>().velocity;
        if (moveDirection != Vector2.zero) {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            agent.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void PathFollow(enemyManager agent, GameObject[] nodes) {
        for (int i = 0; i < nodes.Length; ++i) {
            if (Vector3.Distance(agent.gameObject.transform.position, nodes[i].transform.position) > 3) {
                i--;
            }
            Seek(agent, nodes[i]);
        }
    }

    bool Astar(GameObject current, GameObject[] nodes, GameObject[] path, GameObject target, int depth) {
        if (depth == nodes.Length)
            return false;
        for (int i = 0; i < nodes.Length; i++) {
            Debug.Log(nodes[i].name);
        }
        Vector3 currentPos = current.transform.position;
        Vector3 targetPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        Debug.Log("current: " + current.name + " " + currentPos);
        Debug.Log("target: " + target.name + " " + targetPos);
        Debug.DrawRay(currentPos, targetPos);
        Debug.Break();
        // does currentPos have line of sight to target? 
        if(lineOfSight(currentPos, target))
            return true;
      
        // otherwise create a list of the nodes that currentPos can see 
        List<GameObject> visibleNodes = new List<GameObject>();
        int count = 0;
        for (int i = 0; i < nodes.Length; ++i) {
            if (lineOfSight(currentPos, nodes[i])) {
                visibleNodes.Add(nodes[i]);
                count++;
            }
        }
        GameObject closest = visibleNodes[0];
        for (int i = 1; i < count; ++i) {
            if (Vector3.Distance(targetPos, closest.transform.position) > Vector3.Distance(targetPos, visibleNodes[i].transform.position))
                closest = visibleNodes[i];
        }
        path[depth] = closest;
        // and call Astar on the one that's closest to target
        return Astar(closest, nodes, path, target, depth + 1);
    }
     bool lineOfSight(Vector3 start, GameObject target) {
        Vector3 end = target.transform.position;
        Vector3 direction = (end - start).normalized;
        RaycastHit2D rayHit = Physics2D.Raycast(start, direction);
        Debug.DrawLine(start, target.transform.position);
        if (rayHit.transform == target.transform)
            return true;
        return false;
    }
}

//V2f seek(V2f target, Agent * agent) {
//    V2f desired = target - agent->body.center;
//    //desired *= agent->maximumSpeed / desired.magnitude();
//    desired.normalize();
//    desired *= agent->maximumSpeed;
//    V2f force = desired - agent->velocity;
//    return force * (agent->maximumForce / agent->maximumSpeed);
//}

//V2f wander(Agent * agent) {
//    //create structure for steering vector
//    V2f newDirection;
//    //get velocity from vector form of orientation
//    newDirection = agent->velocity;
//    //change orientation randomly betwee 0 - PI
//    float randomRotation = Random::PRNGf(-90, 90);
//    newDirection.rotate(randomRotation/180);
//    //output steering
//    return newDirection;
//}

//V2f followPath(Agent * a) {
//    V2f pathOffset(1,1);
//    //TemplateVector<Obstacle*> pillars = a->game->pillars;
//    //Obstacle * closestPillar = pillars[0];
//    //for(int i = 1; i < pillars.size(); i++) {
//    //	if(V2f::distance(a->body.center, pillars[i]->getCenter()) < V2f::distance(a->body.center, closestPillar->getCenter())) {
//    //		pillars[i] = closestPillar;
//    //	}
//    //}
//    CircF path = a->circlePath; 
//    V2f outNorm;
//    V2f futurePos = a->body.center + a->velocity;
//    V2f currentParam = path.getClosestPointOnEdge(futurePos, outNorm);
//    V2f target = (futurePos + currentParam) / 2; 
//    //currentPosition += pathOffset;
//    a->direction = (target - a->body.center).normal();
//    return seek(target, a);
//}


//V2f obstacleAvoidance(TemplateVector<Obstacle*> * obstacles, Obstacle * sensorArea, Agent * a, CalculationsFor_ObstacleAvoidance * calc) {
//    if (calc) calc->clear();
//    V2f totalForce;
//    for (int i = 0; i < obstacles->size(); ++i) {
//        Obstacle * actuallyHit = obstacles->get(i);
//        if (actuallyHit != a && actuallyHit->intersects(sensorArea)) {
//            float totalDistance = a->velocity.magnitude();
//            V2f normal;
//            V2f point = actuallyHit->getClosestPointOnEdge(a->body.center, normal);
//            V2f closestOnVelocity;
//            V2f::closestPointOnLine(
//                a->body.center, a->body.center + a->velocity,
//                point, closestOnVelocity);
//            V2f delta = closestOnVelocity - point;
//            delta.normalize();
//            float distFromStart = (point - a->body.center).magnitude();
//            float forceForThisHit = totalDistance - distFromStart;
//            if (calc) {
//                calc->actualHits.add(actuallyHit);
//                calc->hitLocations.add(point);
//                calc->hitNormals.add(delta);
//                calc->hitForce.add(forceForThisHit);
//            }
//            totalForce += delta * forceForThisHit;
//        }
//    }
//    return totalForce;
//}

////vec2 Separation(Agent agent, List<Agent> neighbors) {
//V2f separation(Agent * agent, float neighborRadius, TemplateVector<Agent*> & neighbors) {
//    V2f totalPush;
//    int countTooClose = 0;
//    bool inRange = false;
//    for(int i = 0; i < neighbors.size(); ++i) {
//        if(neighbors[i] != agent) { 
//            V2f delta = agent->body.center - neighbors[i]->body.center;
//            float dist = 0;
//            if(!delta.isZero()) 
//                dist = delta.magnitude() - (agent->body.radius + neighbors[i]->body.radius);
//            if(dist < neighborRadius) {
//                countTooClose++;
//                totalPush += delta.normal();
//            }
//        }
//    }
//    if(countTooClose > 0)
//        return totalPush / countTooClose;
//    return V2f::ZERO();
//}

////vec2 Cohesion(Agent agent, List<Agent> neighbors) {
//V2f cohesion(Agent * agent, TemplateVector<Agent*> & neighbors) {
//    //if (neighbors.empty()) return vec2.zero;
//    if(neighbors.size() == 0) return V2f::ZERO();
//    //vec2 centerOfMass = agent.position;
//    V2f centerOfMass = agent->body.center;
//    //for (Agent neighbor : neighbors)
//    for(int i = 0; i < neighbors.size(); ++i) {
//        //centerOfMass += neighbor.position;
//        if(neighbors[i] == agent)	continue;
//        centerOfMass += neighbors[i]->body.center;
//    }
//    //centerOfMass /= neighbors.count();
//    if(neighbors.size() > 0) 
//        centerOfMass /= (float)neighbors.size();
//    //vec2 desired = centerOfMass - agent.position;
//    V2f desired = centerOfMass - agent->body.center;
//    //desired *= agent.maxSpeed / desired.mag();
//    if(!desired.isZero())
//        desired *= agent->maximumSpeed / desired.magnitude();
//    //vec2 force = desired - agent.velocity;
//    V2f force = desired - agent->velocity;
//    //return force * (agent.maxForce / agent.maxSpeed);
//    return force * (agent->maximumForce / agent->maximumSpeed);
////}
//}

//V2f alignment(Agent * agent, TemplateVector<Agent*> & neighbors) {
//    V2f avgHeading = agent->velocity.normal();
//    for(int i = 0; i < neighbors.size(); ++i) {
//        avgHeading += neighbors[i]->velocity.normal();
//    }
//    if(neighbors.size() > 0)
//        avgHeading /= (float)neighbors.size();
//    agent->direction = avgHeading.normal();
//    return agent->direction;
//}

//V2f flock(Agent * agent, TemplateVector<Agent*> & neighbors, float neighborRadius) {
//    V2f newDirection;
//    newDirection = separation(agent, neighborRadius, neighbors) * 1.5;
//    newDirection += cohesion(agent, neighbors) * 1.3;
//    newDirection += alignment(agent, neighbors);
//    return newDirection;
//}

//V2f pursue(Agent * agent, Agent * target) {
//    float maxPrediction = 1;
//    float prediction;

//    //calculate dist to target
//    V2f direction = target->body.center - agent->body.center;

//    float distance = direction.magnitude();

//    //calculate current speed
//    float speed = target->velocity.magnitude();

//    //check if speed is too small to give prediction time.
//    if(speed <= distance / maxPrediction) {
//        prediction = maxPrediction;
//    } else {
//        prediction = distance / speed;
//    }
//    V2f futurePos = target->body.center + target->velocity * speed;
//    target->pursueLocation = futurePos;
//    return seek(futurePos, agent);
//}

//V2f evade(Agent * agent, Agent * target) {
//    float maxPrediction = 1;
//    float prediction;

//    //calculate dist to target
//    V2f direction = target->body.center - agent->body.center;
//    float distance = direction.magnitude();

//    //calculate current speed
//    float speed = target->velocity.magnitude();

//    //check if speed is too small to give prediction time.
//    if(speed <= distance / maxPrediction) {
//        prediction = maxPrediction;
//    } else {
//        prediction = distance / speed;
//    }
//    V2f futurePos = target->body.center + target->velocity * speed;
//    target->evadeLocation = futurePos;
//    return flee(futurePos, agent);
//}

//V2f arrive(Agent * agent, V2f target, int a_ms) {
//    float slowingRadius = (agent->velocity.magnitude()*2)/(2*agent->maximumForce);
//    double distance = V2f::distance(agent->body.center, target);
//    if(distance < slowingRadius) {
//        return stop(agent, a_ms);
//    } else {
//        return seek(target, agent);
//    }
//}
