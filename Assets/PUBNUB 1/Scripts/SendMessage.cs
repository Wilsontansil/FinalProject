using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubNubAPI;
using UnityEngine.UI;
using TMPro;

public class JSONInformation
{
    public string username;
    public string text;
}

public class SendMessage : MonoBehaviour {
    public static PubNub pubnub;
    public TMP_FontAsset customFont;
    public Button SubmitButton;
    public GameObject canvasObject;
    //public InputField UsernameInput;
    public TMP_InputField TextInput;
    public int indexcounter = 0;
    public Text deleteText;
    public Text moveTextUpwards;
    private Text text;
    [SerializeField] GameObject txtMessage;

    //float paddingX = 0;
    //float paddingY = 0;
    //float padding = 0;
    //float height = 10;
    ushort maxMessagesToDisplay = 50;

    string channel = "MusaHub";
    UserInformationScript userinfo;
    // Create a chat message queue so we can interate through all the messages
    Queue<GameObject> chatMessageQueue = new Queue<GameObject>();
    private void Awake()
    {
        userinfo = FindObjectOfType<UserInformationScript>();
    }
    private void Start()
    {
        ExecutePubNub();

    }
    //private void OnEnable()
    //{
    //    ExecutePubNub();
    //}
    //private void OnDisable()
    //{
    //    foreach (Transform item in canvasObject.transform)
    //    {
    //        Destroy(item.gameObject);
    //    }
    //}
    void ExecutePubNub()
    {
        // Use this for initialization

        PNConfiguration pnConfiguration = new PNConfiguration();
        pnConfiguration.PublishKey = "pub-c-d0e3af64-196f-4be6-a34e-e8a3aa606f7a";
        pnConfiguration.SubscribeKey = "sub-c-0e24d83e-c464-11eb-9e40-ea6857a81ff7";
        pnConfiguration.LogVerbosity = PNLogVerbosity.BODY;
        pnConfiguration.UUID = System.Guid.NewGuid().ToString();
        pubnub = new PubNub(pnConfiguration);
        // Add Listener to Submit button to send messages
        Button btn = SubmitButton.GetComponent<Button>();

        btn.onClick.AddListener(TaskOnClick);

        // Fetch the maxMessagesToDisplay messages sent on the given PubNub channel
        pubnub.FetchMessages()
            .Channels(new List<string> { channel })
            .Count(maxMessagesToDisplay)
            .Async((result, status) =>
            {
                if (status.Error)
                {
                    Debug.Log(string.Format(
                        " FetchMessages Error: {0} {1} {2}",
                        status.StatusCode, status.ErrorData, status.Category
                    ));
                    ExecutePubNub();
                }
                else
                {
                    foreach (KeyValuePair<string, List<PNMessageResult>> kvp in result.Channels)
                    {
                        foreach (PNMessageResult pnMessageResult in kvp.Value)
                        {
                            // Format data into readable format
                            JSONInformation chatmessage = JsonUtility.FromJson<JSONInformation>(pnMessageResult.Payload.ToString());

                            // Call the function to display the message in plain text
                            CreateChat(chatmessage);
                        }
                    }
                }
            });

        // This is the subscribe callback function where data is recieved that is sent on the channel
        pubnub.SubscribeCallback += (sender, e) =>
        {
            SubscribeEventEventArgs message = e as SubscribeEventEventArgs;
            if (message.MessageResult != null)
            {
                // Format data into a readable format
                JSONInformation chatmessage = JsonUtility.FromJson<JSONInformation>(message.MessageResult.Payload.ToString());

                // Call the function to display the message in plain text
                CreateChat(chatmessage);

                // When a new chat is created, remove the first chat and transform all the messages on the page up
                SyncChat();
            }
        };

        // Subscribe to a PubNub channel to receive messages when they are sent on that channel
        pubnub.Subscribe()
            .Channels(new List<string>() {
                channel
            })
            .WithPresence()
            .Execute();
    }
    // Function used to create new chat objects based of the data received from PubNub
    void CreateChat(JSONInformation payLoad){

        // Create a string with the username and text
        string currentObject = string.Concat(payLoad.username, payLoad.text);

        // Create a new gameobject that will display text of the data sent via PubNub
        GameObject chatMessage = new GameObject(currentObject);
        //GameObject chatMessage = Instantiate(txtMessage);
        chatMessage.transform.SetParent(canvasObject.transform);
        //chatMessage.GetComponent<TextMeshProUGUI>().text = string.Concat(payLoad.username, payLoad.text);
        chatMessage.transform.localScale = Vector3.one;
        chatMessage.transform.localPosition = Vector3.zero;
        //chatMessage.transform.position = new Vector3(canvasObject.transform.position.x - paddingX, canvasObject.transform.position.y - paddingY + padding - (indexcounter * height), 0f);
        chatMessage.AddComponent<TextMeshProUGUI>().text = currentObject;

        // Assign text to the gameobject. Add visual properties to text
        var chatText = chatMessage.GetComponent<TextMeshProUGUI>();
        chatText.font = customFont;
        if (payLoad.username == userinfo._UserName)
        {
            chatText.color = new Color32(153, 51, 255, 255);
        }
        else
        {
            chatText.color = UnityEngine.Color.black;
        }

        chatText.fontSize = 12;

        // Assign a RectTransform to gameobject to maniuplate positioning of chat.
        RectTransform rectTransform;
        rectTransform = chatText.GetComponent<RectTransform>();
        //rectTransform.sizeDelta = new Vector2(435, height);

        // Assign the gameobject to the queue of chatmessages
        chatMessageQueue.Enqueue(chatMessage);

        // Keep track of how many objects we have displayed on the screen
        indexcounter++;
    }

    void SyncChat() {
        // If more maxMessagesToDisplay objects are on the screen, we need to start removing them
        if (indexcounter > maxMessagesToDisplay)
        {
            // Delete the first gameobject in the queue
            GameObject deleteChat = chatMessageQueue.Dequeue();
            Destroy(deleteChat);

            // Move all existing text gameobjects up the Y axis
            int c = 0;
            foreach (GameObject moveChat in chatMessageQueue)
            {
                RectTransform moveText = moveChat.GetComponent<RectTransform>();
                //moveText.position = new Vector3(canvasObject.transform.position.x - paddingX, canvasObject.transform.position.y - paddingY + padding - (c * height), 0f);
                c++;
            }
        }
    }


    void TaskOnClick()
    {
        // When the user clicks the Submit button,
        // create a JSON object from input field input
        JSONInformation publishMessage = new JSONInformation();
        publishMessage.username = userinfo._UserName;
        publishMessage.text = " : " + TextInput.text;
        string publishMessageToJSON = JsonUtility.ToJson(publishMessage);

        // Publish the JSON object to the assigned PubNub Channel
        pubnub.Publish()
            .Channel(channel)
            .Message(publishMessageToJSON)
            .Async((result, status) =>
            {
                if (status.Error)
                {
                    Debug.Log(status.Error);
                    Debug.Log(status.ErrorData.Info);
                }
                else
                {
                    Debug.Log(string.Format("Publish Timetoken: {0}", result.Timetoken));
                }
            });

        TextInput.text = "";
    }
}
