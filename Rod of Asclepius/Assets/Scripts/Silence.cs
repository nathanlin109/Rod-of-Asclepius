using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silence : Ability
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
                vampire = other.gameObject;
                GetComponent<MeshRenderer>().enabled = false;
                vampire.GetComponent<Vampire>().silenced = true;
                hasHit = true;
            }
            else if (other.gameObject.tag == "Wizard")
            {
                wizard = other.gameObject;
                GetComponent<MeshRenderer>().enabled = false;
                wizard.GetComponent<Wizard>().silenced = true;
                hasHit = true;
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
                     vampire.GetComponent<Vampire>().silenced = false;
                }
                if (wizard != null)
                {
                    wizard.GetComponent<Wizard>().silenced = false;
                }

                Destroy(gameObject);
            }
        }
    }
}
