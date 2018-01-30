using UnityEngine;




[DisallowMultipleComponent]//modifying th Oscillator class,in case this Oscillator being added twice on one object
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVectorPos = new Vector3(10f, 10f, 10f);//[SerializeField] modifying movementVector
    [SerializeField] float posPeriod = 2f;
    [SerializeField] Vector3 movementVectorRot = new Vector3(10f, 10f, 10f);//[SerializeField] modifying movementVector
    [SerializeField] float rotAmp = 2f;

    //[Range(0,1)]  // modifying movementFactor
    //[SerializeField]  // modifying movementFactor
    float movementFactorPos;  // 0 for not moved, 1 for fully moved

    Vector3 startingPos;//must be stored for absolute movement

    // Use this for initialization
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ApplyPostion();
        ApplyRotation();
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

        float cycles = Time.time / posPeriod; // grows continually from 0.
        const float tau = Mathf.PI * 2f; //about 6.28; 2pi
        float rawSinWave = Mathf.Sin(cycles * tau); // rawSinWave between 1 and -1

        //magic happens at this line!!
        movementFactorPos = rawSinWave / 2f + 0.5f;// Sine goes between 1 and -1. We divide it by 2 and then it becomes 0.5 and -0.5. Shift it by 0.5 to make it 1 and 0.

        Vector3 offset = movementVectorPos * movementFactorPos;
        transform.position = startingPos + offset;
    }

    //continuously rotate
    void ApplyRotation()
    {
        transform.Rotate( movementVectorRot * Time.deltaTime * rotAmp);
    }

}
