using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace AF
{
    [System.Serializable]
    public class LoadingCutsceneMoment
    {
        public AudioClip musicToPlay;

        public GameObject slideshow;

        public string actorName = "";

        [TextArea]
        public string message = "";

        public float waitToAppear = 0.1f;
    }

    public class LoadingCutsceneEvent : MonoBehaviour
    {
        public GameObject dialogueUI;
        public Text messageUIText;
        public Text actorNameUIText;

        public List<LoadingCutsceneMoment> events = new List<LoadingCutsceneMoment>();

        public bool goToNext = false;

        public string sceneName;

        AsyncOperation sceneLoadingOperation;

        public UnityEvent eventOnCutsceneEnding;

        InputActions inputActions;

        private void Start()
        {
            inputActions = new InputActions();

            // Movement Input
            inputActions.PlayerActions.Attack.performed += ctx =>
            {
                Debug.Log("Hey");
                goToNext = true;
            };

            inputActions.Enable();

            dialogueUI.SetActive(false);

            sceneLoadingOperation = SceneManager.LoadSceneAsync(sceneName);
            sceneLoadingOperation.allowSceneActivation = false;

            StartCoroutine(Begin());

        }


        IEnumerator Begin()
        {
            yield return null;

            foreach (LoadingCutsceneMoment ev in events) {
                goToNext = false;
                Debug.Log("Loading progress: " + sceneLoadingOperation.progress * 100 + "%");


                yield return new WaitForSeconds(ev.waitToAppear);

                if (ev.musicToPlay != null)
                {
                    BGMManager.instance.PlayMusic(ev.musicToPlay);
                }

                ev.slideshow.gameObject.SetActive(true);

                if (String.IsNullOrEmpty(ev.actorName) == false)
                {
                    actorNameUIText.text = ev.actorName;
                    actorNameUIText.gameObject.SetActive(true);
                }
                else
                {
                    actorNameUIText.gameObject.SetActive(false);
                }

                if (String.IsNullOrEmpty(ev.message) == false)
                {
                    messageUIText.text = ev.message;
                    dialogueUI.SetActive(true);
                }
                else
                {
                    dialogueUI.SetActive(false);
                }

                yield return new WaitWhile(() => goToNext == false);
                ev.slideshow.gameObject.SetActive(false);
                dialogueUI.SetActive(false);
            }


            yield return new WaitUntil(() => sceneLoadingOperation.progress >= 0.9f);



            yield return null;

            LoadNextScene();

        }

        public void GoToNext()
        {
            goToNext = true;
        }

        public void LoadNextScene()
        {
            Player player = FindObjectOfType<Player>(true);

            GameObject systemUI = FindObjectOfType<SystemUI>(true).gameObject;

            systemUI.gameObject.SetActive(true);
            player.gameObject.SetActive(true);

            sceneLoadingOperation.allowSceneActivation = true;
        }

    }

}
