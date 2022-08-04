using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private float offset;
    [SerializeField] private Vector3 farAway;
    [SerializeField] private TMP_InputField usernameInputField;
    private GameObject activeCamera;
    private bool oldGamePaused = false;
    private bool isGamePaused;
    private bool runUpdateLoop = false;

    private Vector3 smallScale = new Vector3(.1f, .1f, .1f);
    private Vector3 largeScale = new Vector3(1, 1, 1);
    private float slerpSpeed = 0.5f;

    private AudioSource audioSource;
    [SerializeField] private AudioClip soundEffect;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (runUpdateLoop)
        {
            isGamePaused = PlayerController.isGamePaused;
            if (isGamePaused != oldGamePaused)
            {
                if (isGamePaused)
                {
                    audioSource.PlayOneShot(soundEffect);
                    Transform localPlayerTransform = activeCamera.transform;
                    usernameInputField.enabled = true;
                    this.transform.localScale = smallScale;
                    this.transform.position = localPlayerTransform.position + localPlayerTransform.forward * offset;
                    this.transform.LookAt(localPlayerTransform);
                    this.transform.Rotate(0, 180, 0);
                }
                else
                {
                    usernameInputField.enabled = false;
                    this.transform.position = farAway;
                }
            }
            oldGamePaused = isGamePaused;
        }
    }

    private void FixedUpdate()
    {
        this.transform.localScale = Vector3.Slerp(this.transform.localScale, largeScale, slerpSpeed);
    }

    public void SetActiveCamera(GameObject camera)
    {
        this.transform.position = farAway;
        activeCamera = camera;
        GetComponentInChildren<Canvas>().worldCamera = activeCamera.GetComponent<Camera>();
        runUpdateLoop = true;
    }
}
