using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace AF {

    [System.Serializable]
    public class CharacterData {

        public string uuid;
        public Vector3 position;
        public Quaternion rotation;
        public float health;

        public bool isActive;
    }

    [System.Serializable]
    public class PlayerEquipmentData
    {
        public string weaponName;
        public string shieldName;
        public string helmetName;
        public string chestName;
        public string gauntletsName;
        public string legwearName;
        public string accessory1Name;
        public string accessory2Name;
    }

    [System.Serializable]
    public class GameData
    {
        public string currentSceneName;
        public int currentSceneIndex;

        public CharacterData[] characters;

        public PlayerEquipmentData playerEquipmentData;

        public string activeCameraName;
    }

    [RequireComponent(typeof(AudioSource))]
    public class SaveSystem : MonoBehaviour
    {

        public delegate void OnGameLoadEvent(GameData gameData);
        // the event itself
        public event OnGameLoadEvent OnGameLoad;


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


            // Scene Name
            Scene scene = SceneManager.GetActiveScene();
            gameData.currentSceneName = scene.name;
            gameData.currentSceneIndex = scene.buildIndex;

            // Characters
            List<CharacterData> charactersData = new List<CharacterData>();

            Character[] characters = GameObject.FindObjectsOfType<Character>(true);
            foreach (Character c in characters)
            {
                CharacterData characterData = new CharacterData();
                characterData.position = c.transform.position;
                characterData.rotation = c.transform.rotation;
                characterData.health = c.healthbox.health;
                characterData.uuid = c.uuid;
                characterData.isActive = c.gameObject.activeSelf;

                charactersData.Add(characterData);
            }

            gameData.characters = charactersData.ToArray();

            gameData.activeCameraName = CameraManager.instance.currentCamera.name;

            // Player Equipment
            EquipmentManager equipmentManager = FindObjectOfType<EquipmentManager>();
            PlayerEquipmentData playerEquipmentData = new PlayerEquipmentData();

            if (equipmentManager.weapon != null)
            {
                playerEquipmentData.weaponName = equipmentManager.weapon.name;
            }

            if (equipmentManager.shield != null)
            {
                playerEquipmentData.shieldName = equipmentManager.shield.name;
            }

            if (equipmentManager.helmet != null)
            {
                playerEquipmentData.helmetName = equipmentManager.helmet.name;
            }

            if (equipmentManager.chest != null)
            {
                playerEquipmentData.chestName = equipmentManager.chest.name;
            }

            if (equipmentManager.legwear != null)
            {
                playerEquipmentData.legwearName = equipmentManager.legwear.name;
            }

            if (equipmentManager.gauntlets != null)
            {
                playerEquipmentData.gauntletsName = equipmentManager.gauntlets.name;
            }

            if (equipmentManager.accessory1 != null)
            {
                playerEquipmentData.accessory1Name = equipmentManager.accessory1.name;
            }

            if (equipmentManager.accessory2 != null)
            {
                playerEquipmentData.accessory2Name = equipmentManager.accessory2.name;
            }

            gameData.playerEquipmentData = playerEquipmentData;

            Save(gameData, "gameData");

            MenuManager.instance.CloseAll();
        }

        public void LoadGameData()
        {
            audioSource.PlayOneShot(saveSfx);
            GameData gameData = Load<GameData>("gameData");

            // Load scene first
            SceneManager.LoadScene(gameData.currentSceneIndex);

            StartCoroutine(CallLoad(gameData));
        }

        IEnumerator CallLoad(GameData gameData)
        {
            yield return null;

            OnGameLoad(gameData);
            MenuManager.instance.CloseAll();

        }

    }
}