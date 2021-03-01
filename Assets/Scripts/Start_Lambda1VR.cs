using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;


public class Start_Lambda1VR : MonoBehaviour
{

    public Button hlButton;
    public Texture hlhorizontal;
    public Button bsButton;
    public Texture bshorizontal;
    public Button ofButton;
    public Texture ofhorizontal;

    public Button mod1Button;
    public Button mod2Button;
    public Button mod3Button;
    public Button mod4Button;
    public Button mod5Button;
    public Button mod6Button;
    public Button mod7Button;
    public Button mod8Button;
	public Button mod9Button;

    public Slider SSSlider;
    public Slider MSAA;
    public Slider CPU;
    public Slider GPU;
    public Text Failtext;
    public Text Divider;
    private int counter;

    void Start()
    {
        counter = 0;
        //Check what game is even installed
        if (Directory.Exists("/sdcard/xash/valve"))                   //check for folders
        {
            hlButton.gameObject.SetActive(true);
            hlButton.onClick.AddListener(delegate { TaskOnClick("valve"); });
            counter = 1;
        }

        if (Directory.Exists("/sdcard/xash/bshift"))
        {        
            bsButton.gameObject.SetActive(true);
            bsButton.onClick.AddListener(delegate { TaskOnClick("bshift"); });
            counter = 1;
        }

        if (Directory.Exists("/sdcard/xash/gearbox"))
        {
            ofButton.gameObject.SetActive(true);
            ofButton.onClick.AddListener(delegate { TaskOnClick("gearbox"); });
            counter = 1;
        }

        if (counter == 0)
        {
            Failtext.gameObject.SetActive(true);
        }


        string[] customgames = Directory.GetDirectories("/sdcard/xash");  // check for unsupported games
        int game = 0;
        foreach (string cgame in customgames)
        {
            //Debug.Log(Path.GetFileName(cgame));            
            if (Path.GetFileName(cgame) != "gearbox" & Path.GetFileName(cgame) != "valve" & Path.GetFileName(cgame) != "bshift")
            {
                if (game == 0)
                {
                    //Mods found, adjust hl/bs/of boxart
                    RawImage hlImage;
                    RawImage bsImage;
                    RawImage ofImage;

                    hlButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 36.5f);
                    hlImage = hlButton.GetComponentInChildren<RawImage>();
                    hlImage.texture = hlhorizontal;
                    hlImage.GetComponent<RectTransform>().sizeDelta = new Vector2(460.0f,215.0f);
                    hlImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 237.5f);

                    bsButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-365.0f, 36.5f);
                    bsImage = bsButton.GetComponentInChildren<RawImage>();
                    bsImage.texture = bshorizontal;
                    bsImage.GetComponent<RectTransform>().sizeDelta = new Vector2(460.0f, 215.0f);
                    bsImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 237.5f);

                    ofButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(365.0f, 36.5f);
                    ofImage = ofButton.GetComponentInChildren<RawImage>();
                    ofImage.texture = ofhorizontal;
                    ofImage.GetComponent<RectTransform>().sizeDelta = new Vector2(460.0f, 215.0f);
                    ofImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 237.5f);

                    mod1Button.gameObject.SetActive(true);
                    mod1Button.GetComponentInChildren<Text>().text = Path.GetFileName(cgame);
                    if (File.Exists("/sdcard/xash/" + Path.GetFileName(cgame) + ".jpg"))
                    {
                        mod1Button.GetComponentInChildren<RawImage>().texture = LoadJPG("/sdcard/xash/" + Path.GetFileName(cgame) + ".jpg");
                        mod1Button.GetComponentInChildren<Text>().text = "";
                    }
                    mod1Button.onClick.AddListener(delegate { TaskOnClick(Path.GetFileName(cgame)); });
                }
                if (game == 1)
                {
                    mod2Button.gameObject.SetActive(true);
                    mod2Button.GetComponentInChildren<Text>().text = Path.GetFileName(cgame);
                    if (File.Exists("/sdcard/xash/"+Path.GetFileName(cgame)+".jpg"))
                    {
                        mod2Button.GetComponentInChildren<RawImage>().texture = LoadJPG("/sdcard/xash/" + Path.GetFileName(cgame) + ".jpg");
                        mod2Button.GetComponentInChildren<Text>().text = "";
                    }
                    mod2Button.onClick.AddListener(delegate { TaskOnClick(Path.GetFileName(cgame)); });
                }

                if (game == 2)
                {
                    mod3Button.gameObject.SetActive(true);
                    mod3Button.GetComponentInChildren<Text>().text = Path.GetFileName(cgame);
                    if (File.Exists("/sdcard/xash/" + Path.GetFileName(cgame) + ".jpg"))
                    {
                        mod3Button.GetComponentInChildren<RawImage>().texture = LoadJPG("/sdcard/xash/" + Path.GetFileName(cgame) + ".jpg");
                        mod3Button.GetComponentInChildren<Text>().text = "";
                    }
                    mod3Button.onClick.AddListener(delegate { TaskOnClick(Path.GetFileName(cgame)); });                    
                }

                if (game == 3)
                {
                    mod4Button.gameObject.SetActive(true);
                    mod4Button.GetComponentInChildren<Text>().text = Path.GetFileName(cgame);
                    if (File.Exists("/sdcard/xash/" + Path.GetFileName(cgame) + ".jpg"))
                    {
                        mod4Button.GetComponentInChildren<RawImage>().texture = LoadJPG("/sdcard/xash/" + Path.GetFileName(cgame) + ".jpg");
                        mod4Button.GetComponentInChildren<Text>().text = "";
                    }
                    mod4Button.onClick.AddListener(delegate { TaskOnClick(Path.GetFileName(cgame)); });                    
                }

                if (game == 4)
                {
                    mod5Button.gameObject.SetActive(true);
                    mod5Button.GetComponentInChildren<Text>().text = Path.GetFileName(cgame);
                    if (File.Exists("/sdcard/xash/" + Path.GetFileName(cgame) + ".jpg"))
                    {
                        mod5Button.GetComponentInChildren<RawImage>().texture = LoadJPG("/sdcard/xash/" + Path.GetFileName(cgame) + ".jpg");
                        mod5Button.GetComponentInChildren<Text>().text = "";
                    }
                    mod5Button.onClick.AddListener(delegate { TaskOnClick(Path.GetFileName(cgame)); });                    
                }

                if (game == 5)
                {
                    mod6Button.gameObject.SetActive(true);
                    mod6Button.GetComponentInChildren<Text>().text = Path.GetFileName(cgame);
                    if (File.Exists("/sdcard/xash/" + Path.GetFileName(cgame) + ".jpg"))
                    {
                        mod6Button.GetComponentInChildren<RawImage>().texture = LoadJPG("/sdcard/xash/" + Path.GetFileName(cgame) + ".jpg");
                        mod6Button.GetComponentInChildren<Text>().text = "";
                    }
                    mod6Button.onClick.AddListener(delegate { TaskOnClick(Path.GetFileName(cgame)); });
                }

                if (game == 6)
                {
                    mod7Button.gameObject.SetActive(true);
                    mod7Button.GetComponentInChildren<Text>().text = Path.GetFileName(cgame);
                    if (File.Exists("/sdcard/xash/" + Path.GetFileName(cgame) + ".jpg"))
                    {
                        mod7Button.GetComponentInChildren<RawImage>().texture = LoadJPG("/sdcard/xash/" + Path.GetFileName(cgame) + ".jpg");
                        mod7Button.GetComponentInChildren<Text>().text = "";
                    }
                    mod7Button.onClick.AddListener(delegate { TaskOnClick(Path.GetFileName(cgame)); });
                }

                if (game == 7)
                {
                    mod8Button.gameObject.SetActive(true);
                    mod8Button.GetComponentInChildren<Text>().text = Path.GetFileName(cgame);
                    if (File.Exists("/sdcard/xash/" + Path.GetFileName(cgame) + ".jpg"))
                    {
                        mod8Button.GetComponentInChildren<RawImage>().texture = LoadJPG("/sdcard/xash/" + Path.GetFileName(cgame) + ".jpg");
                        mod8Button.GetComponentInChildren<Text>().text = "";
                    }
                    mod8Button.onClick.AddListener(delegate { TaskOnClick(Path.GetFileName(cgame)); });
                }        

                if (game == 8)
                {
                    mod9Button.gameObject.SetActive(true);
                    mod9Button.GetComponentInChildren<Text>().text = Path.GetFileName(cgame);
                    if (File.Exists("/sdcard/xash/" + Path.GetFileName(cgame) + ".jpg"))
                    {
                        mod9Button.GetComponentInChildren<RawImage>().texture = LoadJPG("/sdcard/xash/" + Path.GetFileName(cgame) + ".jpg");
                        mod9Button.GetComponentInChildren<Text>().text = "";
                    }
                    mod9Button.onClick.AddListener(delegate { TaskOnClick(Path.GetFileName(cgame)); });
                }

                game++;  //next game
            }

        }

}
    
       
    public void TaskOnClick(string gamemode)
    {
        //Debug.Log("xash3d --supersampling " + SSSlider.value + " --msaa " + MSAA.value + " --cpu " + CPU.value + " --GPU " + GPU.value + " -game HL_Gold_HD");

        //Debug.Log("Game loaded " + gamemode);

        StreamWriter writer = new StreamWriter("/sdcard/xash/commandline.txt", false);
        writer.WriteLine("xash3d --supersampling " + SSSlider.value + " --msaa " + MSAA.value + " --cpu " + CPU.value + " --gpu " + GPU.value + " -game "  + gamemode);
        writer.Close();


        bool fail = false;
        string bundleId = "com.drbeef.lambda1vr"; // starting lambda1vr
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");

        AndroidJavaObject launchIntent = null;
        try
        {
            launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundleId);
        }
        catch (System.Exception)
        {
            fail = true;
        }

        if (fail)
        { //open sidequest in browser
            Application.OpenURL("https://sidequest.com");
        }
        else //open lambda1vr
            ca.Call("startActivity", launchIntent);

        up.Dispose();
        ca.Dispose();
        packageManager.Dispose();
        launchIntent.Dispose();

    }

    public static Texture2D LoadJPG(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }
}
