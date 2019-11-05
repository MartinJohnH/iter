using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("unlock door");
        Usable usable = other.gameObject.GetComponent<Usable>();
        if (usable)
        {
            usable.Use();
        }
    }
    

}
