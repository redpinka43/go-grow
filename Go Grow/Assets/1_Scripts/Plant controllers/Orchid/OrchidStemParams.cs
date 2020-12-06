using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FluffyHippo
{
    [Serializable]
    public class OrchidStemParams
    {
        public float minAngleBetweenNodes;
        public float maxAngleBetweenNodes;
        public float angleIncreaseBetweenNodes;

        public float minDistanceBetweenNodes;
        public float maxDistanceBetweenNodes;
        public Vector2 potNodePosition;
        public int numNodes;
        public GameObject stakeSprite;
        public float stakeBoxWidth = 5f;

        public float minNodeHeight;
        public GameObject miniStem;
        public float minDistanceBetweenMinistems;
        public float percentOfStemFlowersCanSpawnOn = 0.5f;
        // [Tooltip("Turn the minDistanceBetweenMinistems down when you turn this parameter up. " + 
        //          "This results in a more even spacing of ministems.")]
        // public float miniStemChanceDivisor = 3f;
    }
}
