using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TimelineIso
{
    public class CharacterSelector : MonoBehaviour
    {
        // Start is called before the first frame update
        public EntityComponent[] Characters;
        private int selectedCharacter = 0;

        public EntityIdentifier SelectedCharacter { get => Characters[selectedCharacter].identifier; }
        public Transform SelectedCharacterTransform { get => Characters[selectedCharacter].transform; }
        public EntityComponent Selected { get => Characters[selectedCharacter]; }

        public void CycleCharacter()
        {
            if (Characters.Length <= 0)
            {
                return;
            }
            selectedCharacter = (selectedCharacter + 1) % Characters.Length;
        }
    }
}