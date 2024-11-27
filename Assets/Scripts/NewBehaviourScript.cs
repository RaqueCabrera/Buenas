using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Agora.Rtc;

#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
using UnityEngine.Android;
#endif

//VR SCRIPT 

public class NewBehaviourScript : MonoBehaviour
{
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
private ArrayList permissionList = new ArrayList() { Permission.Camera, Permission.Microphone };
#endif

    // Buenas without token
    private string _appID = "e995437c8bc047d4b84c2349221d3feb";
    // Fill in your channel name.
    private string _channelName = "test";
    // Fill in the temporary token you obtained from Agora Console.
    private string _token = "";
    // A variable to save the remote user uid.
    private int remoteUid, remoteUid2, remoteUid3, remoteUid4, remoteUid5, remoteUid6, remoteUid7 = 0;
    internal VideoSurface LocalView;
    internal VideoSurface RemoteView;
    internal VideoSurface RemoteView2;
    internal VideoSurface RemoteView3;
    internal VideoSurface RemoteView4;
    internal VideoSurface RemoteView5;
    internal VideoSurface RemoteView6;
    internal VideoSurface RemoteView7;
    internal IRtcEngine RtcEngine;

    // Virtual background
    int counter = 0; // to cycle through the different types of backgrounds
    bool isVirtualBackGroundEnabled = false;


    // Start is called before the first frame update
    void Start()
    {
        SetupVideoSDKEngine();
        InitEventHandler();
        SetupUI();
        Join();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPermissions();

    }

    void Awake()
    {
        // Makes sure the object persists across scenes
        DontDestroyOnLoad(gameObject);
    }

    private void CheckPermissions()
    {
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
    foreach (string permission in permissionList)
    {
        if (!Permission.HasUserAuthorizedPermission(permission))
        {
            Permission.RequestUserPermission(permission);
        }
    }
#endif
    }

    //Reference the UI elements from SampleScene
    private void SetupUI()
    {
        GameObject go = GameObject.Find("LocalView");
        LocalView = go.AddComponent<VideoSurface>();
        go.transform.Rotate(0.0f, 0.0f, 180.0f);

        go = GameObject.Find("RemoteView");
        RemoteView = go.AddComponent<VideoSurface>();
        go.transform.Rotate(0.0f, 0.0f, 180.0f);

        go = GameObject.Find("RemoteView2");
        RemoteView2 = go.AddComponent<VideoSurface>();
        go.transform.Rotate(0.0f, 0.0f, 180.0f);

        go = GameObject.Find("RemoteView3");
        RemoteView3 = go.AddComponent<VideoSurface>();
        go.transform.Rotate(0.0f, 0.0f, 180.0f);

        go = GameObject.Find("RemoteView4");
        RemoteView4 = go.AddComponent<VideoSurface>();
        go.transform.Rotate(0.0f, 0.0f, 180.0f);

        go = GameObject.Find("RemoteView5");
        RemoteView2 = go.AddComponent<VideoSurface>();
        go.transform.Rotate(0.0f, 0.0f, 180.0f);

        go = GameObject.Find("RemoteView6");
        RemoteView2 = go.AddComponent<VideoSurface>();
        go.transform.Rotate(0.0f, 0.0f, 180.0f);

        go = GameObject.Find("RemoteView7");
        RemoteView2 = go.AddComponent<VideoSurface>();
        go.transform.Rotate(0.0f, 0.0f, 180.0f);

        go = GameObject.Find("Leave");
        go.GetComponent<Button>().onClick.AddListener(Leave);
        go = GameObject.Find("Join");
        go.GetComponent<Button>().onClick.AddListener(Join);
        go = GameObject.Find("virtualBackground");
        go.GetComponent<Button>().onClick.AddListener(setVirtualBackground);
    }


    private void SetupVideoSDKEngine()
    {
        // Create an instance of the video SDK.
        RtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();
        // Specify the context configuration to initialize the created instance.
        RtcEngineContext context = new RtcEngineContext(_appID, 0,
        CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_COMMUNICATION,
        AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT, AREA_CODE.AREA_CODE_GLOB, null);
        // Initialize the instance.
        RtcEngine.Initialize(context);
    }


    private void InitEventHandler()
    {
        // Creates a UserEventHandler instance.
        UserEventHandler handler = new UserEventHandler(this);
        RtcEngine.InitEventHandler(handler);
    }

    public void Join()
    {
        // Enable the video module.
        RtcEngine.EnableVideo();
        // Set the user role as broadcaster.
        RtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        // Set the local video view.
        LocalView.SetForUser(0, "", VIDEO_SOURCE_TYPE.VIDEO_SOURCE_CAMERA);
        // Start rendering local video.
        LocalView.SetEnable(true);
        // Join a channel.
        RtcEngine.JoinChannel(_token, _channelName);
    }

    public void Leave()
    {
        // Leaves the channel.
        RtcEngine.LeaveChannel();
        // Disable the video modules.
        RtcEngine.DisableVideo();
        // Stops rendering the remote video.
        RemoteView.SetEnable(false);
        RemoteView2.SetEnable(false);
        RemoteView3.SetEnable(false);
        RemoteView4.SetEnable(false);
        // Stops rendering the local video.
        LocalView.SetEnable(false);
    }

    public void setVirtualBackground()
    {
        counter++;
        if (counter > 1)
        //if (counter > 3)
        {
            counter = 0;
            isVirtualBackGroundEnabled = false;
            Debug.Log("Virtual background turned off");
        }
        else
        {
            isVirtualBackGroundEnabled = true;
        }
        VirtualBackgroundSource virtualBackgroundSource = new VirtualBackgroundSource();

        // Set the type of virtual background
        if (counter == 1)
        { // Set a solid background color
            virtualBackgroundSource.background_source_type = BACKGROUND_SOURCE_TYPE.BACKGROUND_COLOR;
            //virtualBackgroundSource.color = 0x000000;
            virtualBackgroundSource.color = 0x0000FF00;
            Debug.Log("Color background enabled");
        }
        else if (counter == 2)
        { // Set background blur
            virtualBackgroundSource.background_source_type = BACKGROUND_SOURCE_TYPE.BACKGROUND_BLUR;
            virtualBackgroundSource.blur_degree = BACKGROUND_BLUR_DEGREE.BLUR_DEGREE_HIGH;
            Debug.Log("Blur background enabled");
        }
        else if (counter == 3)
        { // Set a background image
            virtualBackgroundSource.background_source_type = BACKGROUND_SOURCE_TYPE.BACKGROUND_IMG;
            //virtualBackgroundSource.background_source_type = BACKGROUND_SOURCE_TYPE.
            virtualBackgroundSource.source = "Assets/transparent1.png";
            Debug.Log("Image background enabled");
        }

        // Set processing properties for background
        SegmentationProperty segmentationProperty = new SegmentationProperty();
        segmentationProperty.modelType = SEG_MODEL_TYPE.SEG_MODEL_AI; // Use SEG_MODEL_GREEN if you have a green background
        segmentationProperty.greenCapacity = 0.5F; // Accuracy for identifying green colors (range 0-1)

        // Enable or disable virtual background
        RtcEngine.EnableVirtualBackground(
            isVirtualBackGroundEnabled,
            virtualBackgroundSource, segmentationProperty);
    }


    internal class UserEventHandler : IRtcEngineEventHandler
    {
        private readonly NewBehaviourScript _videoSample;

        internal UserEventHandler(NewBehaviourScript videoSample)
        {
            _videoSample = videoSample;
        }
        // This callback is triggered when the local user joins the channel.
        public override void OnJoinChannelSuccess(RtcConnection connection, int elapsed)
        {
            Debug.Log("You joined channel: " + connection.channelId);
        }

        public override void OnUserJoined(RtcConnection connection, uint uid, int elapsed)
        {

            if (_videoSample.remoteUid == 0)
            {
                _videoSample.RemoteView.SetForUser(uid, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
                _videoSample.remoteUid = (int)uid;
                return;
            }
            if (_videoSample.remoteUid2 == 0)
            {
                _videoSample.RemoteView2.SetForUser(uid, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
                _videoSample.remoteUid2 = (int)uid;
                return;
            }
            if (_videoSample.remoteUid3 == 0)
            {
                _videoSample.RemoteView3.SetForUser(uid, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
                _videoSample.remoteUid3 = (int)uid;
                return;
            }
            if (_videoSample.remoteUid4 == 0)
            {
                _videoSample.RemoteView4.SetForUser(uid, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
                _videoSample.remoteUid4 = (int)uid;
                return;
            }
            if (_videoSample.remoteUid5 == 0)
            {
                _videoSample.RemoteView5.SetForUser(uid, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
                _videoSample.remoteUid5 = (int)uid;
                return;
            }
            if (_videoSample.remoteUid6 == 0)
            {
                _videoSample.RemoteView6.SetForUser(uid, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
                _videoSample.remoteUid6 = (int)uid;
                return;
            }
            if (_videoSample.remoteUid7 == 0)
            {
                _videoSample.RemoteView7.SetForUser(uid, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
                _videoSample.remoteUid7 = (int)uid;
                return;
            }
        }

        // This callback is triggered when a remote user leaves the channel or drops offline.
        public override void OnUserOffline(RtcConnection connection, uint uid, USER_OFFLINE_REASON_TYPE reason)
        {
            if (_videoSample.remoteUid == uid)
            {
                _videoSample.RemoteView.SetEnable(false);
                _videoSample.remoteUid = 0;
            }
            if (_videoSample.remoteUid2 == uid)
            {
                _videoSample.RemoteView2.SetEnable(false);
                _videoSample.remoteUid2 = 0;
            }
            if (_videoSample.remoteUid3 == uid)
            {
                _videoSample.RemoteView3.SetEnable(false);
                _videoSample.remoteUid3 = 0;
            }
            if (_videoSample.remoteUid4 == uid)
            {
                _videoSample.RemoteView4.SetEnable(false);
                _videoSample.remoteUid4 = 0;
            }
            if (_videoSample.remoteUid5 == uid)
            {
                _videoSample.RemoteView5.SetEnable(false);
                _videoSample.remoteUid5 = 0;
            }
            if (_videoSample.remoteUid6 == uid)
            {
                _videoSample.RemoteView6.SetEnable(false);
                _videoSample.remoteUid6 = 0;
            }
            if (_videoSample.remoteUid7 == uid)
            {
                _videoSample.RemoteView7.SetEnable(false);
                _videoSample.remoteUid7 = 0;
            }
        }
    }

    void OnApplicationQuit()
    {
        if (RtcEngine != null)
        {
            Leave();
            RtcEngine.Dispose();
            RtcEngine = null;
        }
    }

}
