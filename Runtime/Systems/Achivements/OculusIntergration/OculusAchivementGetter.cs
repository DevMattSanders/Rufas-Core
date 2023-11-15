using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Platform;
using Oculus.Platform.Models;
using Sirenix.OdinInspector;

namespace Rufas.Achivements
{
    public class OculusAchivementGetter : MonoBehaviour
    {
        [SerializeField] private VoidEvent OnOculusInitEvent;
        
        [SerializeField] private Achievement[] achivements;

        private void Start()
        {
            OnOculusInitEvent.AddListener(GetAchivementData);
        }

        private void OnDestroy()
        {
            OnOculusInitEvent.RemoveListener(GetAchivementData);
        }

        private void GetAchivementData()
        {
            List<string> namesList = new List<string>();
            foreach (Achievement a in achivements)
            {
                namesList.Add(a.apiName);
            }
            string[] namesArray = namesList.ToArray();

            ////Oculus.Platform.Models.AchievementDefinitionList defintionList = Oculus.Platform.Achievements.GetDefinitionsByName(namesArray);

            Debug.Log("Getting Achivement Data");
            Achievements. (namesArray).OnComplete(
            (Message<AchievementProgressList> msg) =>
            {
                Debug.Log(msg.Data.Count);
                foreach (var achievement in msg.Data)
                {
                    Debug.Log(achievement.Name + ": " + achievement.IsUnlocked.ToString());
                }
            }
              );
        }

        [Button] private void LockAllAchivement()
        {

        }
    }
}
