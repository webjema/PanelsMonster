// Copyright © 2017 Nick Kavunenko. All rights reserved.
// Contact me: nick@kavunenko.com
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace com.webjema.PanelsMonster.Elements
{
    public class ButtonWithText : Button
    {
        public TextMeshProUGUI text;
        public Color textNormalColor;
        public Color textHighlightedColor;
        public Color textPressedColor;
        public Color textDisabledColor;

        public Graphic[] colorTransitionAdditionalGraphics;
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

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            Color buttonColor;
            Color textColor;
            switch (state)
            {
                case Selectable.SelectionState.Normal:
                    buttonColor = this.colors.normalColor;
                    textColor = this.textNormalColor;
                    break;
                case Selectable.SelectionState.Highlighted:
                    buttonColor = this.colors.highlightedColor;
                    textColor = this.textHighlightedColor;
                    break;
                case Selectable.SelectionState.Pressed:
                    buttonColor = this.colors.pressedColor;
                    textColor = this.textPressedColor;
                    break;
                case Selectable.SelectionState.Disabled:
                    buttonColor = this.colors.disabledColor;
                    textColor = this.textDisabledColor;
                    break;
                default:
                    buttonColor = Color.white;
                    textColor = Color.white;
                    break;
            }
            if (base.gameObject.activeInHierarchy)
            {
                this.ColorTweenText(textColor * this.colors.colorMultiplier, instant);
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
                        Debug.LogError("[ButtonWithText][DoStateTransition] Transition Is Not Supported");
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

        private void ColorTweenText(Color targetColor, bool instant)
        {
            if (this.text == null)
            {
                return;
            }
            this.text.color = targetColor;
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
        } // SwapButtonSprite

    } // ButtonWithText
}