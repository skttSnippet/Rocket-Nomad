using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    // todo fix lighting 

    [SerializeField] float rcsThrust = 100f;// when initialize a "float", the number has to end with a "f"
    [SerializeField] float mainThrust = 100f;// either "[SerializeField]" or "public" will make var accessible in Unity inspector
    [SerializeField] float levelLoadDelay = 2.5f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip death;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending};
    State state = State.Alive;
    bool collisionsDisabled = false; //Collision is on by default

    // Use this for initialization
    void Start ()
    {
        rigidBody   = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }

        if (Debug.isDebugBuild)//when "development build" check box is on in Build setting
        {
            RespondToDebugKeys();
        }
        

    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled; // var = !var. if true, go false. if false, go true. Basically like a toggle
        }

    }

    //when "This object" hitting other things
    void OnCollisionEnter(Collision collision)
    {

        if (state != State.Alive || collisionsDisabled) {return;}

        switch (collision.gameObject.tag)//switch based on the collided object's tag
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:// anything else except above, kill player
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);//takes a string of a method as argument. "1f" means 1 (float) sec.
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        deathParticles.Play();
        mainEngineParticles.Stop();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);// todo allow for more than 2 levels
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))//can thrust while rotating
        {
            ApplyThrust();
        }
        else
        {
            StopApplyingThrust();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);// made frame rate independant
        if (!audioSource.isPlaying)// "!" means "not". just so it doesn't layer
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }

    private void StopApplyingThrust()
    {
        audioSource.Stop();
        mainEngineParticles.Stop();
    }

    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true; // physics rotation off when manual manul rotation comes in 
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; // physics rotation on again when rotation is over
    }



}
