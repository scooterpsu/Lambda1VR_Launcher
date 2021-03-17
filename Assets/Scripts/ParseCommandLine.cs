using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class ParseCommandLine : MonoBehaviour
{
    public TMP_InputField customLine;
    public Text statusLine;

    public Slider SSSlider;
    public Slider MSAA;
    public Slider CPU;
    public Slider GPU;

    private string GameName;

    void Start()
    {
        ParseCommandlineTXT();
    }

    public void ParseCommandlineTXT()
    {
        StreamReader cmdFileRead = new StreamReader("/sdcard/xash/commandline.txt");
        string commandLineText = cmdFileRead.ReadLine();
        statusLine.text = "commandline.txt loaded";   
        string[] commandWords = commandLineText.Split(' ');
        int i = 0;
        foreach (string word in commandWords)
        {
            if (word == "--supersampling")
            {
                SSSlider.value = float.Parse(commandWords[i + 1]);
            }
            else if (word == "--msaa")
            {
                MSAA.value = int.Parse(commandWords[i + 1]);
            }
            
            if (word == "--cpu")
            {
                CPU.value = int.Parse(commandWords[i + 1]);
            }
            
            if (word == "--gpu")
            {
                GPU.value = int.Parse(commandWords[i + 1]);
            }
            
            if (word == "-game")
            {
                GameName = commandWords[i + 1];
                UpdateCommandlineTXT();
            }

            i++;
        }
    }

    public void UpdateCommandlineTXT()
    {
        string commandLine = "xash3d --supersampling " + SSSlider.value + " --msaa " + MSAA.value + " --cpu " + CPU.value + " --gpu " + GPU.value + " -game " + GameName;
        customLine.text = commandLine;
    }

    public void SaveCommandlineTXT()
    {
        StreamWriter cmdFileWrite = new StreamWriter("/sdcard/xash/commandline.txt");
        cmdFileWrite.WriteLine(customLine.text);
        cmdFileWrite.Close();
        statusLine.text = "commandline.txt saved";
    }
}
