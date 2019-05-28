using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Sound;
using UnityEngine;

namespace InterativaSystem.Models
{
    public class ModelsManager : MonoBehaviour
    {
        public bool dropdownMenu;
        public Transform selectedChild;

        public GenericModel componentToAdd;

        public void Populate()
        {
            for (int i = 0, n = transform.childCount; i < n; i++)
            {
                switch (transform.GetChild(i).name)
                {
                    case "Ids":
                        if (transform.GetChild(i).gameObject.GetComponent<IdsData>() == null)
                            transform.GetChild(i).gameObject.AddComponent<IdsData>();
                        break;
                    case "Register":
                        if (transform.GetChild(i).gameObject.GetComponent<RegistrationData>() == null)
                            transform.GetChild(i).gameObject.AddComponent<RegistrationData>();
                        break;
                    case "GameSettings":
                        if (transform.GetChild(i).gameObject.GetComponent<GameSettingsData>() == null)
                            transform.GetChild(i).gameObject.AddComponent<GameSettingsData>();
                        break;
                    case "Resources":
                        if (transform.GetChild(i).gameObject.GetComponent<ResourcesDataBase>() == null)
                            transform.GetChild(i).gameObject.AddComponent<ResourcesDataBase>();
                        break;
                    case "Sound":
                        if (transform.GetChild(i).gameObject.GetComponent<SoundDatabase>() == null)
                            transform.GetChild(i).gameObject.AddComponent<SoundDatabase>();
                        break;
                    case "Score":
                        if (transform.GetChild(i).gameObject.GetComponent<ScoreData>() == null)
                            transform.GetChild(i).gameObject.AddComponent<ScoreData>();
                        break;
                }
            }
        }
    }
}