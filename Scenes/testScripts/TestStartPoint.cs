using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.webjema.PanelsMonster;

public class TestStartPoint : MonoBehaviour {

	void Start () {
        PanelsManager.Instance
            .GetPanel(PanelName.WelcomePanel)
            .SetProperty(PanelPropertyName.title, "Welcome Title Text!")
            .Show();
        //PanelsManager.Instance.GetPanel("NonamePanel").Show();
    }

}
