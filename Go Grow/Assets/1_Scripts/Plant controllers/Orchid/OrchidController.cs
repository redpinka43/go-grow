using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace FluffyHippo
{
    public class OrchidController : MonoBehaviour
    {
        public int seed;

        [Header("Leaves")]
        public GameObject[] leafStagesObjects;
        int _leafStage = -1;

        public GameObject bigLeaf;
        public int maxNumLeaves;
        public float xShift;
        public float yShift;
        public float scaleChange;

        [HelpBox("Positive rotation change is counter-clockwise.", HelpBoxMessageType.Info)]
        public float rotationChange;
        public Vector2 rotationRange;
        int _numLeaves;
        List<GameObject> _leavesList = new List<GameObject>();

        private SpriteRenderer _bigLeafSprRenderer;
        private float _totalRotationAmount;
        public float pastLeavesScaleChange = 1f;

        [Header("Stem")]
        public GameObject stemPrefab;
        private SpriteShapeController _stem;
        public Spline2DComponent spline2DComponent;
        public float tangentScale;
        public Vector2[] testPoints;
        public OrchidStemParams orchidStemParams;
        // public CatmullRomSpline catmullRomSpline;

        void Start()
        {
            Debug.Assert(bigLeaf, "Please assign bigLeaf.", this);
            Random.InitState(seed);

            _bigLeafSprRenderer = bigLeaf.GetComponentInChildren<SpriteRenderer>();
            Debug.Assert(_bigLeafSprRenderer, "Child sprite renderer missing.", bigLeaf);
        }

        public void GrowLeaf()
        {
            if (_leafStage >= leafStagesObjects.Length)
            {
                return;
            }

            if (_leafStage >= 0 && _leafStage < leafStagesObjects.Length)
            {
                leafStagesObjects[_leafStage].SetActive(false);
            }

            _leafStage++;
            if (_leafStage < leafStagesObjects.Length)
            {
                leafStagesObjects[_leafStage].SetActive(true);
            }
        }

        public void GrowStem()
        {
            Debug.Assert(stemPrefab.GetComponent<SpriteShapeController>(),
                "stemPrefab needs a SpriteShapeController component.", this);
            Debug.Assert(spline2DComponent, "Please assign spline2DComponent.", this);
            Debug.Assert(orchidStemParams.miniStem, "Please assign miniStem", this);
            Debug.Assert(orchidStemParams.minDistanceBetweenMinistems > 0f, "Please make sure" +
                    "orchidStemParams.minDistanceBetweenMinistems > 0f.", this);

            // Reset
            if (_stem)
            {
                Destroy(_stem.gameObject);
            }
            _stem = Instantiate(stemPrefab, transform).GetComponent<SpriteShapeController>();
            Extensions.GetRidOfCloneEnding(_stem.gameObject);

            Spline spline = _stem.spline;
            spline.Clear();
            GenerateSpline();
            spline2DComponent.InitSpline();
                
            CreateStemFromCatmullSpline(spline);
            SpawnMiniStems();

            _stem.transform.localPosition = Vector3.zero;
            _stem.RefreshSpriteShape();
        }

        private void CreateStemFromCatmullSpline(Spline spline)
        {
            BezierSplineNode[] bezierNodes = spline2DComponent.GetSpline().ToBezierSpline();
            float nodeHeight = 1f;
            float minNodeHeight = orchidStemParams.minNodeHeight;

            for (int i = 0; i < bezierNodes.Length; i++)
            {
                spline.InsertPointAt(i, bezierNodes[i].position);
                spline.SetTangentMode(i, ShapeTangentMode.Continuous);
                spline.SetRightTangent(i, bezierNodes[i].rightTangent / tangentScale);
                spline.SetLeftTangent(i, bezierNodes[i].leftTangent / tangentScale);

                if (i > bezierNodes.Length / 2f)
                {
                    nodeHeight -= (1 - minNodeHeight) / (bezierNodes.Length / 2f);
                }
                spline.SetHeight(i, nodeHeight);
            }
        }

        private void SpawnMiniStems()
        {
            BezierSplineNode[] miniStemPossibleNodes = spline2DComponent.GetSpline().GetRegularIntervalBezierNodes( orchidStemParams.minDistanceBetweenMinistems );
            GameObject miniStem;
            float miniStemAngle;
            float spawnRange = orchidStemParams.percentOfStemFlowersCanSpawnOn;
            float chanceOfMinistemSpawn;
            float j;
            HorizontalDirection lastDirection = HorizontalDirection.LEFT;

            for (int i = 0; i < miniStemPossibleNodes.Length; i++)
            {
                j = (float) i / miniStemPossibleNodes.Length;
                chanceOfMinistemSpawn = 2f * (j - 1) + 1;
                //  1f - Mathf.Pow(1f / spawnRange * j - 1f / spawnRange, 2);
                Debug.Log("i = " + i + ", chanceOfMinistemSpawn = " + chanceOfMinistemSpawn);
                if (Random.Range(0f, 1f) > chanceOfMinistemSpawn)
                {
                    Debug.Log("breaking on i = " + i);
                    continue;
                }
                
                miniStem = Instantiate(orchidStemParams.miniStem, _stem.transform);
                miniStem.transform.localPosition = miniStemPossibleNodes[i].position;
                miniStemAngle = Vector3.Angle(Vector3.up,
                                            miniStemPossibleNodes[i].rightTangent.normalized);

                // Alternate sides
                if (lastDirection == HorizontalDirection.LEFT)
                {
                    lastDirection = HorizontalDirection.RIGHT;
                    miniStemAngle = (miniStemAngle + 180f) % 360f;
                }
                else
                {
                    lastDirection = HorizontalDirection.LEFT;
                }

                // Angle funny business
                if (miniStemAngle > 180f)
                {
                    miniStemAngle -= 360f;
                }

                if (miniStemAngle < 0f)
                {
                    miniStemAngle += (miniStemAngle < 90f) ? 90f : -90f;
                }
                else
                {
                    miniStemAngle -= (miniStemAngle < -90f) ? 90f : -90f;
                }

                miniStemAngle -= 180f;
                if (Mathf.Abs(miniStemAngle) > 90f)
                {
                    miniStemAngle = (miniStemAngle > 0f) ? 90f : -90f;
                }

                miniStem.transform.Rotate(0f, 0f, miniStemAngle);
            }
        }

        private void GenerateSpline()
        {
            Spline2D spline = spline2DComponent.GetSpline();
            spline.Clear();

            // Generate some new nodes
            List<Vector2> points = new List<Vector2>();

            Vector2 newPoint = orchidStemParams.potNodePosition;
            spline.AddPoint(newPoint + new Vector2(0.5f, -3f)); // Pre-pot node
            spline.AddPoint(newPoint); // Pot node

            newPoint = GetStakeNodePosition();
            spline.AddPoint(newPoint);

            float minAngle = orchidStemParams.minAngleBetweenNodes;
            float maxAngle = orchidStemParams.maxAngleBetweenNodes;
            float distance = orchidStemParams.maxDistanceBetweenNodes;
            float angle = 0f;
            Vector2 sinRange = GetNewSinRange();
            Vector3 newVector;
            for (int i = 1; i < orchidStemParams.numNodes; i++)
            {
                distance = Random.Range(orchidStemParams.minDistanceBetweenNodes, distance);
                angle += GetNextStemAngle(i, orchidStemParams.numNodes, sinRange);
                newVector = Quaternion.Euler(0, 0, angle) * new Vector3(0, distance, 0);
                newPoint += (Vector2)newVector;
                spline.AddPoint(newPoint);
            }
            spline.AddPoints(points);
        }

        Vector2 GetNewSinRange()
        {
            float x = Random.Range(0f, 0.5f);
            float y = Random.Range(0f, 1.5f);
            if (x > y)
            {
                float newX = y;
                y = x;
                x = newX;
            }
            return new Vector2(x, y);
        }

        float GetNextStemAngle(int i, int totalNumNodes, Vector2 sinRange)
        {
            float ratio = (float)i / totalNumNodes;  // 1 / 4 = 0.25
            // Assume sinRange = 0, 1
            // something(ratio) = 1;
            // Assume sinRange = 0.25, 0.75

            // Get a spot within the clamped range
            float x = ratio * (sinRange.y - sinRange.x) + sinRange.x;
            return 90f * Mathf.Sin(x * 2 * Mathf.PI);
        }

        Vector2 GetStakeNodePosition()
        {
            // - In stem generator, get the height and position of the stake
            // - Have the 1st node be the root, and the 2nd node be on the
            //   right side of the stake, above the halfway mark on the stake.

            // Perhaps randomly generate a position in a box that's to the right of the
            // stake and above the halfway point of the stake

            GameObject stake = orchidStemParams.stakeSprite;
            Debug.Assert(stake.GetComponent<SpriteRenderer>(), "Stake needs a SpriteRenderer component.", stake);

            float stakeHeight = stake.GetComponent<SpriteRenderer>().bounds.size.y / transform.localScale.x;
            Vector3 stakePosition = stake.transform.localPosition;
            Vector2 lowerLeftCorner = (Vector2)stakePosition;
            Vector2 upperRightCorner = new Vector2(stakePosition.x + orchidStemParams.stakeBoxWidth,
                                                    stakePosition.y + stakeHeight / 2f);

            return new Vector2(Random.Range(lowerLeftCorner.x, upperRightCorner.x),
                               Random.Range(lowerLeftCorner.y, upperRightCorner.y));
        }

        #region Old code
        // Old GrowLeaf code
        // public void GrowLeaf()
        // {

        // if (_numLeaves >= maxNumLeaves)
        // {
        //     return;
        // }

        // _totalRotationAmount += Random.Range(rotationRange.x, rotationRange.y);

        // _numLeaves++;
        // GameObject newLeaf = Instantiate(bigLeaf, transform);
        // _leavesList.Add(newLeaf);
        // Extensions.GetRidOfCloneEnding(newLeaf);
        // newLeaf.name += " " + _numLeaves;
        // newLeaf.GetComponentInChildren<SpriteRenderer>().sortingOrder = _numLeaves * -1;

        // float scale = 1f - ((_numLeaves - 1) * scaleChange);
        // newLeaf.transform.localScale *= scale;

        // newLeaf.transform.localPosition += new Vector3(xShift * (_numLeaves - 1), 0f, 0f);
        // newLeaf.transform.localPosition += new Vector3(0f, yShift * (_numLeaves - 1), 0f);

        // if (_numLeaves % 2 == 0)
        // {
        //     Transform leafTransform = newLeaf.transform; 
        //     leafTransform.ChangeLocalPosition_X(leafTransform.localPosition.x * -1f);
        //     leafTransform.ChangeLocalScale_X(leafTransform.localScale.x * -1f);

        //     newLeaf.transform.Rotate(new Vector3(0f, 0f, (_totalRotationAmount + rotationChange * -2f) * -1f) );
        // }
        // else
        // {
        //     newLeaf.transform.Rotate(new Vector3(0f, 0f, _totalRotationAmount) );
        // }

        // for (int i = 0; i < _leavesList.Count - 1; i++)
        // {
        //     Debug.Log("y scale = " + _leavesList[i].transform.localPosition.y);
        //     _leavesList[i].transform.ChangeLocalScale_Y( _leavesList[i].transform.localScale.y * pastLeavesScaleChange );
        // }
        // }
        #endregion
    }
}
