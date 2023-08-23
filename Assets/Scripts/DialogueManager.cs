using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Image actorImage;
    public Text actorName;
    public Text messageText;
    public RectTransform backgroundBox;

    Message[] currentMessages;
    Actor[] currentActors;
    int activeMessages = 0;
    public static bool isActive;
    public static bool moving = true;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isActive)
            NextMessage();

        if (!moving)
            return;

        if (isActive)
            ShowObject();
        else
            HideObject();

        moving = !moving;
    }


    public void OpenMultipleDialogue(Message[] messages, Actor[] actors)
    {
        currentMessages = messages;
        currentActors = actors;
        activeMessages = 0;
        isActive = true;
        DisplayMessage();
    }
    public void OpenMultipleDialogueOneActor(Message[] messages, Actor actor)
    {
        currentMessages = messages;
        List<Actor> actorList = new List<Actor>();
        foreach (Message msg in messages)
        {
            actorList.Add(actor);
        }
        Actor[] actorsArray = actorList.ToArray();
        currentActors = actorsArray;
        activeMessages = 0;
        isActive = true;
        moving = true;
        DisplayMessage();
    }
    public void OpenDialogue(Message message, Actor actor)
    {
        currentMessages = new Message[] { message };
        currentActors = new Actor[] { actor };
        activeMessages = 0;
        isActive = true;
        moving = true;
        DisplayMessage();
    }

    public void DisplayMessage()
    {
        Message messageToDisplay = currentMessages[activeMessages];
        messageText.text = messageToDisplay.message;

        Actor actorToDisplay = currentActors[activeMessages];
        actorName.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.sprite;
    }

    public void NextMessage()
    {
        activeMessages++;
        if (activeMessages < currentMessages.Length)
            DisplayMessage();
        else
        {
            moving = true;
            isActive = false;
        }
    }
    public void ShowObject()
    {
        var position = transform.position;
        position.Set(position.x, position.y - 5000, position.z);
        transform.position = position;
    }
    public void HideObject()
    {
        var position = transform.position;
        position.Set(position.x, position.y + 5000, position.z);
        transform.position = position;
    }
}
