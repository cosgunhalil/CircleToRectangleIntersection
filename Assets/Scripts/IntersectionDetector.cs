using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class IntersectionDetector : MonoBehaviour {

    public static IntersectionDetector Instance;

    public Circle Circle;
    private List<MovingObject> _movingObject;
    private List<Rectangle> _rects;
    private Player _player;
    public bool isInterSect;

    private int _currentLevel;
    private int _lastLevel;
    private List<Level> _levels;
    private Vector2 _circleDistance;

    private List<GameObject> _rectanglePool;

    public bool IsLevelCompleted;
    
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        InitGame();
        ClearScene();
        LoadLevelsFromJson();
        LoadNextLevel(_currentLevel);
        InitPlayer();
        //Rectangle.CreateRect(new Vector2(-4, 4), 1, 1);
        //_movingObject = Rectangle.GetComponent<MovingObject>();
        //_movingObject.InitMovingObject(-4, 4, 2);
        
    }

    public void LoadLevel()
    {
        IsLevelCompleted = true;

        LoadNextLevel(_currentLevel);

        IsLevelCompleted = false;

        _currentLevel++;
        if (_currentLevel > _lastLevel)
        {
            _currentLevel = 0;
        }
    }

    private void LoadNextLevel(int levelIndex)
    {
        ClearScene();

        foreach (var rect in _levels[levelIndex].LevelItems)
        {
            var r = GenerateRect();
            var rModel = r.GetComponent<Rectangle>();
            rModel.CreateRect(rect.Position, rect.Height, rect.Width);
            _rects.Add(rModel);
            _movingObject.Add(r.GetComponent<MovingObject>());
        }
        InitMovingObjects(_movingObject);
        InitPlayer();
    }

    private void InitPlayer()
    {
        Circle.CreateCircle(new Vector2(0, -3), .5f);
        _player = Circle.GetComponent<Player>();
        _player.InitPlayer(4, new Vector2(0, -3), 3);
    }

    private void InitMovingObjects(List<MovingObject> _movingObject)
    {
        foreach (var item in _movingObject)
        {
            item.InitMovingObject(-4, 4, 2);
        }
    }

    private GameObject GenerateRect()
    {
        GameObject rect;
        if (_rectanglePool.Count < 1)
        {
            rect = Instantiate(Resources.Load("Rectangle", typeof(GameObject))) as GameObject;
            _rectanglePool.Add(rect);
        }

        rect = _rectanglePool[_rectanglePool.Count - 1];
        _rectanglePool.Remove(rect);

        rect.SetActive(true);
        return rect;
    }


    private void InitGame()
    {
        _rectanglePool = new List<GameObject>();
        IsLevelCompleted = false;
        _currentLevel = 0;
        _levels = new List<Level>();
        _movingObject = new List<MovingObject>();
        _rects = new List<Rectangle>();
    }

    private void LoadLevelsFromJson()
    {
        var levelNames = GetLevelNames();
    
        foreach (var levelName in levelNames)
        {
            string path = Application.dataPath + "/Resources/" + levelName;
            var data = ReadDataFromText(path);
            var levelData = JsonUtility.FromJson<Level>(data);
            _levels.Add(levelData);
        }

        _lastLevel = _levels.Count - 1;
    }

    private string ReadDataFromText(string path)
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

    private void ClearScene()
    {
        var rectangles = GameObject.FindObjectsOfType<Rectangle>();

        foreach (var rect in rectangles)
        {
            rect.gameObject.SetActive(false);
            _rectanglePool.Add(rect.gameObject);
        }

        _movingObject = new List<MovingObject>();
        _rects = new List<Rectangle>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (IsLevelCompleted == false)
        {
            for (int i = 0; i < _rects.Count; i++)
            {
                isInterSect = CheckIntersection(_rects[i]);
                _movingObject[i].Move();

                if (isInterSect)
                {
                    _rects[i].SetColor(Color.green);
                }
            }

            _player.Move();

            if (Input.GetMouseButtonDown(0))
            {
                _player.IsMoving = true;
            }
        }
        
    }

    private bool CheckIntersection(Rectangle rectangle)
    {
        _circleDistance.x = Math.Abs(Circle.GetCirclePosition().x - rectangle.GetRectPosition().x);
        _circleDistance.y = Math.Abs(Circle.GetCirclePosition().y - rectangle.GetRectPosition().y);

        if (_circleDistance.x > (rectangle.Width / 2 + Circle.Radius))
        {
            return false;
        }
        if (_circleDistance.y > (rectangle.Height / 2 + Circle.Radius))
        {
            return false;
        }

        if (_circleDistance.x <= (rectangle.Width / 2))
        {
            return true;
        }
        if (_circleDistance.y <= (rectangle.Height / 2))
        {
            return true;
        }

        var cornerDistance_sq = Math.Pow((_circleDistance.x - rectangle.Width / 2) , 2) + 
                                Math.Pow(_circleDistance.y - rectangle.Height / 2 , 2);

        return (cornerDistance_sq <= (Math.Pow(Circle.Radius,2)));
    }
}


