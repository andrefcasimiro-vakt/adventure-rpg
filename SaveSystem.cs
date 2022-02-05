using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace AF {

    [System.Serializable]
    public class CharacterData {

        public string uuid;
        public Vector3 position;
        public Quaternion rotation;
        public float health;

        public string state;

        public bool isActive;
    }

    [System.Serializable]
    public class GameData
    {
        public CharacterData[] characters;

        public string activeCameraName;
    }

    [RequireComponent(typeof(AudioSource))]
    public class SaveSystem : MonoBehaviour
    {

        public static SaveSystem instance;

        public AudioClip saveSfx;
        AudioSource audioSource => GetComponent<AudioSource>();

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }

            DontDestroyOnLoad(this.gameObject);
        }

        public void Save<T>(T data, string fileName)
        {
            string path = Path.Combine(Application.persistentDataPath, fileName + ".json");

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            string jsonDataString = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, jsonDataString);
        }

        public T Load<T>(string fileName)
        {
            string path = Path.Combine(Application.persistentDataPath, fileName + ".json");

            if (!File.Exists(path))
            {
                return default(T);
            }

            string loadedJsonString = File.ReadAllText(path);
            return JsonUtility.FromJson<T>(loadedJsonString);
        }

        public void SaveGameData()
        {
            audioSource.PlayOneShot(saveSfx);
            GameData gameData = new GameData();

            // Characters
            List<CharacterData> charactersData = new List<CharacterData>();

            Character[] characters = GameObject.FindObjectsOfType<Character>(true);
            foreach (Character c in characters)
            {
                CharacterData characterData = new CharacterData();
                characterData.position = c.transform.position;
                characterData.rotation = c.transform.rotation;
                characterData.health = c.health;
                characterData.uuid = c.uuid;
                characterData.isActive = c.gameObject.activeSelf;

                Enemy enemy = c as Enemy;
                if (enemy != null)
                {
                    characterData.state = enemy.currentState.ToString();
                }
                else
                {
                    characterData.state = "";
                }

                charactersData.Add(characterData);
            }

            gameData.characters = charactersData.ToArray();

            gameData.activeCameraName = CameraManager.instance.currentCamera.name;

            Save(gameData, "gameData");

            MainMenu.instance.Close();
        }

        public void LoadGameData()
        {
            audioSource.PlayOneShot(saveSfx);
            GameData gameData = Load<GameData>("gameData");

            // Create lookup
            Dictionary<string, CharacterData> characterDataDictionary = new Dictionary<string, CharacterData>();
            foreach (CharacterData charData in gameData.characters) {
                characterDataDictionary.Add(charData.uuid, charData);
            }

            Character[] characters = GameObject.FindObjectsOfType<Character>(true);
            foreach (Character c in characters)
            {
                if (characterDataDictionary.ContainsKey(c.uuid)) { 
                    CharacterData charData = characterDataDictionary[c.uuid];
                    c.transform.position = charData.position;
                    c.transform.rotation = charData.rotation;

                    if (charData.isActive)
                    {
                        c.Revive(charData.health);
                    }
                    else
                    {
                        c.gameObject.SetActive(false);
                    }

                    if (charData.state != "")
                    {
                        Enemy enemy = c as Enemy;
                        if (enemy != null)
                        {
                            if (charData.state.Contains("PatrolState"))
                            {
                                PatrolState state = enemy.GetComponentInChildren<PatrolState>();
                                enemy.SwitchToNextState(state);
                            }
                            else if (charData.state.Contains("CombatState"))
                            {
                                CombatState state = enemy.GetComponentInChildren<CombatState>();
                                enemy.SwitchToNextState(state);
                            }
                            else if (charData.state.Contains("ChaseState"))
                            {
                                ChaseState state = enemy.GetComponentInChildren<ChaseState>();
                                enemy.SwitchToNextState(state);
                            }
                            else if (charData.state.Contains("IdleState"))
                            {
                                IdleState state = enemy.GetComponentInChildren<IdleState>();
                                enemy.SwitchToNextState(state);
                            }
                        }
                    }
                }
            }

            // Saved Player Camera
            if (gameData.activeCameraName != null && gameData.activeCameraName != "") {
                GameObject targetCamera = null;
                GameObject cameraHolder = GameObject.Find("Cameras");
                foreach (Transform t in cameraHolder.transform)
                {
                    if (t.gameObject.name == gameData.activeCameraName)
                    {
                        targetCamera = t.gameObject;
                    }
                }

                if (targetCamera  != null)
                {
                    CameraManager.instance.SwitchCamera(targetCamera);
                }
            }

            MainMenu.instance.Close();
        }

    }
}