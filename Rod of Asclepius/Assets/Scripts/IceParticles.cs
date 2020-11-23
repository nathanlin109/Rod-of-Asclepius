using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceParticles : Ability
{
    // Start is called before the first frame update
    protected override void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        // Destroys itself after a few seconds
        timeActive += Time.deltaTime;

        if (timeActive >= effectTime)
        {
            Destroy(gameObject);
        }
    }
}
