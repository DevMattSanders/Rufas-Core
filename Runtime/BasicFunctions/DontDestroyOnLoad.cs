using UnityEngine;

namespace Rufas.BasicFunctions
{
    public class DontDestroyOnLoad : MonoBehaviour
    {      
        protected void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
