using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpoint;
    private Transform currentCheckpoint;
    private Health playerHealth;
    private UIManager uiManager;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void Respawn()
    {
        if (currentCheckpoint == null)
        {
            uiManager.GameOver();
            return;
        }

        playerHealth.Respawn(); // Restore player health and reset animation
        transform.position = currentCheckpoint.position; // Move player to checkpoint location

        // Move the camera to the checkpoint's room
        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            currentCheckpoint = collision.transform;

            // Play sound
            if (SoundManager.instance != null && checkpoint != null)
            {
                SoundManager.instance.PlaySound(checkpoint);
            }

            // Disable collider
            Collider2D collider = collision.GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = false;
            }

            // Set animation trigger
            Animator animator = collision.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("appear");
            }
        }
    }
}
