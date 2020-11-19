using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAbility : Ability
{
    // Fields
    public GameObject iceParticlesPrefab;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    // Collision enter
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Instantiate(iceParticlesPrefab, transform.position.normalized, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
