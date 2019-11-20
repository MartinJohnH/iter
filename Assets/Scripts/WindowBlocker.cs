using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowBlocker : MonoBehaviour
{
    private bool isWindowOpended = true;
    public float openingSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWindowOpended &&  transform.position.y >= 35)
        {
            transform.position -= new Vector3(0, openingSpeed  * Time.deltaTime, 0);

        }
    }
    public void closeWindow()
    {
        isWindowOpended = false; 
    }
}
