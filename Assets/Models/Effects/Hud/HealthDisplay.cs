using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace TimelineIso
{

    public class HealthDisplay : MonoBehaviour
    {
        private CharacterHealth characterHealth;
        private CanvasGroup healthBarGroup;
        private RectTransform containerRect;
        private RectTransform healthBarRect;
        private RectTransform shieldBarRect;
        public Text damageDisplay;
        private float lastScale = 1f;

        // Start is called before the first frame update
        void Start()
        {
            this.characterHealth = this.GetComponentInParent<CharacterHealth>();
            this.healthBarGroup = this.GetComponent<CanvasGroup>();
            this.containerRect = this.GetComponent<RectTransform>();
            this.healthBarRect = this.transform.GetChild(0).GetComponent<RectTransform>();
            this.shieldBarRect = this.transform.GetChild(1).GetComponent<RectTransform>();
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
            var health = this.characterHealth.Health;
            if (health.current < health.max || health.shield > 0)
            {
                this.healthBarGroup.alpha = 1;
            } else
            {
                this.healthBarGroup.alpha = 0;
            }
            var max = Math.Max((health.current + health.shield), health.max);
            var width = this.containerRect.rect.width;
            var healthWidth = width * health.current / max;
            var shieldWidth = width * health.shield / max;
            healthBarRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, healthWidth);
            shieldBarRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, healthWidth, shieldWidth);
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
