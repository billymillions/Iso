using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace TimelineIso
{
    public class LockonReticule : MonoBehaviour
    {
        private Enemy enemy;
        private CharacterSelector charSelector;
        private Image img;

        // Use this for initialization
        void Start()
        {
            this.enemy = this.GetComponentInParent<Enemy>();
            this.charSelector = GameObject.Find("CharSelector").GetComponent<CharacterSelector>();
            this.img = this.GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            var lockon = this.charSelector.Selected.GetComponent<PlayerLockon>();
            if (lockon && enemy && lockon.Locked == enemy)
            {
                this.img.enabled = true;
            } else
            {
                this.img.enabled = false;
            }
        }
    }
}
