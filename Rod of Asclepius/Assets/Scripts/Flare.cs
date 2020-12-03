using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flare : Ability
{
    // Fields
    private GameObject vampire;
    private GameObject wizard;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        RemoveEffect();
        if (transform.position.y <= 0 && hasHit == false)
        {
            Destroy(gameObject);
        }
    }

    // Trigger enter
    private void OnTriggerEnter(Collider other)
    {
        if (hasHit == false)
        {
            if (other.gameObject.tag == "Vampire")
            {
                // Visible
                vampire = other.gameObject;

                // Makes this ability invisible
                GetComponentInChildren<MeshRenderer>().enabled = false;
                ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem particle in particles)
                {
                    particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                }

                // Makes vampire visible
                vampire.GetComponent<MeshRenderer>().material.renderQueue = 3000;
                vampire.GetComponent<Vampire>().spotLight.SetActive(true);

                // Particles
                vampire.GetComponent<Vampire>().flareParticles.GetComponent<ParticleSystem>().Clear();
                vampire.GetComponent<Vampire>().flareParticles.GetComponent<ParticleSystem>().Play();
                hasHit = true;

                // Sound
                GameObject.Find("AudioManager").GetComponent<AudioMan>().Play("flare-sound");
            }
            else if (other.gameObject.tag == "Wizard")
            {
                wizard = other.gameObject;

                // Makes this ability invisible
                GetComponentInChildren<MeshRenderer>().enabled = false;
                ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem particle in particles)
                {
                    particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                }

                // Makes wizard visible
                wizard.GetComponent<MeshRenderer>().material.renderQueue = 3000;
                wizard.GetComponent<Wizard>().spotLight.SetActive(true);

                // Particles
                wizard.GetComponent<Wizard>().flareParticles.GetComponent<ParticleSystem>().Clear();
                wizard.GetComponent<Wizard>().flareParticles.GetComponent<ParticleSystem>().Play();
                hasHit = true;

                // Sound
                GameObject.Find("AudioManager").GetComponent<AudioMan>().Play("flare-sound");
            }
        }
    }

    // Remove effect
    void RemoveEffect()
    {
        if (hasHit == true)
        {
            timeActive += Time.deltaTime;

            if (timeActive >= effectTime)
            {               
                // Checks which one got hit
                if (vampire != null)
                {
                    vampire.GetComponent<MeshRenderer>().material.renderQueue = 3002;
                    vampire.GetComponent<Vampire>().spotLight.SetActive(false);
                }
                if (wizard != null)
                {
                    wizard.GetComponent<MeshRenderer>().material.renderQueue = 3002;
                    wizard.GetComponent<Wizard>().spotLight.SetActive(false);
                }

                Destroy(gameObject);
            }
        }
    }
}
