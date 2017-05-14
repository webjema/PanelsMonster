using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

namespace com.webjema.PanelsMonster.Elements
{
    [System.Serializable]
    public class SpriteSwapAdd
    {
        public Image image;
        public Sprite normal;
        public Sprite highlighted;
        public Sprite pressed;
        public Sprite disabled;
    }

    public class ButtonWithTextStates : Button
    {
        public TextMeshProUGUI textNormalState;
        public TextMeshProUGUI textHighlightedState;
        public TextMeshProUGUI textPressedState;
        public TextMeshProUGUI textDisabledState;

        public Graphic[] colorTransitionAdditionalGraphics;
        public SpriteSwapAdd[] spriteSwapAdditionalGraphics;
        public bool useAllGraphicsAutomatically;

        private Sprite normalSprite;

        private Graphic[] m_allGraphics;
        protected Graphic[] AllGraphics
        {
            get
            {
                if (m_allGraphics == null)
                {
                    m_allGraphics = targetGraphic.transform.parent.GetComponentsInChildren<Graphic>();
                }
                return m_allGraphics;
            }
        }

        public void SetText(string text)
        {
            this.textNormalState.text = text;
            this.textHighlightedState.text = text;
            this.textPressedState.text = text;
            this.textDisabledState.text = text;
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            Color buttonColor;
            TextMeshProUGUI newTextState = null;
            switch (state)
            {
                case Selectable.SelectionState.Normal:
                    buttonColor = this.colors.normalColor;
                    newTextState = this.textNormalState;
                    break;
                case Selectable.SelectionState.Highlighted:
                    buttonColor = this.colors.highlightedColor;
                    newTextState = this.textHighlightedState;
                    break;
                case Selectable.SelectionState.Pressed:
                    buttonColor = this.colors.pressedColor;
                    newTextState = this.textPressedState;
                    break;
                case Selectable.SelectionState.Disabled:
                    buttonColor = this.colors.disabledColor;
                    newTextState = this.textDisabledState;
                    break;
                default:
                    buttonColor = Color.white;
                    break;
            }
            if (base.gameObject.activeInHierarchy)
            {
                this.StateTweenText(newTextState);
                switch (this.transition)
                {
                    case Selectable.Transition.ColorTint:
                        this.ColorTween(buttonColor * this.colors.colorMultiplier, instant);
                        break;
                    case Selectable.Transition.SpriteSwap:
                        this.SwapButtonSprite(state);
                        break;
                    default:
#if PANELS_DEBUG_ON
                        Debug.LogError("[ButtonWithTextStates][DoStateTransition] Transition Is Not Supported");
#endif
                        break;
                }
            }
        } // DoStateTransition

        private void ColorTween(Color targetColor, bool instant)
        {
            if (this.targetGraphic == null)
            {
                return;
            }
            Graphic[] graphics = this.colorTransitionAdditionalGraphics;
            if (this.useAllGraphicsAutomatically)
            {
                graphics = this.AllGraphics;
            }
            if (graphics != null)
            {
                foreach (Graphic g in graphics)
                {
                    g.CrossFadeColor(targetColor, (!instant) ? this.colors.fadeDuration : 0f, true, true);
                }
            }
        } // ColorTween

        private void StateTweenText(TextMeshProUGUI newTextState)
        {
            if (newTextState == null)
            {
                return;
            }
            newTextState.text = this.textNormalState.text;
            this.textNormalState.gameObject.SetActive(false);
            this.textHighlightedState.gameObject.SetActive(false);
            this.textPressedState.gameObject.SetActive(false);
            this.textDisabledState.gameObject.SetActive(false);
            newTextState.gameObject.SetActive(true);
        } // ColorTweenText

        private void SwapButtonSprite(SelectionState state)
        {
            Image targetImage = this.targetGraphic.GetComponent<Image>();
            if (this.normalSprite == null)
            {
                this.normalSprite = targetImage.sprite;
            }
            switch (state)
            {
                case Selectable.SelectionState.Normal:
                    targetImage.sprite = this.normalSprite;
                    break;
                case Selectable.SelectionState.Highlighted:
                    targetImage.sprite = this.spriteState.highlightedSprite;
                    break;
                case Selectable.SelectionState.Pressed:
                    targetImage.sprite = this.spriteState.pressedSprite;
                    break;
                case Selectable.SelectionState.Disabled:
                    targetImage.sprite = this.spriteState.disabledSprite;
                    break;
                default:
                    targetImage.sprite = this.normalSprite;
                    break;
            }
            if (this.spriteSwapAdditionalGraphics != null && this.spriteSwapAdditionalGraphics.Length > 0)
            {
                for (int i = 0; i < this.spriteSwapAdditionalGraphics.Length; i++)
                {
                    switch (state)
                    {
                        case Selectable.SelectionState.Normal:
                            this.spriteSwapAdditionalGraphics[i].image.sprite = this.spriteSwapAdditionalGraphics[i].normal;
                            break;
                        case Selectable.SelectionState.Highlighted:
                            this.spriteSwapAdditionalGraphics[i].image.sprite = this.spriteSwapAdditionalGraphics[i].highlighted;
                            break;
                        case Selectable.SelectionState.Pressed:
                            this.spriteSwapAdditionalGraphics[i].image.sprite = this.spriteSwapAdditionalGraphics[i].pressed;
                            break;
                        case Selectable.SelectionState.Disabled:
                            this.spriteSwapAdditionalGraphics[i].image.sprite = this.spriteSwapAdditionalGraphics[i].disabled;
                            break;
                        default:
                            this.spriteSwapAdditionalGraphics[i].image.sprite = this.spriteSwapAdditionalGraphics[i].normal;
                            break;
                    }
                }
            }
        } // SwapButtonSprite

    } // ButtonWithTextStates
}