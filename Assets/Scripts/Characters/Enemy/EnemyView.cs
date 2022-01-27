using System;
using UnityEngine;

/// <summary>
/// Представление врага в игре.
/// </summary>
public class EnemyView : GameCharacterView
{
    private EnemyModel enemyModel
    {
        get => this.model as EnemyModel;
    }

    #region Анимация вращения колес.

    /// <summary>
    /// Анимация поворота колеса.
    /// </summary>
    private Animation rotateAnimationField = null;
    /// <summary>
    /// Анимация поворота колеса.
    /// </summary>
    private Animation rotateAnimation
    {
        get
        {
            if (this.rotateAnimationField == null)
            {
                GameObject whills = this.transform.Find("Whills").gameObject;
                this.rotateAnimationField = whills.GetComponent<Animation>();
            }
            return this.rotateAnimationField;
        }
    }

    #endregion

    #region Анимации головы.

    /// <summary>
    /// Аниматор, который управляет анимациями головы.
    /// </summary>
    private Animator headAnimatorField = null;
    /// <summary>
    /// Аниматор, который управляет анимациями головы.
    /// </summary>
    private Animator headAnimator
    {
        get
        {
            if (this.headAnimatorField == null)
            {
                this.headAnimatorField = GetComponent<Animator>();
            }
            return this.headAnimatorField;
        }
    }
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
        if (!this.enemyModel.IsNearWithPlayer)
        {
            //Если проигрывание анимации уже кончилось.
            if (!this.rotateAnimation.isPlaying)
            {
                //Запустить колесо вращаться снова.
                this.rotateAnimation.Play();
            }
        }

        //Раз в какое-то время делать пугаеющие вещи головой.
        this.timeElapsedBetweenAnimaton += Time.deltaTime;
        if (TIME_BETWEEN_ANIMATIONS < this.timeElapsedBetweenAnimaton)
        {
            Int32 percent = UnityEngine.Random.Range(0, 101);
            //Вращать головой.
            if (percent < 25)
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
}
