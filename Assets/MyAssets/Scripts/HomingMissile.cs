﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour {
    public float speed;
    public float turningRate;
    public BulletImpactController explosion;
    public ParticleSystem thruster;
    public GameObject target;
    public float damage = 1.0f;
    public float lifeTime = 10.0f;

    private Rigidbody missileRigidBody;

    // Use this for initialization
    void Start () {
        missileRigidBody = gameObject.GetComponent<Rigidbody>();
        thruster.Play();
    }

    void Update()
    {
        lifeTime -= Time.deltaTime;

        if(lifeTime < 0)
        {
            Detonate();
        }
    }

    void FixedUpdate()
    {
        missileRigidBody.velocity = transform.forward * speed;

        var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);

        missileRigidBody.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turningRate));
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("AutoTurret"))
        {
            ShipHealth shipHealth = other.gameObject.GetComponentInParent<ShipHealth>();
            if(shipHealth != null)
            {
                shipHealth.TakeDamage(damage);
            }
            Detonate();
        }
    }

    private void Detonate()
    {
        explosion.Play();
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}
