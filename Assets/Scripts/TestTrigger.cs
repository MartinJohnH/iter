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
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("unlock door!!" + other.gameObject.tag);
        Usable usable = other.gameObject.GetComponent<Usable>();
        if (usable)
        {
            usable.Use();
        }

        if (other.gameObject.tag == "puzzleArea3")
        {
            OpenDoor2.pressurePlateCounter++;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "puzzleArea3")
        {
            OpenDoor2.pressurePlateCounter--;
        }
    }
    

}
