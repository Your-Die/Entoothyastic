using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FaceManager : MonoBehaviour
{
    private static FaceManager _instance;
    public static FaceManager Instance { get { return _instance; } }
        
    private FaceController currentFace = null;
    public List<FaceController> faces;
    public float offset;
    public float distanceFromCamera;
    private Vector3 camPos;
    private Vector3 inPos;
    private Vector3 outPos;
    private Vector3 centerPos;
    private bool canAnimate = false;
    private enum States { In, Center, Out};
    private States currentState;
    private int teethInCurrentFace;
    private int teethCleaned = 0;
    private int currentIndex = 0;
    public UnityEvent gameOver;

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        camPos = Camera.main.transform.position;
        inPos = new Vector3(camPos.x - offset, camPos.y, camPos.z + distanceFromCamera);
        outPos = new Vector3(camPos.x + offset, camPos.y, camPos.z + distanceFromCamera);
        centerPos = new Vector3(camPos.x, camPos.y, camPos.z + distanceFromCamera);

        foreach(FaceController face in faces)
        {
            face.gameObject.SetActive(false);
        }

        NextFace();
    }

    // Update is called once per frame
    void Update()
    {
        if (canAnimate)
        {
            if(currentState == States.In)
            {
                currentFace.gameObject.transform.position = Vector3.Lerp(currentFace.gameObject.transform.position, centerPos, 0.2f);
                SetFinalPositionAndState(centerPos);
            }
            else if(currentState == States.Center)
            {
                currentFace.gameObject.transform.position = Vector3.Lerp(currentFace.gameObject.transform.position, outPos, 0.2f);
                SetFinalPositionAndState(outPos);
            }
        }
    }

    private FaceController SelectNextFace()
    {
        if(currentIndex >= faces.Count)
        {
            Debug.Log(currentIndex);
            Debug.Log(faces.Count);
            EndGame();
            return currentFace;
        }
        else
        {
            FaceController result = faces[currentIndex];
            result.gameObject.SetActive(true);
            currentIndex++;
            return result;
        }
    }
    


    private void MoveFaceToStartPos()
    {
        if(currentFace != null)
        {
            currentFace.gameObject.transform.position = inPos;
        }
    }

    public void NextFace()
    {
        if (currentFace)
        {
            AudioManager.instance.Play("Relief3");
            currentFace.gameObject.SetActive(false);
        }

        currentFace = SelectNextFace();
        MoveFaceToStartPos();
        currentState = States.In;
        canAnimate = true;
    }    
   
    private void SetFinalPositionAndState( Vector3 finalPosition)
    {
        if(currentFace.gameObject.transform.position.x + 0.2f >= finalPosition.x)
        {
            currentFace.gameObject.transform.position = finalPosition;
            canAnimate = false;

            if(currentState == States.In)
            {
                currentState = States.Center;
            }
            else if(currentState == States.Center)
            {
                currentState = States.Out;
            }

            CheckState();
        }
    }

    private void CheckState()
    {
        if(currentState == States.Out)
        {
            NextFace();
        }
    }


    public void EndGame()
    {
        Debug.Log("game over");
        GameManager.Instance.SetScore(GameTimer.currentTime);

        gameOver.Invoke();
    }
}
