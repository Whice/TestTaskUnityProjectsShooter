using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Characters.Enemy.SimpleEnemies
{
    /// <summary>
    /// Контроллер для головы простого игрока,
    /// чтобы регистрировать попадания в голову.
    /// </summary>
    class SimpleEnemyHeadController: MonoBehaviour
    {
        /// <summary>
        /// Модель врага, которому принадлежит голова.
        /// </summary>
        [SerializeField]
        private SimpleEnemyModel simpleEnemyModel = null;

        private void OnTriggerEnter(Collider other)
        {
            //При попадании пули в голову, сразу убить игрока.
            if (other.gameObject.name == "Bullet(Clone)")
            {
                this.simpleEnemyModel.KillEnemy();
            }
        }
    }
}
