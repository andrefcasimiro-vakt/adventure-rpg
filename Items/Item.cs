using UnityEngine;
using System.Collections;

namespace AF
{
    [CreateAssetMenu(menuName = "Item / New Item")]
    public class Item : ScriptableObject
    {

        public string name;

        [TextArea]
        public string description;

    }

}
