using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace AF
{

    public class NewGameButton : MonoBehaviour
    {
        public int startingSceneIndex;

        public void OnClick()
        {
            SceneManager.LoadScene(startingSceneIndex);

        }
    }

}