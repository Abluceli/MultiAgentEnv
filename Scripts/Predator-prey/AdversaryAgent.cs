using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using MLAgents.CommunicatorObjects;
using System.Linq;
using System;

public class AdversaryAgent : Agent
{

    public Rigidbody agentRb;
    public Transform[] Adversarys;
    public Transform[] Goods;
    public Transform[] Landmarks;
    public Rigidbody[] AdversarysRB;
    public Rigidbody[] GoodsRB;
    public Transform Ground;
    //public Transform Landmark;
    // Speed of agent movement.
    public float moveSpeed;



    //private void Start()
    //{
    //    agentRb = GetComponent<Rigidbody>();
    //}


    public override void CollectObservations()
    {

        // Target and Agent positions
        foreach (Transform adversary in this.Adversarys)
        {
            AddVectorObs(adversary.position);
        }
        foreach(Transform good in this.Goods)
        {
            AddVectorObs(good.position);
        }
        foreach(Transform landmark in this.Landmarks)
        {
            AddVectorObs(landmark.position);
        }
        foreach(Rigidbody rigidbody in this.AdversarysRB)
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
        //Debug.Log(controlSignal);

        this.agentRb.AddForce(controlSignal * moveSpeed);

        //float distanceTolandmark = Vector3.Distance(this.transform.position, this.Landmark.position);
        //float distancegoodTolandmark = Vector3.Distance(this.Landmark.position, this.Good.position);
        /*float distanceTogood = Vector3.Distance(this.transform.position, this.Good.position);
        AddReward(- distanceTogood / 2000f);*/
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
        if (collision.gameObject.CompareTag("Good"))
        {
            AddReward(10f);
            //Done();
        }
/*        if (collision.gameObject.CompareTag("Adversary"))
        {
            AddReward(-0.5f);
        }*/
    }

    /* def adversary_reward(self, agent, world):
         # Adversaries are rewarded for collisions with agents
         rew = 0
         shape = False
         agents = self.good_agents(world)
         adversaries = self.adversaries(world)
         if shape:  # reward can optionally be shaped (decreased reward for increased distance from agents)
             for adv in adversaries:
                 rew -= 0.1 * min([np.sqrt(np.sum(np.square(a.state.p_pos - adv.state.p_pos))) for a in agents])
         if agent.collide:
             for ag in agents:
                 for adv in adversaries:
                     if self.is_collision(ag, adv):
                         rew += 10
         return rew*/
    public void Agent_reward()
    {
        //float reward = - 0.01f * Vector3.Distance(this.transform.position, this.Good.position);
        float reward = 0f;
        bool shape = false;
        if(shape)
        {
            float[] res = new float[this.Goods.Length];
            foreach (Transform adversary in this.Adversarys)
            {
                for(int i=0; i<this.Goods.Length; i++)
                {
                    //res.Append(Vector3.Distance(adversary.position, good.position));
                    res[i] = Vector3.Distance(adversary.position, Goods[i].position);
                    //reward -= 0.01f * Vector3.Distance(adversary.position, good.position);
                }
                
                reward -= 0.01f * res.Min();
                //Array.Clear(res, 0, res.Length);
            }
        }
        AddReward(reward);
    }
  
}
