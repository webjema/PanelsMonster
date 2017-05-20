using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.webjema.PanelsMonster
{
    public interface ICustomScreenFade
    {
        IEnumerator CustomFadeOut();
    }
}