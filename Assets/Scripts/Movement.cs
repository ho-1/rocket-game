using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Debug = UnityEngine.Debug;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 700f;
    [SerializeField] float rotationThrust = 100f;
    [SerializeField] private AudioClip mainEngine;
    
    [SerializeField] private ParticleSystem mainBoostParticles;
    [SerializeField] private ParticleSystem rightBoostParticles;
    [SerializeField] private ParticleSystem leftBoostParticles;
    
    Rigidbody rb;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);
            }

            if (!mainBoostParticles.isPlaying)
            {
                mainBoostParticles.Play();
            }
        }
        else
        {
            mainBoostParticles.Stop();
            audioSource.Stop();
        }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            ApplyRotation(rotationThrust);
            if (!rightBoostParticles.isPlaying)
            {
                rightBoostParticles.Play();
            }
        } 
        else if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-rotationThrust);
            if (!leftBoostParticles.isPlaying)
            {
                leftBoostParticles.Play();
            }
        }
        else
        {
            rightBoostParticles.Stop();
            leftBoostParticles.Stop();
        }
    }

    public void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;  // freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false;  // unfreezing rotation so the physics system can take over
    }
}
