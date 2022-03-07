using System;
using UnityEngine;

/// <summary>
/// Модель врага, который ускоряется раз в некоторое время.
/// </summary>
public class SpeedBoosterEnemyModel : SimpleEnemyModel
{
    /// <summary>
    /// Режим скорости.
    /// </summary>
    enum Mode
    {
        /// <summary>
        /// Ускорение.
        /// </summary>
        Acceleration,
        /// <summary>
        /// Обычная скорость.
        /// </summary>
        Normal
    }

    /// <summary>
    /// Интервал между ускорениями.
    /// </summary>
    [SerializeField]
    private Single intervalBetweenAccelerations = 6f;
    /// <summary>
    /// Длительность ускорения.
    /// </summary>
    [SerializeField]
    private Single accelerationsDuration = 1f;

    /// <summary>
    /// Таймер для интервала и длительности ускорения.
    /// </summary>
    private Single accelerationsTimer = 0f;
    /// <summary>
    /// Текущий режим скорости.
    /// </summary>
    private Mode currentSpeedMode= Mode.Normal;
    private void Update()
    {
        OnUpdate();
        this.accelerationsTimer += Time.deltaTime;
        if (this.currentSpeedMode == Mode.Normal)
        {
            if (this.accelerationsTimer > this.intervalBetweenAccelerations)
            {
                this.speed = ORIGIN_SPEED * 5;
                this.currentSpeedMode = Mode.Acceleration;
                this.accelerationsTimer = 0;
            }
        }
        else if(this.currentSpeedMode == Mode.Acceleration)
        {
            if (this.accelerationsTimer > this.accelerationsDuration)
            {
                this.speed = ORIGIN_SPEED;
                this.currentSpeedMode = Mode.Normal;
                this.accelerationsTimer = 0;
            }
        }
    }
}
