using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObject : MonoBehaviour
{
    private static DontDestroyObject instance;

    private void Awake()
    {
        // Check if an instance already exists
        if (instance == null)
        {
            // If no instance exists, set this object as the instance and mark it as don't destroy on load
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists, destroy this object
            Destroy(gameObject);
        }
    }
}
