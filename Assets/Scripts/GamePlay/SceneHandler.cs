using UnityEngine;
using UnityEngine.SceneManagement;

namespace GamePlay
{
    public class SceneHandler : MonoBehaviour
    {
        public static void ChangeScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
