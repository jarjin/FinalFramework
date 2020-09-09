using FirClient.Data;
using FirClient.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace FirClient.HUD
{
    public class HUDObject : GameBehaviour
    {
        static Color32 HeroColor = new Color32(255, 227, 0, 255);
        static Color32 EnemyColor = new Color32(255, 0, 0, 255);

        private Text nameText;
        private Image heathBar;

        // Use this for initialization
        public void InitHud(float offsetY, string nick, NpcType type)
        {
            var currColor = type == NpcType.Hero ? HeroColor : EnemyColor;
            nameText = gameObject.GetChild<Text>("NameText");
            if (nameText != null)
            {
                nameText.text = nick;
                nameText.color = currColor;
            }
            heathBar = gameObject.GetChild<Image>("HealthBar/Fill Area");
            if (type == NpcType.Enemy)
            {
                heathBar.color = currColor;
            }
        }

        /// <summary>
        /// 设置血条值
        /// </summary>
        /// <param name="value"></param>
        public void SetHealthBarValue(float value)
        {
            if (heathBar != null)
            {
                heathBar.fillAmount = value;
            }
        }
    }
}

