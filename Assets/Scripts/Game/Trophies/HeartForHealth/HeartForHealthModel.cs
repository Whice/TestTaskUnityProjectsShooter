using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Game.Trophies.HeartForHealth
{
    /// <summary>
    /// Сердечко для добавления игроку ХП.
    /// </summary>
    public class HeartForHealthModel : TrophyItemModel
    {
        /// <summary>
        /// Увеличить максимальное здоровье игрока.
        /// <br/>При увеличении максимального ХП происходит дополнение текущего ХП до максимального.
        /// </summary>
        public void IncreaseMaximumHealthForPlayer()
        {
            PlayerModel.instance.maxHealthPoint += 11;
            PlayerModel.instance.healthPoints = PlayerModel.instance.maxHealthPoint;
        }
    }
}
