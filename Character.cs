using UnityEngine;
using System.Collections;
using System.Linq;
namespace AF
{

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Healthbox))]
    [RequireComponent(typeof(EquipmentManager))]
    public class Character : MonoBehaviour, ISaveable
    {
        public readonly int hashDodging = Animator.StringToHash("Dodging");

        [HideInInspector] public Animator animator => GetComponent<Animator>();
        [HideInInspector] public Healthbox healthbox => GetComponent<Healthbox>();
        [HideInInspector] public EquipmentManager equipmentManager => GetComponent<EquipmentManager>();

        public string uuid;

        protected void Awake()
        {
            uuid = this.gameObject.name;
        }

        protected void Start()
        {
            SaveSystem.instance.OnGameLoad += OnGameLoaded;
        }

        public bool IsDodging()
        {
            return animator.GetBool(hashDodging);
        }

        public void OnGameLoaded(GameData gameData)
        {
            CharacterData characterData = gameData.characters.First(character => character.uuid == this.uuid);

            if (characterData == null)
            {
                return;
            }

            transform.position = characterData.position;
            transform.rotation = characterData.rotation;


            this.gameObject.SetActive(characterData.isActive);

            healthbox.health = characterData.health;

            if (healthbox.health <= 0)
            {
                healthbox.Die();
            }
        }
    }
}
