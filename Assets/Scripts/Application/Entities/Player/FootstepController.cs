using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class FootstepController : MonoBehaviour
{
    CharacterController characterController;
    public FirstPersonController firstPersonController;
    public AudioClip jumpStartSound;
    public AudioClip jumpEndSound;
    public AudioClip footstepSound;
    public AudioClip footstepRunningSound;
    public AudioSource audioSource;
    public float stepRate = 0.5f;
    public float stepCoolDown;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        bool isJumping = firstPersonController.movementState == MovementState.Jumping;
        bool isRunning = firstPersonController.movementState == MovementState.Running;
        bool isWalking = firstPersonController.movementState == MovementState.Walking;

        stepCoolDown -= Time.deltaTime;
        audioSource.pitch = 1f + Random.Range(-0.2f, 0.2f);

        if (isJumping && characterController.isGrounded)
        {
            audioSource.PlayOneShot(jumpStartSound, 0.9f);
        }
        
        else if (isWalking is false && stepCoolDown < 0f)
        {
            audioSource.PlayOneShot(isRunning ? footstepRunningSound : footstepSound, 0.9f);
            stepCoolDown = stepRate;
        }
    }
}