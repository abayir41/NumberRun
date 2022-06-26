using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    public Portal[] myPortals;

    public void SetPortals(int yearOne, int yearTwo)
    {
        var spawnPossibility = Random.Range(0, 100);
        if (spawnPossibility < GameManager.Instance.onePortalSpawnRate)
        {
            var randomPortal = Random.Range(0, 100);
            if (randomPortal == 0)
            {
                var randomSpawn0 = Random.Range(0, 100);
                if (randomSpawn0 < GameManager.Instance.badPortalRate)
                    randomSpawn0 = 0;
                else
                {
                    randomSpawn0 = 1;
                }
                myPortals[randomSpawn0].gameObject.SetActive(true);
                myPortals[randomSpawn0].SetPortal(yearOne);
            }
            else
            {
                var randomSpawn1 = Random.Range(0, 100);
                if (randomSpawn1 < GameManager.Instance.badPortalRate)
                    randomSpawn1 = 3;
                else
                {
                    randomSpawn1 = 2;
                } 
                myPortals[randomSpawn1].gameObject.SetActive(true);
                myPortals[randomSpawn1].SetPortal(yearTwo);
            }
        }
        else
        {
            
            var randomSpawn0 = Random.Range(0, 100);
            if (randomSpawn0 < GameManager.Instance.badPortalRate)
                randomSpawn0 = 0;
            else
            {
                randomSpawn0 = 1;
            }
            myPortals[randomSpawn0].gameObject.SetActive(true);
            myPortals[randomSpawn0].SetPortal(yearOne);
        
            var randomSpawn1 = Random.Range(0, 100);
            if (randomSpawn1 < GameManager.Instance.badPortalRate)
                randomSpawn1 = 3;
            else
            {
                randomSpawn1 = 2;
            }
            myPortals[randomSpawn1].gameObject.SetActive(true);
            myPortals[randomSpawn1].SetPortal(yearTwo);
        }
        
    }
}
