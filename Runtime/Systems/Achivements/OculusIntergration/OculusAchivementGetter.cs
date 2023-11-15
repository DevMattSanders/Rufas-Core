using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Platform;
using Oculus.Platform.Models;

namespace Rufas.Achivements
{
    public class OculusAchivementGetter : MonoBehaviour
    {
        [SerializeField] private Achivement[] achivements;

        private void Start()
        {
            Oculus.Platform.Core.Initialize();

            List<string> namesList = new List<string>();
            foreach (Achivement a in achivements)
            {
                namesList.Add(a.apiName);
            }
            string[] namesArray = namesList.ToArray();

            //Oculus.Platform.Models.AchievementDefinitionList defintionList = Oculus.Platform.Achievements.GetDefinitionsByName(namesArray);

            //    Achievements.GetProgressByName(new string[] { LIKES_TO_WIOptional }).OnComplete(
            //    (Message<AchievementProgressList> msg) =>
            //    {
            //        foreach (var achievement in msg.Data)
            //        {
            //            if (achievement.Name == LIKES_TO_WIN)
            //            {
            //                m_likesToWinUnlocked = achievement.IsUnlocked;
            //            }
            //        }
            //    }
            //      );
        }
    }
}
