using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float strartPos;
    private float length;
    private GameObject cam;
    [SerializeField] private float parallaxValue;
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        strartPos = transform.position.x;
        length = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float temp = (cam.transform.position.x * (1-parallaxValue));
        float distance = (cam.transform.position.x * (parallaxValue));

        transform.position = new Vector3(strartPos + distance, transform.position.y, transform.position.z);

        if(temp > strartPos + length) strartPos += length; // Move onto the right
    }
}
