using UnityEngine;

public class NPCController : MonoBehaviour
{
    public Actor actor;
    public string specifications;
    // Start is called before the first frame update
    private OpenAICommunication _ai;
    void Start()
    {
        _ai = GetComponent<OpenAICommunication>();
        _ai.setSpecifications(specifications);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AskQuestion(Message message)
    {
        _ai.AskQuestion(message, actor);
    }
}
