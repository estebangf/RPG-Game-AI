using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Actor actor;
    public float speed = 5f; // Velocidad de movimiento del personaje
    float horizontalInput;
    float verticalInput;

    private Animator _animator;
    private Rigidbody2D _rigidbody;

    private GameObject npcInRangeToVision;

    void Start()
    {
        horizontalInput = 0;
        verticalInput = 0;
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (npcInRangeToVision != null)
            if (npcInRangeToVision.GetComponent<OpenAICommunication>().inProgress)
                return;

        if (DialogueManager.isActive || DialogueInputManager.isActive)
        {
            horizontalInput = 0;
            verticalInput = 0;
            _rigidbody.velocity = Vector2.zero;
            AplyAnimation();
            
            return;
        }

        // Obtener entrada del teclado
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Aplicar el movimiento al GameObject
        Vector2 velocity = new Vector2(horizontalInput, verticalInput);
        _rigidbody.velocity = velocity * speed;

        AplyAnimation();

        if (npcInRangeToVision != null && Input.GetKeyDown(KeyCode.Space) && (horizontalInput == 0 || verticalInput == 0))
            StartDialogue();
    }
    public void StartDialogue()
    {
        FindObjectOfType<DialogueInputManager>().OpenDialogueInput(actor);
    }
    public void SendToNPC(Message message)
    {
        npcInRangeToVision.GetComponent<NPCController>().AskQuestion(message);
    }

    private void AplyAnimation()
    {
        _animator.SetBool("walking", horizontalInput != 0 || verticalInput != 0);

        if (horizontalInput == 0 && verticalInput == 0)
            return;

        _animator.SetFloat("x", horizontalInput > 0 ? 1 : horizontalInput < 0 ? -1 : horizontalInput);
        _animator.SetFloat("y", verticalInput > 0 ? 1 : verticalInput < 0 ? -1 : verticalInput);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
            npcInRangeToVision = other.gameObject;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
            npcInRangeToVision = null;
    }
}