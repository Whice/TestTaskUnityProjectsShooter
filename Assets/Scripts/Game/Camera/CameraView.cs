using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraView : ItemView
{
    #region Main camera.

    /// <summary>
    /// Ссылка на камеру игрока.
    /// </summary>
    public Camera mainCamera = null;

    /// <summary>
    /// Проверить находится ли точка в зоне видимости камеры камеры.
    /// </summary>
    /// <param name="position">Местоположение точки.</param>
    /// <returns></returns>
    public Boolean InViewportCamera(Vector3 position)
    {
        Vector3 viewPosition = this.mainCamera.WorldToViewportPoint(position);
        if (viewPosition.x > 0.015f && viewPosition.x < 1.05f && viewPosition.z > 0)
        {
            return true;
        }
        return false;
    }

    #endregion Main camera.


    private void Awake()
    {
        this.centerOfCanvasBossHPBarX = this.canvasBossHPBarRectTransform.rect.width / 2 - 
            this.bossHPBar.GetComponent<RectTransform>().rect.width/2;
    }
    #region полоска HP босса.

    [Header("Boss HP bar")]
    /// <summary>
    /// Картинка, которая должна динамически
    /// уменьшаться при нанесении урона боссу.
    /// </summary>
    public Image dynamicFrontHP = null;
    /// <summary>
    /// Вся полоска ХП босса.
    /// </summary>
    public GameObject bossHPBar = null;
    /// <summary>
    /// Данные о холсте ХПшки босса.
    /// </summary>
    [SerializeField]
    private RectTransform canvasBossHPBarRectTransform = null;
    /// <summary>
    /// Изначальное положение ХПшки босса по оси Х.
    /// </summary>
    private Single centerOfCanvasBossHPBarX;

    private GameObject bossField = null;
    private GameObject boss
    {
        get
        {
            if(this.bossField==null)
            {
                this.bossField = ArenaModel.instance.boss;
            }
            return this.bossField;
        }
    }

    private void Update()
    {
        if (this.boss.activeSelf)
        {
            Vector3 viewPosition = this.mainCamera.WorldToViewportPoint(this.boss.transform.position);
            if (InViewportCamera(this.boss.transform.position))
            {
                this.bossHPBar.transform.position = Vector3.Lerp
                    (
                    this.bossHPBar.transform.position,
                    new Vector3(
                                viewPosition.x,
                                this.bossHPBar.transform.position.y,
                                this.bossHPBar.transform.position.z
                                ),
                    Time.deltaTime
                    );
            }
            else
            {
                this.bossHPBar.transform.position = Vector3.Lerp
                    (
                    this.bossHPBar.transform.position,
                    new Vector3(
                                this.centerOfCanvasBossHPBarX +this.bossHPBar.GetComponent<RectTransform>().rect.width/2,
                                this.bossHPBar.transform.position.y,
                                this.bossHPBar.transform.position.z
                                ),
                    Time.deltaTime
                    );
            }

            Debug.Log(
                nameof(viewPosition) + " - " +
                nameof(viewPosition.x) + ": " + viewPosition.x.ToString() + " " +
                nameof(viewPosition.y) + ": " + viewPosition.y.ToString() + " " +
                nameof(viewPosition.z) + ": " + viewPosition.z.ToString() + " " 
                );
        }
    }

    #endregion полоска HP босса.
}