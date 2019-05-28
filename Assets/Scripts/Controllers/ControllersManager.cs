using System;
using InterativaSystem.Controllers.Sound;
using UnityEngine;

namespace InterativaSystem.Controllers
{
    public class ControllersManager : MonoBehaviour
    {
        public bool dropdownMenu;
        public Transform selectedChild;

        public Type componentToAdd;

        public void Populate()
        {
            for (int i = 0, n = transform.childCount; i < n; i++)
            {
                switch (transform.GetChild(i).name)
                {
                    case "Id":
                        if (transform.GetChild(i).gameObject.GetComponent<IdsController>() == null)
                            transform.GetChild(i).gameObject.AddComponent<IdsController>();
                        break;
                    case "Register":
                        if (transform.GetChild(i).gameObject.GetComponent<RegisterController>() == null)
                            transform.GetChild(i).gameObject.AddComponent<RegisterController>();
                        break;
                    case "IO":
                        if (transform.GetChild(i).gameObject.GetComponent<IOController>() == null)
                            transform.GetChild(i).gameObject.AddComponent<IOController>();
                        break;
                    case "Page":
                        if (transform.GetChild(i).gameObject.GetComponent<PagesController>() == null)
                            transform.GetChild(i).gameObject.AddComponent<PagesController>();
                        break;
                    case "Time":
                        if (transform.GetChild(i).gameObject.GetComponent<TimeController>() == null)
                            transform.GetChild(i).gameObject.AddComponent<TimeController>();
                        break;
                    case "Score":
                        if (transform.GetChild(i).gameObject.GetComponent<ScoreController>() == null)
                            transform.GetChild(i).gameObject.AddComponent<ScoreController>();
                        break;
                    case "SoundBGM":
                        if(transform.GetChild(i).gameObject.GetComponent<BGMController>() == null)
                            transform.GetChild(i).gameObject.AddComponent<BGMController>();
                        break;
                    case "SoundSFX":
                        if (transform.GetChild(i).gameObject.GetComponent<SFXController>() == null)
                            transform.GetChild(i).gameObject.AddComponent<SFXController>();
                        break;
                }
            }
        }
    }
}