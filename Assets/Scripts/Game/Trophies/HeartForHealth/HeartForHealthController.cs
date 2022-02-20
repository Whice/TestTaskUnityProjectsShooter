using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Game.Trophies.HeartForHealth
{
    class HeartForHealthController:TrophyItemController
    {
        public HeartForHealthModel heartForHealthModel
        {
            get => this.model as HeartForHealthModel;
        }

        private void OnTriggerEnter(UnityEngine.Collider other)
        {
            //При столкновении с игроком, увеличить ему ХП.
            if (other.gameObject.name == "PlayerFront")
            {
                this.heartForHealthModel.IncreaseMaximumHealthForPlayer();

                this.gameObject.SetActive(false);
            }
        }
    }
}
