using UnityEngine;


public class Night : MonoBehaviour {

    [SerializeField] ParticleSystem psSystem;
    [SerializeField] bool usePrewarm;

    // Use this for initialization
    void Start ()
    {
        psSystem = GetComponent<ParticleSystem>();
        var main = psSystem.main;
        main.loop = true;

        Restart();

    }

    void OnGUI()
    {
        bool newPrewarm = GUI.Toggle(new Rect(10, 60, 200, 30), usePrewarm, "Use Prewarm");

        if (newPrewarm != usePrewarm)
        {
            usePrewarm = newPrewarm;
            Restart();
        }
    }

    private void Restart()
    {
        psSystem.Stop();
        psSystem.Clear();

        var main = psSystem.main;
        main.prewarm = usePrewarm;

        psSystem.Play();
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}

