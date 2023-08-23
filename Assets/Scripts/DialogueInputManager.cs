using UnityEngine;
using UnityEngine.UI;

public class DialogueInputManager : MonoBehaviour
{
    public InputField inputField; // Asigna el objeto Input Field en el Inspector

    public Image actorImage;
    public Text actorName;
    public RectTransform backgroundBox;

    Actor currentActor;
    public static bool isActive;

    private Vector3 hiddenPosition;
    private Vector3 viewPosition;

    public PlayerController player;

    public static bool moving = true;

    // Start is called before the first frame update
    void Start()
    {
        hiddenPosition = transform.position;
        hiddenPosition.y = hiddenPosition.y - 5000;
        viewPosition = transform.position;
        inputField.onEndEdit.AddListener(HandleTextEntered);
    }

    // Update is called once per frame
    void Update()
    {
        if (!moving)
            return;

        if (isActive)
            ShowObject();
        else
            HideObject();

        moving = !moving;
    }
    public void OpenDialogueInput(Actor actor)
    {
        currentActor = actor;
        isActive = true;
        moving = true;
        DisplayInput();
    }

    public void DisplayInput()
    {
        actorName.text = currentActor.name;
        actorImage.sprite = currentActor.sprite;
        inputField.Select();
        inputField.ActivateInputField();
    }

    private void HandleTextEntered(string enteredText)
    {
        Message message = new Message();
        message.message = enteredText;
        player.SendToNPC(message);
        isActive = false;
        moving = true;
    }


    public void ShowObject()
    {
        var position = transform.position;
        position.Set(position.x, position.y - 5000, position.z);
        transform.position = position;
    }
    public void HideObject()
    {
        ClearInput();
        var position = transform.position;
        position.Set(position.x, position.y + 5000, position.z);
        transform.position = position;
    }
    public void ClearInput()
    {
        inputField.text = ""; // Borra el contenido del campo de entrada
    }
}
