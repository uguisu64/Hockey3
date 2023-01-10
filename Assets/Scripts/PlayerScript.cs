using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private float speed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.up * Time.deltaTime * speed;
            if(transform.position.y > 3.0f)
            {
                transform.position = new Vector3(transform.position.x, 3.0f, transform.position.z);
            }
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.down * Time.deltaTime * speed;
            if (transform.position.y < -3.0f)
            {
                transform.position = new Vector3(transform.position.x, -3.0f, transform.position.z);
            }
        }
    }
}
