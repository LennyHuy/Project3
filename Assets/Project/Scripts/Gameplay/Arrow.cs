using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    LineRenderer lineRenderer;
    bool isSwingingUp = true;
    Vector2 startDragPos;
    public static PlayerState playerState;
    [Header("Angle")]
    [SerializeField] float currentValue = 0;
    
    [SerializeField] float speedCurrentValue;
    [SerializeField] public float minAngle, maxAngle;

    public static float speedValue = 1 ;

    Vector3 Position;

    public Vector3 direction 
    {
        get
        {
            float progress = (currentValue+1)/2f;
            float currentAngle = Mathf.Lerp(minAngle,maxAngle,progress);
            float currentRadian = currentAngle* Mathf.Deg2Rad;
            Vector3 currentVector = new Vector3(Mathf.Cos(currentRadian),Mathf.Sin(currentRadian),0);
            return currentVector.normalized;
        }
    }
    void Awake()
    {
        lineRenderer = this.GetComponent<LineRenderer>();
    }

    public void TurnOn()
    {
        lineRenderer.enabled = true; 

        StopAllCoroutines();
        
        if (gameObject.activeInHierarchy) StartCoroutine(SwingBackAndForth(currentValue));
    }
    public void TurnOff()
    {
        StopAllCoroutines();
        lineRenderer.enabled = false;
    }
    IEnumerator SwingBackAndForth(float initialValue)
    {
        speedCurrentValue = speedValue;
        currentValue = initialValue;
        int direction = 1;
        if (!isSwingingUp) direction = -1;
        while (true)
        {
            currentValue+= Time.deltaTime*speedCurrentValue*direction;
            if(currentValue > 1 || currentValue < -1 )
            {
                isSwingingUp =!isSwingingUp;
                direction =-direction;
                currentValue = currentValue -(currentValue%1);
            }
            SetLineAt(currentValue);

            yield return null;
            
        }
    }
    void SetLineAt(float value)
    {
        value = (value/2) +.5f;
        float currentAngle =  Mathf.Lerp (minAngle,maxAngle,value);
        float currentRadian = currentAngle* Mathf.Deg2Rad;

        Position = Vector3.zero;
        Position.x = Mathf.Cos(currentRadian);
        Position.y = Mathf.Sin(currentRadian);

        //Position.normalize

        lineRenderer.SetPosition(1,Position);
        
    }
public Vector2[] Plot(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps)
{
    Vector2[] results = new Vector2[steps];

    float timeStep = 0.01f; // variable timestep
    Vector2 gravityAccel = Physics2D.gravity * rigidbody.gravityScale;

    for (int i = 0; i < steps; i++)
    {
        results[i] = pos;
        velocity += gravityAccel * timeStep;
        pos += velocity * timeStep;
        RaycastHit2D hit = Physics2D.Raycast(pos, velocity.normalized, velocity.magnitude * timeStep);
        if (hit.collider != null)
        {
            results[i] = hit.point;
            break;
        }
    }

    return results;
}
}
