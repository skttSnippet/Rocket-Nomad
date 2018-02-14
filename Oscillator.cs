using UnityEngine;


[DisallowMultipleComponent]//modifying th Oscillator class,in case this Oscillator being added twice on one object
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);//[SerializeField] modifying movementVector
    [SerializeField] float posPeriod = 2f;
    [SerializeField] float posTimeOffset = 0.0f;
    

    [SerializeField] Vector3 rotationVector = new Vector3(10f, 10f, 10f);//[SerializeField] modifying movementVector
    [SerializeField] float rotAmp = 2f;

    [SerializeField] Vector3 scaleVector = new Vector3(1, 1, 1);//[SerializeField] modifying movementVector
    [SerializeField] float scalePeriod = 2f;
    [SerializeField] float scaleTimeOffset = 0.0f;

    //[Range(0,1)]  // modifying movementFactor
    //[SerializeField]  // modifying movementFactor
    float movementFactor;  // 0 for not moved, 1 for fully moved
    float scaleFactor;

    Vector3 startingPos;//must be stored for absolute movement
    Vector3 startingScale;
    const float tau = Mathf.PI * 2f; //about 6.28; 2pi


    // Use this for initialization
    void Start()
    {
        startingPos = transform.position;
        startingScale = transform.localScale;
        
    }

    // Update is called once per frame
    void Update()
    {
        ApplyPostion();
        ApplyRotation();
        ApplyScale();
    }

    //using sin to move obstacle back and forth
    void ApplyPostion()
    {
        //protector when period is 0. Being divided by 0 will pop up error. Epsilon is the smallest float.
        //Epsilon is the smallest number available
        if (posPeriod <= Mathf.Epsilon)
        {
            return;
        }

        float cycles = (Time.time+ posTimeOffset) / posPeriod; // grows continually from 0.
        float rawSinWave = Mathf.Sin(cycles * tau); // rawSinWave between 1 and -1
        //magic happens at this line!!
        movementFactor = rawSinWave / 2f + 0.5f;// Sine goes between 1 and -1. We divide it by 2 and then it becomes 0.5 and -0.5. Shift it by 0.5 to make it 1 and 0.

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }

    //continuously rotate
    void ApplyRotation()
    {
        transform.Rotate(rotationVector * Time.deltaTime * rotAmp);
    }


    void ApplyScale()
    {
        if (scalePeriod <= Mathf.Epsilon)
        {
            return;
        }

        float cycles = (Time.time + scaleTimeOffset) / scalePeriod; // grows continually from 0.
        float rawSinWave = Mathf.Sin(cycles * tau); // rawSinWave between 1 and -1
        scaleFactor = rawSinWave / 2f + 0.5f;

        Vector3 offset = new Vector3(Mathf.Pow(scaleVector.x, scaleFactor),
                                     Mathf.Pow(scaleVector.y, scaleFactor),
                                     Mathf.Pow(scaleVector.z, scaleFactor));
        transform.localScale = Vector3.Scale(startingScale, offset);



        
    }
}






