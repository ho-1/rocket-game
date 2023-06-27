using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float reloadTime = 2f;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip failSound;

    [SerializeField] private ParticleSystem successParticles;
    [SerializeField] private ParticleSystem crashParticles;

    private AudioSource audioSource;

    private bool isCrushed = false;
    private bool isTransitioning = false;
    private bool collisionDisabled = false;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled; // toggle collision
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || collisionDisabled) { return; }
        
        switch (other.gameObject.tag)
        {
            case "Respawn":
                Debug.Log("This thing is friendly");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence(); 
                break;
        }
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.PlayOneShot(failSound);
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", reloadTime);
    }

    void StartSuccessSequence()
    {
        isTransitioning = true;
        audioSource.PlayOneShot(successSound);
        successParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", reloadTime);
    }
    
    void LoadNextLevel()
    {
        audioSource.Stop();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        
        // reset variable
        GetComponent<Movement>().enabled = true;
        isTransitioning = false;
        
        SceneManager.LoadScene(nextSceneIndex);
    }
    
    void ReloadLevel()
    {
        audioSource.Stop();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
