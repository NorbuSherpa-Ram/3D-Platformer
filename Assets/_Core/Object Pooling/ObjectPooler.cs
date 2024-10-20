using System.Collections.Generic;
using UnityEngine;
namespace ObjectPool
{
    public class ObjectPooler : MonoBehaviour
    {
        [SerializeField] ObjectPoolData poolData;
        [SerializeField] Transform poolHolder;
        [SerializeField] Queue<Transform> poolGameObjectQueue;

        [SerializeField] private List<Transform> poolObjectsList;


#if UNITY_EDITOR
        [Space(10)]
        [TextArea(5, 5)]
        [SerializeField] private string DevDescription = "Why This  Pool ";
#endif

        public virtual void Awake()
        {
            poolGameObjectQueue = new Queue<Transform>();
            //THIS MEAN CAN NOT CREATE AT RUN TIME SO CREATE IT AT THE START 
            if (!poolData.shouldExpand)
                CreatePool();
        }


        public virtual void CreatePool()
        {
            for (int i = 0; i < poolData.poolSize; i++)
            {
                ExpandPool();
            }
        }

        public virtual Transform GetObjectFromPool()
        {
            if (poolGameObjectQueue.Count < 1)
            {
                if (poolData.shouldExpand)
                {
                    for (int i = 0; i < poolData.expandBy; i++)
                    {
                        ExpandPool();
                    }
                }
                else
                {
                    Debug.LogWarning("Pool is empty and expansion is not allowed.");
                    return null;
                }
            }
            Transform obj = poolGameObjectQueue.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }

        public virtual void ReturnToPool(Transform obj)
        {
            if (poolGameObjectQueue.Count > poolData.maxPoolSize)
            {
                Destroy(obj.gameObject);
            }
            else
            {
                obj.gameObject.SetActive(false);
                poolGameObjectQueue.Enqueue(obj);
            }
        }


        private void ExpandPool()
        {
            Transform newObj = Instantiate(poolData.objectToPool, poolHolder);
            newObj.gameObject.SetActive(false);
            poolGameObjectQueue.Enqueue(newObj);
        }
    }
}
