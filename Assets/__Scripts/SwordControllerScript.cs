using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordControllerScript : MonoBehaviour
{
    private GameObject sword;
    private DrayScript drayScript;

    // Start is called before the first frame update
    void Start()
    {
        Transform swordT = transform.Find("Sword");
        if(swordT == null)
        {
            Debug.LogError("Could not find Sword");
            return;
        }
        sword = swordT.gameObject;
        drayScript = transform.GetComponentInParent<DrayScript>();
        if(drayScript == null)
        {
            Debug.LogError("Could not find Dray Script");
            return;
        }
        sword.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 90 * drayScript.facing);
        sword.SetActive(drayScript.mode == DrayScript.eMode.attack);
    }
}
