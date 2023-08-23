# RPG-Game-AI
Este es un juego que usa la api de OpenAI para generar los dialogos en base a lo que el usuario les dice.
La idea es lograr que los dialogos sean mas naturales y que haya una conversacion para lograr una información.

Se debe crear el archivo ConstantManager.cs con el siguiente contenido para que funcione correctamente:

```
using UnityEngine;

public static class ConstantManager
{
    public const string apiKey = "sk-.................";
    public const string apiUrl = "https://api.openai.com/v1/chat/completions";
}
```

Reemplaza la apiKey por una tuya generada en https://platform.openai.com/
