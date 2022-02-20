using System;
using UnityEngine;

/// <summary>
/// Представление врага в игре.
/// </summary>
public class SimpleEnemyView : EnemyView
{
    private SimpleEnemyModel simpleEnemyModel
    {
        get => this.model as SimpleEnemyModel;
    }

    #region Звуки, которые издают враги.

    /// <summary>
    /// Источник звуков у врага.
    /// </summary>
    [SerializeField]
    private AudioSource enemiesAudioSourcePrivate = null;
    /// <summary>
    /// Источник звуков у врага.
    /// </summary>
    public AudioSource enemiesAudioSource
    {
        get
        {
            if(this.enemiesAudioSourcePrivate.clip==null)
            {
                this.enemiesAudioSourcePrivate.clip = this.EnemyKick;
            }
            return this.enemiesAudioSourcePrivate;
        }
    }
    /// <summary>
    /// Звук, с которым враг бьет игрока.
    /// </summary>
    public AudioClip EnemyKick = null;
    public AudioClip EnemyDodgeSound = null;
    public AudioClip EnemyGiggleSound = null;
    /// <summary>
    /// Звук, с которым прыгает голова.
    /// </summary>
    public AudioClip EnemyHeadJumpSound = null;
    /// <summary>
    /// Проиграть звук, с которым прыгает голова.
    /// </summary>
    private void PlayEnemyHeadJumpSound()
    {
        this.enemiesAudioSource.clip = this.EnemyHeadJumpSound;
        this.enemiesAudioSource.Play();
    }
    /// <summary>
    /// Звук, с которым вращается голова.
    /// </summary>
    public AudioClip EnemyHeadRotateSound = null;
    /// <summary>
    /// Проиграть звук, с которым вращается голова.
    /// </summary>
    private void PlayEnemyHeadRotateSound()
    {
        this.enemiesAudioSource.clip = this.EnemyHeadRotateSound;
        this.enemiesAudioSource.Play();
    }

    #endregion Звуки, которые издают враги.

    #region Анимация вращения колес.

    /// <summary>
    /// Анимация поворота колеса.
    /// </summary>
    public Animation rotateWhillsAnimation = null;

    #endregion

    #region Анимации головы.

    /// <summary>
    /// Аниматор, который управляет анимациями головы.
    /// </summary>
    public Animator headAnimator = null;
    /// <summary>
    /// Время, которое должно пройти между выполнением анимаций.
    /// </summary>
    private const Single TIME_BETWEEN_ANIMATIONS = 5f;
    /// <summary>
    /// Сколько прошло времени после выполнения последней анимации.
    /// </summary>
    private Single timeElapsedBetweenAnimaton = 0;
    /// <summary>
    /// Проиграть выбранную анимацию.
    /// </summary>
    /// <param name="name">Имя анимации.</param>
    private void PlayAnimation(String name)
    {
        this.headAnimator.enabled = true;

        this.headAnimator.Play(name);
    }
    /// <summary>
    /// Остановить проигрывание анимации.
    /// </summary>
    private void StopAnimation()
    {
        this.headAnimator.enabled = false;
    }

    #endregion

    private void Update()
    {
        //Если враг далеко от игрока.
        if (!this.simpleEnemyModel.IsNearWithPlayer)
        {
            //Если проигрывание анимации уже кончилось.
            if (!this.rotateWhillsAnimation.isPlaying)
            {
                //Запустить колесо вращаться снова.
                this.rotateWhillsAnimation.Play();
            }
        }

        //Раз в какое-то время делать пугаеющие вещи головой.
        this.timeElapsedBetweenAnimaton += Time.deltaTime;
        if (TIME_BETWEEN_ANIMATIONS < this.timeElapsedBetweenAnimaton)
        {
            Int32 percent = UnityEngine.Random.Range(0, 101);
            //Вращать головой.
            if (percent < 15)
            {
                PlayAnimation("HeadRotate");
            }
            //Подкидывать голову.
            else if (percent > 24 && percent < 40)
            {
                PlayAnimation("HeadFlight");
            }
            this.timeElapsedBetweenAnimaton = 0;
        }
    }
}
    
