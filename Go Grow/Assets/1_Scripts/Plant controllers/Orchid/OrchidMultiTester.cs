using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FluffyHippo
{
    public class OrchidMultiTester : MonoBehaviour
    {
        public OrchidController orchidPrefab;
        public int numOrchids;
        public float distanceBetweenOrchids;
        OrchidController[] orchids;
        public Vector2 numNodesRange;

        void Start()
        {
            Debug.Assert(orchidPrefab, "Please assign orchidPrefab.", this);
            orchids = new OrchidController[numOrchids];
            int numOnEachSide = (numOrchids - 1) / 2;

            for (int i = 0; i < numOrchids; i++)
            {
                if (i == numOnEachSide)
                {
                    if (numOnEachSide % 2 == 0)
                    {
                        orchids[i] = orchidPrefab;
                    }
                    else
                    {
                        orchids[i] = orchidPrefab;
                    }
                }
                else
                {
                    orchids[i] = CreateCopy(i);
                }
            }

            orchidPrefab.gameObject.name += " " + (numOnEachSide + 1);
        }

        OrchidController CreateCopy(int i)
        {  
            GameObject gameObject = Instantiate(orchidPrefab.gameObject, transform);
            OrchidController orchidController = gameObject.GetComponent<OrchidController>();
            Extensions.GetRidOfCloneEnding(gameObject);
            gameObject.name += " " + i;

            int placesFromMiddle = i - (numOrchids - 1) / 2;
            gameObject.transform.localPosition += new Vector3(distanceBetweenOrchids * 
                                                              placesFromMiddle, 0, 0);
            return orchidController;
        }

        public void GrowStems()
        {
            foreach (OrchidController orchid in orchids)
            {
                orchid.orchidStemParams.numNodes = (int) Random.Range(numNodesRange.x, 
                                                                      numNodesRange.y + 1);
                orchid.GrowStem();
            }
        }
    }
}
