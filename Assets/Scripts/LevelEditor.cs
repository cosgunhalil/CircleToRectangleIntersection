using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class LevelEditor : EditorWindow
{

    public static LevelEditor window;
    public string NewLevelName = "Level";
    public Vector2 scrollPosition = Vector2.zero;
    private Rectangle[] _rects;
    private ObstacleCreator[] _obstacleCreators;
    private List<string> _savedLevelNames = new List<string>();
    private Level _levelData;

    [MenuItem("Tools/LevelEditor")]
    public static void CreateWindow()
    {
        window = (LevelEditor)EditorWindow.GetWindow(typeof(LevelEditor)); //create a window
        window.title = "Level Editor"; //set a window title
    }

    void OnGUI()
    {
        if (window == null)
        {
            CreateWindow();
            _savedLevelNames = new List<string>();
        }


        NewLevelName = GUI.TextField(new Rect(10, 10, position.width, 20), NewLevelName, 25);

        if (GUI.Button(new Rect(10, 40, position.width, 20), "Save"))
        {
            Save(NewLevelName);
        }

        if (GUI.Button(new Rect(10, 70, position.width, 20), "LoadLevels"))
        {
            CreateLevelButtons();
        }
        GUILayout.BeginArea(new Rect(10, 100, position.width, position.height));
        for (int i = 0; i < _savedLevelNames.Count; i++)
        {
            if (GUILayout.Button(_savedLevelNames[i]))
            {
                LoadDataFromJson(_savedLevelNames[i]);
            }
        }
        GUILayout.EndArea();

    }

    public void CreateLevelButtons()
    {
        _savedLevelNames = new List<string>();

        _savedLevelNames = GetLevelNames();

        if (GUI.Button(new Rect(10, 90, position.width, 20), "Level1"))
        {
            Debug.Log("Test is completed");
        }
    }

    private void Save(string levelName)
    {
        _rects = FindObjectsOfType<Rectangle>();
        _obstacleCreators = FindObjectsOfType<ObstacleCreator>();

        string path = Application.dataPath + "/Resources/" + levelName + ".txt";
        var data = SerializeMapData();

        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(fs))
            {

                writer.Write(data);
            }

        }

    }

    private string SerializeMapData()
    {
        Level level = new Level();
        foreach (var item in _rects)
        {
            LevelItem r = new LevelItem();
            r.Height = item.Height;
            r.Width = item.Width;
            r.Position = item.GetRectPosition();
            level.LevelItems.Add(r);
        }

        foreach (var item in _obstacleCreators)
        {
            Generator g = new Generator();
            g.ObjectCount = item.ObjectCount;
            g.ObstacleHeight = item.ObstacleHeight;
            g.ObstacleWidth = item.ObstacleWidth;
            g.Position = item.GetPosition();
            level.ObjectGenerators.Add(g);
        }

        var data = JsonUtility.ToJson(level);

        return data;
    }



    public void LoadDataFromJson(string fileName)
    {
        string path = Application.dataPath + "/Resources/" + fileName;
        var data = ReadDataFromText(path);
        _levelData = JsonUtility.FromJson<Level>(data);
        LoadScene();
    }

    private void LoadScene()
    {
        ClearScene();
        foreach (var rect in _levelData.LevelItems)
        {
            var r = GenerateRect();
            var rModel = r.GetComponent<Rectangle>();
            rModel.CreateRect(rect.Position, rect.Height, rect.Width);
        }

        foreach (var obstacleCreator in _levelData.ObjectGenerators)
        {
            var generator = GenerateObstacleCreator();
            var model = generator.GetComponent<ObstacleCreator>();
            model.ObjectCount = obstacleCreator.ObjectCount;
            model.ObstacleHeight = obstacleCreator.ObstacleHeight;
            model.ObstacleWidth = obstacleCreator.ObstacleWidth;
            model.transform.position = obstacleCreator.Position;
        }
    }

    private GameObject GenerateRect()
    {
        GameObject rect = Instantiate(Resources.Load("Rectangle", typeof(GameObject))) as GameObject;
        return rect;
    }

    private GameObject GenerateObstacleCreator()
    {
        GameObject obstacleCreator = Instantiate(Resources.Load("ObstacleCreator", typeof(GameObject))) as GameObject;
        return obstacleCreator;
    }

    public string ReadDataFromText(string path)
    {
        string data = null;
        try
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(fs))
                {
                    data = reader.ReadToEnd();
                }
            }

        }
        catch (System.Exception ex)
        {
            Debug.Log(ex);
        }

        return data;
    }

    private void ClearScene()
    {
        var rectangles = GameObject.FindObjectsOfType<Rectangle>();
        var obstacleCreator = FindObjectsOfType<ObstacleCreator>();

        foreach (var rect in rectangles)
        {
            DestroyImmediate(rect.gameObject);
        }

        foreach (var obs in obstacleCreator)
        {
            DestroyImmediate(obs.gameObject);
        }
    }

    private List<string> GetLevelNames()
    {
        List<string> levelNames = new List<string>();

        string partialName = "Level";

        DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(Application.dataPath + "/Resources");
        FileSystemInfo[] filesAndDirs = hdDirectoryInWhichToSearch.GetFileSystemInfos("*" + partialName + "*.txt");

        foreach (FileSystemInfo foundFile in filesAndDirs)
        {
            string fullName = foundFile.Name;
            levelNames.Add(fullName);
        }

        return levelNames;
    }

}

public enum EObjectType
{
    movingSquare
}

[Serializable]
public class Level
{
    public List<LevelItem> LevelItems;
    public List<Generator> ObjectGenerators;

    public Level()
    {
        LevelItems = new List<LevelItem>();
        ObjectGenerators = new List<Generator>();
    } 
}

[Serializable]
public class LevelItem
{
    public float Height;
    public float Width;
    public Vector2 Position;
}

[Serializable]
public class Generator
{
    public int ObjectCount;
    public Vector2 Position;
    public float ObstacleHeight;
    public float ObstacleWidth;
}