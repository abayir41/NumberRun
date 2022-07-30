using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Destroyer"))
        {
            if (LevelManager.Instance.ExpPortals.Contains(gameObject))
            {
                LevelManager.Instance.ExpPortals.Remove(gameObject);
            }

            if (LevelManager.Instance.Numbers.Contains(gameObject))
            {
                LevelManager.Instance.Numbers.Remove(gameObject);
            }
            
            Destroy(gameObject);
        }
    }
}
