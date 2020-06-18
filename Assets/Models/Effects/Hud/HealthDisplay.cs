using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TimelineIso
{

    public class HealthDisplay : MonoBehaviour
    {
        private CharacterHealth characterHealth;
        private CanvasGroup healthBarGroup;
        private RectTransform healthBarRect;
        public Text damageDisplay;
        private float lastScale = 1f;

        // Start is called before the first frame update
        void Start()
        {
            this.characterHealth = this.GetComponentInParent<CharacterHealth>();
            this.healthBarGroup = this.GetComponent<CanvasGroup>();
            this.healthBarRect = this.transform.GetChild(0).GetComponent<RectTransform>();
            this.characterHealth.Damage.AddListener(DisplayDamage);
        }

        void DisplayDamage(int damage)
        {
            if (damageDisplay)
            {
                var dd = Instantiate(damageDisplay, this.transform);
                dd.text = damage.ToString();
                StartCoroutine(FadeOut(dd, .5f));
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (characterHealth.Health.current < characterHealth.Health.max)
            {
                this.healthBarGroup.alpha = 1;
            } else
            {
                this.healthBarGroup.alpha = 0;
            }
            this.lastScale = Mathf.Lerp(lastScale, characterHealth.Health.current / characterHealth.Health.max, 10 * Time.deltaTime);

            this.healthBarRect.localScale = new Vector3(lastScale, 1, 1);

        }

        IEnumerator FadeOut(Text t, float time)
        {
            float accum = 0f;
            while (accum < time)
            {
                accum += Time.deltaTime;
                var pos = t.rectTransform.localPosition;
                t.rectTransform.localPosition = new Vector3(pos.x, pos.y + Time.deltaTime * 200, pos.z);
                t.rectTransform.localScale = t.rectTransform.localScale * (1f - 0.7f* Time.deltaTime);
                yield return null;
            }
            Destroy(t.gameObject);
        }


    


    }
}
