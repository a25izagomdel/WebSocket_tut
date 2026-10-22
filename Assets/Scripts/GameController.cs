using UnityEngine;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    [SerializeField] private UIDocument menuDocument;
    [SerializeField] private WebSocketClient wsClient; 

    private TextField nickname;
    private TextField missatge;
    private ScrollView scrollView;

    void Start()
    {
        VisualElement root = menuDocument.rootVisualElement;
        Button sendButton = root.Q<Button>("btnEnviar");
        nickname = root.Q<TextField>("nickname");
        missatge = root.Q<TextField>("missatge");
        scrollView = root.Q<ScrollView>("missatgeList");

        sendButton.clicked += OnSendClicked;
        wsClient.OnMessageReceived += AddMessage;
    }

    private void OnSendClicked()
    {
        if (string.IsNullOrWhiteSpace(missatge.value)) return;

        wsClient.SendWebSocketMessage(nickname.value + ";" + missatge.value);
        missatge.value = "";
    }

    private void AddMessage(string text)
    {
        Label label = new Label(text);
        scrollView.Add(label);

        // Baja el scroll hasta el último mensaje
        scrollView.schedule.Execute(() => scrollView.ScrollTo(label));
    }

    private void OnDestroy()
    {
        if (wsClient != null) wsClient.OnMessageReceived -= AddMessage;
    }
}