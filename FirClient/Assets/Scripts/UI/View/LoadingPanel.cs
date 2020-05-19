//File creation in 2019年05月03日 09:58:29 星期五
using UnityEngine.UI;

namespace FirClient.UI.View
{
    public class LoadingPanel : GameBehaviour
    {
        public Slider slider_loadingBar;
        public Text txt_status;

        void Awake()
        {
            this.slider_loadingBar = transform.Find("#slider_loadingBar").GetComponent<Slider>();
            this.txt_status = transform.Find("#txt_status").GetComponent<Text>();
        }

        void OnDestroy()
        {
            this.slider_loadingBar = null;
            this.txt_status = null;
        }
    }
}
