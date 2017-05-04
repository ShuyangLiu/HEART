

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;
using System;
using System.IO;
using System.Linq;

using UnityEngine.Windows.Speech;

#if WINDOWS_UWP
using Windows.Networking;
using Windows.Networking.Sockets;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Networking.Connectivity;
#endif




public class AnimatedArms : MonoBehaviour, IInputClickHandler
{
    private int moveCounter;
    private Text text;
    public readonly int COUNT = 5;
    private Animator anim;
    private int flag = 0;
    public GameObject prefab;
    private bool firstRun;
    private bool buffer = true;
    private bool newMessage;
    private String CurrentMessage;
    private int phase;
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    // Use this for initialization
    void Start()
    {
        moveCounter = COUNT;
        phase = 0;
        text = GameObject.Find("Text").GetComponent<Text>();
        text.text = "Welcome to HololensSR.\n\nPlease say \"Start\" to begin.";
        keywords.Add("Start", () =>
        {
            if(phase == 0)
            {
                phase++;
            }
        });
        keywords.Add("Next", () =>
        {
            text.text = "NEXT";
            moveCounter = COUNT;
            phase++;
        });
        keywords.Add("Quit", () =>
        {
            text.text = "Quitting";
            Application.Quit();
        });
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
        anim = gameObject.GetComponentInChildren<Animator>();
        firstRun = true;
        newMessage = false;
        CurrentMessage = "";
        //InputManager.Instance.PushModalInputHandler(this.gameObject);
    }

#if !WINDOWS_UWP
    void Update()
    {
#endif
#if WINDOWS_UWP
    async void Update()
    {
#endif
        if(phase == 1)
        {
            text.text = "Good! Let's start by moving your wrist " + moveCounter.ToString() + " time";
            if(moveCounter != COUNT)
            {
                text.text = moveCounter.ToString() + " more time";
            }
            if(moveCounter != 1)
            {
                text.text += "s";
            }
            text.text += "\n\nSay \"Next\" if you want to go to the next exercise";
            //text.text += ("\n" + CurrentMessage);
        }
        else if (phase == 2)
        {
            text.text = "Great! Now try lifting your arm " + moveCounter.ToString() + " time";
            if (moveCounter != COUNT)
            { 
                text.text = moveCounter.ToString() + " more time";
            }
            if (moveCounter != 1)
            {
                text.text += "s";
            }
            text.text += "\n\nSay \"Next\" if you want to go to the next exercise";
            //text.text += ("\n" + CurrentMessage);
        }
        else if (phase == 3)
        {
            text.text = "Nice job! Let's try pulling your arm back " + moveCounter.ToString() + " time";
            if (moveCounter != COUNT)
            {
                text.text = moveCounter.ToString() + " more time";
            }
            if (moveCounter != 1)
            {
                text.text += "s";
            }
            text.text += "\n\nSay \"Next\" if you want to go to the next exercise";
            //text.text += ("\n" + CurrentMessage);
        }
        else if (phase == 4)
        {
            text.text = "Keep it up! Lastly, let's try to rotate your arm " + moveCounter.ToString() + " time";
            if (moveCounter != COUNT)
            {
                text.text = moveCounter.ToString() + " more time";
            }
            if (moveCounter != 1)
            {
                text.text += "s";
            }
            text.text += "\n\nSay \"Quit\" if you want to quit";
            //text.text += ("\n" + CurrentMessage);
        }
        else if (phase == 5)
        {
            text.text = "Great work today! Be sure to come back to continue your therapy!";
            text.text += "\n\nSay \"Quit\" when you are ready to quit";
            //text.text += ("\n" + CurrentMessage);
        }
        if (newMessage)
        {
            buffer = true;
            newMessage = false;
            if (CurrentMessage.Equals("3\n") || CurrentMessage.Equals("3"))
            {
                anim.SetInteger("I2MoveForward", 1);
                if (phase == 3)
                {
                    moveCounter--;
                    if (moveCounter == 0)
                    {
                        moveCounter = COUNT;
                        phase++;
                    }
                }
            }
            else if (CurrentMessage.Equals("1\n") || CurrentMessage.Equals("1"))
            {
                anim.SetInteger("I2WaveHands", 1);
                if(phase == 1)
                {
                    moveCounter--;
                    if(moveCounter == 0)
                    {
                        moveCounter = COUNT;
                        phase++;
                    }
                }
            }
            else if (CurrentMessage.Equals("2\n") || CurrentMessage.Equals("2"))
            {
                anim.SetInteger("I2RaiseHands", 1);
                if (phase == 2)
                {
                    moveCounter--;
                    if (moveCounter == 0)
                    {
                        moveCounter = COUNT;
                        phase++;
                    }
                }
            }
            else if (CurrentMessage.Equals("4\n") || CurrentMessage.Equals("4"))
            {
                anim.SetInteger("I2RotateHands", 1);
                if (phase == 4)
                {
                    moveCounter--;
                    if (moveCounter == 0)
                    {
                        moveCounter = COUNT;
                        phase++;
                    }
                }
            }
            else if(phase == 5)
            {
                text.text = "Congrats, you have finished the session!";
                text.text += "\n\nSay \"Quit\" to close the application";
            }     
            
        }
        else if(buffer == true)
        {
            buffer = false;
        }
        else
        {
            anim.SetInteger("I2MoveForward", 0);
            anim.SetInteger("I2WaveHands", 0);
            anim.SetInteger("I2RaiseHands", 0);
            anim.SetInteger("I2RotateHands", 0);
        }

#if WINDOWS_UWP
        if (firstRun)
        {
            firstRun = false;
            try
            {

                //text.text = "Starting the Server!";
                Windows.Networking.Sockets.StreamSocketListener socketListener = new Windows.Networking.Sockets.StreamSocketListener();

                socketListener.ConnectionReceived += SocketListener_ConnectionReceived;

                HostName myHost = null;
                foreach (HostName localHostName in NetworkInformation.GetHostNames())
                {
                    if (localHostName.IPInformation != null)
                    {
                        if (localHostName.Type == HostNameType.Ipv4)
                        {
                            myHost = localHostName;
                            break;
                        }
                    }
                }
                CurrentMessage = myHost.ToString();
                Debug.Log(CurrentMessage);
                await socketListener.BindServiceNameAsync("8001");
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
#endif
 }   

    public void OnTouch()
{
    anim.SetInteger("I2MoveForward", 1);
}



public void OnRestart()
{
    anim.SetInteger("I2MoveForward", 0);
}



public void OnInputClicked(InputClickedEventData eventData)
{
    if (this.flag == 0)
    {
        anim.SetInteger("I2MoveForward", 1);
        this.flag = 1;
    }
    else
    {
        anim.SetInteger("I2MoveForward", 0);
    }
}

#if WINDOWS_UWP
    async void SocketListener_ConnectionReceived(Windows.Networking.Sockets.StreamSocketListener sender,
    Windows.Networking.Sockets.StreamSocketListenerConnectionReceivedEventArgs args)
    {
        CurrentMessage = "Connected!";
        //Stream streamIn = args.Socket.GetDataStream().AsStreamForRead();
        StreamReader reader = new StreamReader(args.Socket.InputStream.AsStreamForRead());
        while (true)
        {
            CurrentMessage = await reader.ReadLineAsync();
            newMessage = true;
            //text.text = CurrentMessage;
        }
    }
#endif

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}