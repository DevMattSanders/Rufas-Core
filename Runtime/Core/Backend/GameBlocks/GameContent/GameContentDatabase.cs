using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class GameContentDatabase : GameSystem<GameContentDatabase>
    {
        [TitleGroup("UniqueIDs to GameContent")]
        [HideInEditorMode]
        public Dictionary<string, GameContentReference> keysToObjects = new Dictionary<string, GameContentReference>();

        [TitleGroup("GameContent to UniqueIDs")]
        [HideInEditorMode]
        public Dictionary<GameContentReference, string> objectsToKeys = new Dictionary<GameContentReference, string>();

        public override bool AutogenerateGameSystem()
        {
            return true;
        }

        public override void FinaliseInitialisation()
        {
            CoroutineMonoBehaviour.i.StartCoroutine(GeneratingDictionaryLists());
        }

        private IEnumerator GeneratingDictionaryLists()
        {
            int counter = 0;
            keysToObjects.Clear();
            objectsToKeys.Clear();

            foreach(ScriptableWithUniqueID next in ScriptablesUniqueIDDatabase.Instance.gameContentObjects_KeyToObject.Values)
            {
                if(next is GameContentReference)
                {
                    keysToObjects.Add(next.UniqueID, next as GameContentReference);
                    objectsToKeys.Add(next as GameContentReference, next.UniqueID);
                }

                counter++;
                if(counter >= 10)
                {
                    counter = 0;
                    yield return null;
                }
            }

            base.FinaliseInitialisation();
        }

    }
}
