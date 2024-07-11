using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private float speed;
    [SerializeField] private float speedMin;
    [SerializeField] private float speedMax;
    [SerializeField] private int startingPoint;
    [SerializeField] private Transform[] points;
    private int i;
    private bool isMoving;
    private void Start()
    {
        //isMoving = false;

        StartCoroutine(MovePlatform());

        speed = Random.Range(speedMin, speedMax);
        transform.position = points[startingPoint].position;
    }

    private IEnumerator MovePlatform()
    {
        while (true) // Replace isMoving if needed 
        {
            if (Vector2.Distance(transform.position, points[i].position) < 0.02f)
            {
                i++;
                if (i == points.Length)
                {
                    i = 0;

                    //isMoving = false;
                }
            }
            transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(transform.position.y < collision.transform.position.y - 0.5f)
        {
            collision.transform.SetParent(transform);

            //isMoving = true;

            //StartCoroutine(MovePlatform());
        } 

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);

       // isMoving = false;
       
        //StopCoroutine(MovePlatform());
    }
}
