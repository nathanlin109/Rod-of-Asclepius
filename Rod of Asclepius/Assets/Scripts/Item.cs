using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Fields
    public bool triggered;
    public bool enemyCurrentlyCaught;
    public bool playedUntrapAnim;

    // Start is called before the first frame update
    void Start()
    {
        // set render queue to properly mask
        if (GetComponent<MeshRenderer>() != null)
        {
            GetComponent<MeshRenderer>().material.renderQueue = 3002;
        }

        MeshRenderer[] childMeshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer meshRenderer in childMeshRenderers)
        {
            meshRenderer.material.renderQueue = 3002;
        }

        triggered = false;
        enemyCurrentlyCaught = false;
        playedUntrapAnim = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
