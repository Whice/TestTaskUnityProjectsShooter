using System;
using System.Collections.Generic;
using UnityEngine;

public class ArenaView : ItemView
{
    #region Заготовки.

    /// <summary>
    /// Заготовка пули.
    /// </summary>
    public GameObject bulletPrefab = null;
    /// <summary>
    /// Заготовка врага.
    /// </summary>
    public GameObject enemyPrefab = null;
    /// <summary>
    /// Заготовка ящика.
    /// </summary>
    public GameObject gameBoxPrefab = null;

    #endregion

    #region Провадер звуков.

    /// <summary>
    /// Провайдер звуков для всей игры.
    /// </summary>
    [SerializeField]
    public AudioClip[] audioClips = new AudioClip[1];
    /// <summary>
    /// Получить звук по его названию.
    /// </summary>
    /// <param name="clipName">Название звука.</param>
    /// <returns>Первый в списке звук с указанным именем или null, если такого нет.</returns>
    public AudioClip GetAudioClip(String clipName)
    {
        foreach (AudioClip clip in this.audioClips)
        {

            if (clip.name == clipName)
            {
                return clip;
            }
        }
        return null;
    }

    #endregion Провадер звуков.


}