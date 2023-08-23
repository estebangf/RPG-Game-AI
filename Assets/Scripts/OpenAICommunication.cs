using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Unity.Serialization.Json;
using System.Linq;

public class OpenAICommunication : MonoBehaviour
{
    private List<string> conversation = new List<string>();

    public bool inProgress;

    private void Start()
    {
    }

    public void setSpecifications(string specifications)
    {
        conversation.Add(specifications);
    }

    IEnumerator SendQuestion(Message message, Actor actor)
    {
        // Configurar el encabezado de autorizaci�n
        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "Content-Type", "application/json" },
            { "Authorization", "Bearer " + ConstantManager.apiKey }
        };

        MessageOnApi[] messages = new MessageOnApi[] { new MessageOnApi(), new MessageOnApi() };
        messages[0].role = "system";
        messages[0].content = string.Join("\n", conversation.ToArray());
        messages[1].role = "user";
        messages[1].content = message.message;

        ApiCall apicall = new ApiCall();
        apicall.model = "gpt-3.5-turbo";
        apicall.messages = messages;
        string json = JsonSerialization.ToJson(apicall);

        // Enviar la solicitud POST a la API de OpenAI
        //using (UnityWebRequest www = UnityWebRequest.Post(apiUrl, json, "application/json"))
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(ConstantManager.apiUrl, json))
        {
            www.method = "POST";
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            foreach (var header in headers)
            {
                www.SetRequestHeader(header.Key, header.Value);
            }

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error en la solicitud: " + www.error);
            }
            else
            {
                string responseJson = www.downloadHandler.text;
                // Procesar la respuesta JSON aqu� (extraer y mostrar la respuesta del asistente)
                Debug.Log("Response: " + responseJson);
                ResponseApiCall responseApiCall = JsonSerialization.FromJson<ResponseApiCall>(responseJson);
                MessageOnApi response = responseApiCall.choices[0].message;

                Message messageResponse = new Message();
                messageResponse.message = response.content;
                conversation.Add($"user: {message.message}");
                conversation.Add($"{response.role}: {response.content}");

                //FindObjectOfType<DialogueManager>().OpenDialogue(messageResponse, actor);
                string[] messagesResponse = response.content.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                Debug.Log("Length: " + messagesResponse.Length);
                foreach (var item in messagesResponse)
                {
                    Debug.Log("msg: " + item);
                }

                List<Message> msgs = new List<Message>();
                foreach (string msgstr in messagesResponse)
                {
                    Message msg = new Message();
                    msg.message = msgstr;
                    msgs.Add(msg);
                }
                Message[] messagesList = msgs.ToArray();
                
                if (messagesList.Length > 1)
                   FindObjectOfType<DialogueManager>().OpenMultipleDialogueOneActor(messagesList, actor);
                else
                    FindObjectOfType<DialogueManager>().OpenDialogue(messageResponse, actor);
            }
            inProgress = false;
        }
    }

    //public void AskQuestion(string question)
    public void AskQuestion(Message message, Actor actor)
    {
        inProgress = true;
        StartCoroutine(SendQuestion(message, actor));
    }
}

[System.Serializable]
public class MessageOnApi
{
    public string role;
    public string content;
}
[System.Serializable]
public class ApiCall
{
    public string model;
    public MessageOnApi[] messages;
    public int max_tokens = 50; // Largo de la respuesta.
    public float temperature = 0.3f; // Creatividad y riesgos.
}

[System.Serializable]
public class ResponseApiCall
{
    public string id, created, model;
    public Choices[] choices;
    public Usage usage;
}
[System.Serializable]
public class Usage {
    public int prompt_tokens, completion_tokens, total_tokens;
}
[System.Serializable]
public class Choices
{
    public int index;
    public MessageOnApi message;
    public string finish_reason;
}


/*
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;

public class OpenAICommunication : MonoBehaviour
{

    private void Update()
    {
        if(Input.GetButtonDown("Jump"))
            AskQuestion("Hola! Este es un juego de unity!");
    }

    IEnumerator SendQuestion(string question)
    {
        MessagesList messages = new MessagesList();
        messages.list = new MessageOnApi[] { new MessageOnApi() };
        messages.list[0].role = "user";
        messages.list[0].content = question;
        ApiCall apiCall = new ApiCall();
        apiCall.model = "text-davinci-003";
        apiCall.prompt = question;

        string json = JsonUtility.ToJson(apiCall);
        Debug.Log("JSON: " + json);

        // Configurar el encabezado de autorizaci�n
        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "Content-Type", "application/json" },
            { "Authorization", "Bearer " + apiKey }
        };

        // Enviar la solicitud POST a la API de OpenAI
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(apiUrl, json))
        {
            www.method = "POST";
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            foreach (var header in headers)
            {
                www.SetRequestHeader(header.Key, header.Value);
            }

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error en la solicitud: " + www.error);
            }
            else
            {
                string responseJson = www.downloadHandler.text;
                // Procesar la respuesta JSON aqu� (extraer y mostrar la respuesta del asistente)
                Debug.Log("Response: " + responseJson);
                ResponseApiCall responseApiCall = JsonUtility.FromJson<ResponseApiCall>(responseJson);
                Debug.Log(JsonUtility.ToJson(responseApiCall));
                Debug.Log("Response: " + responseApiCall.choices[0].text);
            }
        }
    }

    public void AskQuestion(string question)
    {
        StartCoroutine(SendQuestion(question));
    }
}

[System.Serializable]
public class MessageOnApi
{
    public string role;
    public string content;
}
public class MessagesList
{
    public MessageOnApi[] list;
}
public class ApiCall
{
    public string model;
    public string prompt;
    public float temperature = 0.7f;
}
public class ResponseApiCall
{
    public string id, created, model;
    public Choices[] choices = new Choices[] { new Choices() };
    public Usage usage = new Usage();
}
public class Usage
{
    public int prompt_tokens, completion_tokens, total_tokens;
}
public class Choices
{
    public int index;
    public string text;
    public string finish_reason;
}

*/
