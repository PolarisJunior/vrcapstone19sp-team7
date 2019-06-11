// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// using System.IO;

// namespace Valve.VR.InteractionSystem
// {
//     public class PathMaker : MonoBehaviour
//     {
//         // List<LineRenderer> lineRenderers = new List<LineRenderer>();
//         // Start is called before the first frame update

//         public Hand hand;

//         public LineRenderer lineRenderer;

//         public LineRenderer curvedLineRenderer;

//         public BezierCurve curveFitter;

//         // public GameObject centerPoint;

//         public float radiusFromCenter = 25;


//         public float pathWidth = 1;


//         // refresh rate in s
//         public float refreshRate = 0.1f;

//         public Camera playerCamera;

//         public AudioSource audioSync;

//         public float timeDraw = -1;

//         private float timeCounter = 0;


//         private bool tracking = false;

//         private float acc;

        

//         private SteamVR_Action_Boolean gripAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");
//         private SteamVR_Action_Boolean pinchAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");

//         private SteamVR_Action_Boolean teleportAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Teleport");

//         void Start()
//         {
//             lineRenderer.useWorldSpace = false;
//             curvedLineRenderer.useWorldSpace = false;

//         }

//         // Update is called once per frame
//         void Update()
//         {
//             if (Input.GetKeyUp(KeyCode.T))
//             {
//                 AddVertexFromHand();
//             }

//             if (pinchAction.GetStateUp(hand.handType))
//             {
//                 AddVertexFromHand();
//             }

//             if (timeDraw > 0 && timeCounter > timeDraw) {
//                 tracking = false;
//             }
            

//             if (tracking)
//             {
//                 if (acc > refreshRate)
//                 {
//                     AddVertexFromHand();
//                     acc -= refreshRate;
//                 }

//                 acc += Time.deltaTime;
//                 timeCounter += Time.deltaTime;
//             }


//             if (gripAction.GetStateUp(hand.handType))
//             {
//                 if (!tracking && audioSync != null) {
//                     audioSync.Play();
//                 }
//                 tracking = !tracking;
//                 timeCounter = 0;
//             }

//             // make vertex with mouse
//             if (Input.GetMouseButtonDown(0))
//             {
//                 AddVertexFromMouse();
//             }

//             // save
//             if (Input.GetKeyUp(KeyCode.X))
//             {
//                 SavePath();
//             }

//             if (teleportAction.GetStateUp(hand.handType))
//             {
//                 SavePath();
//             }
//         }

//         void AddVertexFromHand()
//         {
//             // Vector3 dir = (hand.transform.forward);
//             Vector3 dir = hand.transform.localRotation * Vector3.forward;
//             AddVertexFromDir(dir);
//         }

//         void AddVertexFromMouse()
//         {
//             Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
//             var dir = ray.direction;
//             AddVertexFromDir(dir);
//         }

//         void AddVertexFromDir(Vector3 dir)
//         {
//             vertexCount = vertexCount + 1;
//             // lineRenderer.SetPosition(vertexCount - 1, dir * radiusFromCenter + centerPoint.transform.position);
//             lineRenderer.SetPosition(vertexCount - 1, dir * radiusFromCenter + transform.localPosition);

//             if (curveFitter != null && (vertexCount == 4 || ((vertexCount - 4) % 3) == 0) && vertexCount > 3)
//             {
//                 Vector3[] ps = positions;
//                 List<Vector3> points = new List<Vector3>();

//                 for (int i = ps.Length - 4; i < ps.Length; i++)
//                 {
//                     points.Add(ps[i]);
//                 }

//                 List<Vector3> transformed = curveFitter.TransformPointsCubic(points);

//                 foreach (var v in transformed)
//                 {
//                     curvedLineRenderer.positionCount++;
//                     curvedLineRenderer.SetPosition(curvedLineRenderer.positionCount - 1, v);
//                 }

//             }

//             if (vertexCount > 1)
//             {
//                 var startPos = lineRenderer.GetPosition(vertexCount - 2);
//                 var endPos = lineRenderer.GetPosition(vertexCount - 1);
//                 var dif = endPos - startPos;

//                 float length = Vector3.Distance(startPos, endPos);
//                 var offset = dif.normalized * length / 2;



//                 // GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
//                 // BoxCollider collider = cube.AddComponent<BoxCollider>();
//                 // cube.transform.position = startPos + offset;
//                 // cube.transform.parent = transform;

//                 // collider.size = new Vector3(pathWidth, pathWidth, length);
//                 // cube.transform.localScale = collider.size;

//                 // Quaternion rot = Quaternion.FromToRotation(cube.transform.forward, dif);
//                 // cube.transform.rotation = rot;
//             }
//         }

//         void SavePath()
//         {

//             List<string> paths = new List<string>();
//             paths.Add("Assets/Paths/Resources/m3.txt");
//             if (curvedLineRenderer != null) {
//                 paths.Add("Assets/Paths/Resources/m3.txt");
//             }

//             Vector3[][] ps = {positions, bezierPositions};
//             int i = 0;
            
//             foreach (var p in paths)
//             {
//                 string path = p;

//                 StreamWriter writer = new StreamWriter(path, false);
//                 Vector3[] vertices = ps[i];

//                 foreach (var vertex in vertices)
//                 {
//                     writer.WriteLine(vertex.x + "," + vertex.y + "," + vertex.z);
//                 }

//                 writer.Close();
//                 i++;
//             }
//             Debug.Log("path saved");
//         }

//         public Vector3[] positions
//         {
//             get
//             {
//                 int count = lineRenderer.positionCount;
//                 Vector3[] p = new Vector3[count];

//                 lineRenderer.GetPositions(p);
//                 return p;
//             }
//         }

//         public Vector3[] bezierPositions
//         {
//             get
//             {
//                 int count = curvedLineRenderer.positionCount;
//                 Vector3[] p = new Vector3[count];

//                 curvedLineRenderer.GetPositions(p);
//                 return p;
//             }
//         }

//         private int vertexCount
//         {
//             get
//             {
//                 return lineRenderer.positionCount;
//             }

//             set
//             {
//                 lineRenderer.positionCount = value;
//             }
//         }
//     }
// }