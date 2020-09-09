using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using FirClient.Extensions;

namespace FirClient.Examples
{
    public class TestAsync : MonoBehaviour
    {
        // Start is called before the first frame update
        async void Start()
        {
            // Wait one second
            await new WaitForSeconds(1.0f);

            // Wait for IEnumerator to complete
            await CustomCoroutineAsync();

            await LoadModelAsync();

            // You can also get the final yielded value from the coroutine
            var value = (string)(await CustomCoroutineWithReturnValue());
            // value is equal to "asdf" here

            // Open notepad and wait for the user to exit
            var returnCode = await Process.Start("notepad.exe");

            // Load another scene and wait for it to finish loading
            await SceneManager.LoadSceneAsync("scene2");
        }

        async Task LoadModelAsync()
        {
            var assetBundle = await GetAssetBundle("www.my-server.com/myfile");
            var prefab = await assetBundle.LoadAssetAsync<GameObject>("myasset");
            GameObject.Instantiate(prefab);
            assetBundle.Unload(false);
        }

        async Task<AssetBundle> GetAssetBundle(string url)
        {
            return (await new WWW(url)).assetBundle;
        }

        IEnumerator CustomCoroutineAsync()
        {
            yield return new WaitForSeconds(1.0f);
        }

        IEnumerator CustomCoroutineWithReturnValue()
        {
            yield return new WaitForSeconds(1.0f);
            yield return "asdf";
        }
    }
}