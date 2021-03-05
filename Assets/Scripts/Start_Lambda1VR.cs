using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;


public class Start_Lambda1VR : MonoBehaviour
{
    public Texture hlVertical;
    public Texture hlHorizontal;
    public Texture bsVertical;
    public Texture bsHorizontal;
    public Texture ofHorizontal;
    public Texture ofVertical;

    public GameObject launchTilePrefab;
    public GameObject launchPosterPrefab;
    public Transform GamesCanvas;
    private Texture ModTexture;
    private string ModTitle;

    public Slider SSSlider;
    public Slider MSAA;
    public Slider CPU;
    public Slider GPU;
    public Text Failtext;
    private int counter;

    void Start()
    {
        //Check what game is even installed
        if (Directory.Exists("/sdcard/xash/valve") || Directory.Exists("/sdcard/xash/gearbox") || Directory.Exists("/sdcard/xash/bshift"))
        {
            if (Directory.Exists("/sdcard/xash/bshift"))
            {
                addTile(launchPosterPrefab, bsVertical, "", "bshift");
            }
            if (Directory.Exists("/sdcard/xash/valve"))
            {
                addTile(launchPosterPrefab, hlVertical, "", "valve");
            }
            if (Directory.Exists("/sdcard/xash/gearbox"))
            {
                addTile(launchPosterPrefab, ofVertical, "", "gearbox");
            }
        }
        else
        {
            Failtext.gameObject.SetActive(true);
            return;
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
                    //Clear out posters
                    foreach (Transform child in GamesCanvas)
                    {
                        GameObject.Destroy(child.gameObject);
                    }

                    //Add tiles instead
                    if (Directory.Exists("/sdcard/xash/bshift"))
                    {
                        addTile(launchTilePrefab, bsHorizontal, "", "bshift");
                    }
                    if (Directory.Exists("/sdcard/xash/valve"))
                    {
                        addTile(launchTilePrefab, hlHorizontal, "", "valve");
                    }
                    if (Directory.Exists("/sdcard/xash/gearbox"))
                    {
                        addTile(launchTilePrefab, ofHorizontal, "", "gearbox");
                    }
                }

                //Dynamically add buttons for all mods found
                ModTitle = "";
                if (File.Exists("/sdcard/xash/" + Path.GetFileName(cgame) + ".jpg"))
                {
                    ModTexture = LoadImg("/sdcard/xash/" + Path.GetFileName(cgame) + ".jpg");
                } 
                else if (File.Exists("/sdcard/xash/" + Path.GetFileName(cgame) + ".png"))
                {
                    ModTexture = LoadImg("/sdcard/xash/" + Path.GetFileName(cgame) + ".png");
                } 
                else
                {
                    ModTexture = null;
                    ModTitle = Path.GetFileName(cgame);
                }
                addTile(launchTilePrefab, ModTexture, ModTitle, Path.GetFileName(cgame));

                game++;  //next game
            }
        }
    }

    public void addTile(GameObject Prefab, Texture TileTexture, string TileText, string ClickTarget)
    {
        GameObject launchTile = Instantiate(Prefab);
        launchTile.transform.SetParent(GamesCanvas, false);
        launchTile.GetComponentInChildren<RawImage>().texture = TileTexture;
        launchTile.GetComponentInChildren<Text>().text = TileText;
        launchTile.GetComponentInChildren<Button>().onClick.AddListener(delegate { TaskOnClick(ClickTarget); });
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

    public static Texture2D LoadImg(string filePath)
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
