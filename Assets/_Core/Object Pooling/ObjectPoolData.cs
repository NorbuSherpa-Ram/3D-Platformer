using UnityEngine;

namespace ObjectPool
{
    [CreateAssetMenu(fileName = "NewObjectPoolData", menuName = "ScriptableObject/ Object Pool Data")]
    public class ObjectPoolData : ScriptableObject
    {
        [SerializeField] public Transform objectToPool;

        [Tooltip("INITIAL POOL SIZE")]
        [SerializeField] public int poolSize;
        [Tooltip("DO NOT EXPAND IF MAX SIZE IS REACHED MEAN JUST DESTROY INSTANTIATED OBJECT INSTEAD OF RETURIN I T TO THE QUEUE")]

        [SerializeField] public int maxPoolSize;
        [Tooltip("ALLOW EXPAND OF POOL FORM FORM POOL SIZE TO MAXPOOLSIZED")]
        [SerializeField] public bool shouldExpand = true;
        [SerializeField] public int expandBy = 1;


#if UNITY_EDITOR
        [TextArea(5, 5)]
        public string DevDescription = "Where and Where is being used ";
#endif
    }
}