using System;
using System.Collections; using System.Collections.Generic; using System.Text;
using UnityEngine; using UnityEngine.UI;

/// <summary> ///  Класс информации об игроке. /// </summary> public class PlayerInfo : CharacterInfo {     private StringBuilder stringBuilderForText = new StringBuilder(30);     public Text textHealthAndCountCubes = null;     public Text textRedCubesCount = null;     public Text textGreenCubesCount = null;     public Text textYellowCubesCount = null;      #region Цвета кубиков и снарядов      private Int32 redCubesCount=0;     private Int32 greenCubesCount=0;     private Int32 yellowCubesCount=0;     public Color bulletColor = Color.white;      public void AddColoredCube(Color cubeColor)
    {
        this.bulletColor = cubeColor;

        //string 
        if(cubeColor==Color.red)
        {
            this.redCubesCount++;
        }
    }      #endregion      // Start is called before the first frame update     void Start()     {         this.healthPoints = 100;         this.textHealthAndCountCubes.resizeTextMaxSize = 100;     }           // Update is called once per frame     void Update()     {         StringBuilder text = this.stringBuilderForText;         text.Clear();         text.Append("Очки жизни: ");         text.Append(this.healthPoints.ToString());
        this.textHealthAndCountCubes.text = text.ToString();           text.Clear();         text.Append("Красных кубов: ");         text.Append(this.redCubesCount.ToString());
        this.textRedCubesCount.text = text.ToString();          text.Clear();         text.Append("Зеленых кубов: ");         text.Append(this.greenCubesCount.ToString());
        this.textGreenCubesCount.text = text.ToString();          text.Clear();         text.Append("Желтых кубов: ");         text.Append(this.yellowCubesCount.ToString());
        this.textYellowCubesCount.text = text.ToString();     } } 