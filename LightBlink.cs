using UnityEngine;

public class LightBlink : MonoBehaviour {

    Color newColor;
    Renderer rend;

    float duration = 0.5F;
    float lerp;//linear interpolation called lerp. But I don't use Color.Lerp here
    bool randomSwitch;

    // Use this for initialization
    void Start ()
    {
        rend = GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        float lerp = Mathf.PingPong(Time.time, duration) / duration;

        newColor = rend.material.color;
        newColor.a = lerp;

        rend.material.color = newColor;
    }
}
