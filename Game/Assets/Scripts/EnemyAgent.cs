using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class EnemyAgent : Agent
{
    private Rigidbody rbody;
    public Transform Hedef;
    public float carpan = 7f;
    public float takipAlanı = 10f; 

    private Vector3 ajanBaşlangıçKonumu;
    private Vector3 hedefBaşlangıçKonumu;

    void Start()
    {
        rbody = GetComponent<Rigidbody>();

        
        ajanBaşlangıçKonumu = transform.localPosition;
        hedefBaşlangıçKonumu = Hedef.localPosition;
    }

    public override void OnEpisodeBegin()
    {
        
        transform.localPosition = ajanBaşlangıçKonumu;
        Hedef.localPosition = hedefBaşlangıçKonumu;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Hedef.localPosition);
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(rbody.velocity.x);
        sensor.AddObservation(rbody.velocity.z);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Vector3 kontrolSinyali = Vector3.zero;
        kontrolSinyali.x = actions.ContinuousActions[0];
        kontrolSinyali.z = actions.ContinuousActions[1];

        
        rbody.AddForce(kontrolSinyali * carpan);

        
        float uzaklık = Vector3.Distance(transform.localPosition, Hedef.localPosition);

        if (uzaklık < 1.5f)
        {
            
            SetReward(1.0f);
            
        }
        else if (uzaklık <= takipAlanı)
        {
            
            SetReward(0.1f);
            
            Vector3 hareketYonu = (Hedef.localPosition - transform.localPosition).normalized;
            rbody.AddForce(hareketYonu * carpan);
        }
        else
        {
            
            SetReward(-0.01f);
        }

    }
}
