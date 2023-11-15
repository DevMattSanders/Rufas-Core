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
            Debug.Log("Getting Achivement Data");
            List<string> namesList = new List<string>();
            foreach (Achievement a in achivements)
            {
                namesList.Add(a.apiName);
            }
            string[] namesArray = namesList.ToArray();
            
            //This will only return achievments that have any or all progress completed. Achievments with zero progress will not be shown
            Achievements.GetProgressByName(namesArray).OnComplete(
            (Message<AchievementProgressList> msg) =>
                {
                    Dictionary<string, AchievementProgress> allAchievmentNamesWithProgress = new Dictionary<string,AchievementProgress>();

                    //If theres no achievment found here that matches any name in the namesArray, the local achievement scriptable should be set to 'locked'.
                    foreach (var achievement in msg.Data)
                    {
                        //This part will only run for achievments that have progress or have been unlocked.

                        //Do something here with an achievment that has either been unlocked or had progress put towards it.   
                        allAchievmentNamesWithProgress.Add(achievement.Name, achievement);
                    }

                    foreach(Achievement a in achivements)
                    {
                        if (allAchievmentNamesWithProgress.ContainsKey(a.apiName))
                        {
                            //Found achievment on server. Syncing progress with our local achievement scriptable
                            a.unlocked = allAchievmentNamesWithProgress[a.apiName].IsUnlocked;

                            if (a.unlockType == UnlockType.Count) {
                                Debug.Log("C");
                                a.SetCounter(allAchievmentNamesWithProgress[a.apiName].Count);
                            } else if (a.unlockType == UnlockType.Bitfield) {
                                Debug.Log("B");
                                a.SetBitfield(allAchievmentNamesWithProgress[a.apiName].Bitfield);
                            }
                        }
                        else
                        {
                            //Could not find any progress towards this achievment
                            a.unlocked = false;
                            a.SetCounter(0);
                            a.SetBitfield("");
                        }
                    }
                }
            );
        }
    }

}
