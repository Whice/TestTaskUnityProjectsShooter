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
            if (percent < 250)
            {
                this.headAnimator.Play("HeadRotate");
            }
            //Подкидывать голову.
            else if (percent > 24 && percent < 40)
            {
                this.headAnimator.Play("HeadFlight");
            }
            this.timeElapsedBetweenAnimaton = 0;
        }
    }

    public void StopAnimationInAnimator()
    {
        var number = this.headAnimator.GetCurrentAnimatorStateInfo(0);
        var nameAnim = number.IsName("HeadRotate");
    }
}
