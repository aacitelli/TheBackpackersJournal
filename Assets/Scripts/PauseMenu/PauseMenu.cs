using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUI;
    PauseAction action;

    private void Awake()
    {
        action = new PauseAction();
    }

    private void OnEnable()
    {
        action.Enable();
    }

    private void OnDisable()
    {
        action.Disable();
    }

    private void Start()
    {
        action.Pause.PauseGame.performed += _ => DeterminePause();
    }

    private void DeterminePause()
    {
        if (isPaused)
            Resume();
        else
            Pause();
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void Save()
    {
        SaveByXML();
    }
    public void SaveByXML()
    {
        Save save = createSaveGameObject();
        XmlDocument xmlDoc = new XmlDocument();
        XmlSerializer serializer = new XmlSerializer(typeof(Save));
        FileStream stream = new FileStream(Application.dataPath + "/XMLSaves/" + save.playerName + ".xml", FileMode.Create);
        serializer.Serialize(stream, save);
        stream.Close();
/*        #region CreateXML elements

        XmlElement root = xmlDoc.CreateElement("Save");
        root.SetAttribute("PlayerName", save.playerName);
        root.SetAttribute("GamePercentage", save.GamePercentage.ToString());
        XmlElement playerPosXElement = xmlDoc.CreateElement("PlayerPositionX");
        playerPosXElement.InnerText = save.playerPositionX.ToString();
        root.AppendChild(playerPosXElement);

        XmlElement playerPosYElement = xmlDoc.CreateElement("PlayerPositionY");
        playerPosYElement.InnerText = save.playerPositionY.ToString();
        root.AppendChild(playerPosYElement);

        XmlElement playerPosZElement = xmlDoc.CreateElement("PlayerPositionZ");
        playerPosZElement.InnerText = save.playerPositionZ.ToString();
        root.AppendChild(playerPosZElement);

        #endregion

        xmlDoc.AppendChild(root);
        
        xmlDoc.Save(Application.dataPath + "/XMLSaves/" + save.playerName + ".xml");
        if (File.Exists(Application.dataPath + "/XMLSaves/" + save.playerName + ".xml") )
        {
            Debug.Log("XML FILE SAVED");
        }*/
    }

    private Save createSaveGameObject()
    {
        Save save = new Save();
        save.playerName = "Roger";
        save.playerPositionX = GameObject.FindWithTag("Player").transform.position.x;
        save.playerPositionY = GameObject.FindWithTag("Player").transform.position.y;
        save.playerPositionZ = GameObject.FindWithTag("Player").transform.position.z;
        save.GamePercentage = 0;
        return save;

    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Quit()
    {
        Debug.Log("Quit button pressed");
        Application.Quit();
    }

}
