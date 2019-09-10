using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class GoodAgent : Agent
{

    //public GameObject myArea;

    public Rigidbody agentRb;
    public Transform[] Adversarys;
    public Transform[] Goods;
    public Transform[] Landmarks;
    public Rigidbody[] AdversarysRB;
    public Rigidbody[] GoodsRB;
    //public Transform Target;
    public Transform Ground;
    // Speed of agent movement.
    public float moveSpeed;





    public override void CollectObservations()
    {

        foreach (Transform adversary in this.Adversarys)
        {
            AddVectorObs(adversary.position);
        }
        foreach (Transform good in this.Goods)
        {
            AddVectorObs(good.position);
        }
        foreach (Transform landmark in this.Landmarks)
        {
            AddVectorObs(landmark.position);
        }
        foreach (Rigidbody rigidbody in this.AdversarysRB)
        {
            AddVectorObs(rigidbody.velocity);
        }
        foreach (Rigidbody rigidbody in this.GoodsRB)
        {
            AddVectorObs(rigidbody.velocity);
        }

    }




    public override void AgentAction(float[] vectorAction, string textAction)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = Mathf.Clamp(vectorAction[0], -1f, 1f);
        controlSignal.z = Mathf.Clamp(vectorAction[1], -1f, 1f);
        agentRb.AddForce(controlSignal * moveSpeed);

        /*float distance = Vector3.Distance(this.transform.position, this.Target.position);
        AddReward(-distance/2000f);*/
        Agent_reward();
    }

    public override void AgentReset()
    {
        agentRb.velocity = Vector3.zero;
        transform.position = new Vector3(UnityEngine.Random.Range(-9, 9),
                                         0.5f, UnityEngine.Random.Range(-9, 9))
            + this.Ground.position;

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            AddReward(-1f);
        }
        if (collision.gameObject.CompareTag("Landmark"))
        {
            AddReward(-1f);
        }
        if (collision.gameObject.CompareTag("Adversary"))
        {
            AddReward(-10f);
            //this.Done();
        }
    }
    /* def agent_reward(self, agent, world):
     # Agents are negatively rewarded if caught by adversaries
     rew = 0
     shape = False
     adversaries = self.adversaries(world)
     if shape:  # reward can optionally be shaped (increased reward for increased distance from adversary)
         for adv in adversaries:
             rew += 0.1 * np.sqrt(np.sum(np.square(agent.state.p_pos - adv.state.p_pos)))
     if agent.collide:
         for a in adversaries:
             if self.is_collision(a, agent):
                 rew -= 10

     # agents are penalized for exiting the screen, so that they can be caught by the adversaries
     def bound(x):
         if x < 0.9:
             return 0
         if x < 1.0:
             return (x - 0.9) * 10
         return min(np.exp(2 * x - 2), 10)
     for p in range(world.dim_p):
         x = abs(agent.state.p_pos[p])
         rew -= bound(x)

     return rew*/

    public void Agent_reward()
    {
        float reward = 0f;
        bool shape = false;
        if (shape)
        {
            foreach (Transform adversary in this.Adversarys)
            {
                reward += 0.01f * Vector3.Distance(this.transform.position, adversary.position);

            }
        }
        AddReward(reward);
    }

}
