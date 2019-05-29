using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{

    public Pendulo pendulum;
    public CanvasScript canvas;
    public ENTICourse.IK.InverseKinematics roboticHand;

    public bool startSimulation = false;

    public float timeToStartSimulation;
    public float timeToStopHandFollow;
    public float timeToStopSimulation;
    public float timeToPredictStopPosition;

    float timer;

    enum States { init, simulationHandFollow, simulationNoHand, closeHand, stopPendulum, done };

    States statusSimulation;
    public Transform defHandPos;
    public Transform target;

    public ENTICourse.IK.InverseKinematics[] fingers;
    
    private float initStopTime;
    private bool openedHands;

    void Start()
    {
        pendulum.GetComponent<Pendulo>();
        canvas.GetComponent<CanvasScript>();
        roboticHand.GetComponent<ENTICourse.IK.InverseKinematics>();

        pendulum.SetMove(false);
        timer = 0;
        statusSimulation = States.done;
        roboticHand.NewDestination(defHandPos);

        initStopTime = timeToPredictStopPosition;
        openedHands = false;
        roboticHand.perfFollow = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (statusSimulation != States.done)
         timer += Time.deltaTime;

        if (startSimulation /*Input.GetKeyDown("space")*/ && statusSimulation == States.done)
        {
            startSimulation = false;
            statusSimulation = States.init;
            roboticHand.NewDestination(pendulum.GetBall());
            for (int i = 0; i < 5; i++)
            {
                fingers[i].GetComponent<FingerManager>().CloseHand();
            }
        }

        if (statusSimulation == States.init && timer > timeToStartSimulation) //empieza movimiento pendulo con la mano
        {
            pendulum.SetMove(true);
            roboticHand.perfFollow = true;
            timer = 0;
            statusSimulation = States.simulationHandFollow;
        }

        if (statusSimulation == States.simulationHandFollow && timer > timeToStopHandFollow - 0.2f && !openedHands) //la mano empieza a soltar la bola
        {
            openedHands = true;
            for (int i = 0; i < 5; i++)
            {
                fingers[i].GetComponent<FingerManager>().OpenHand();
            }
        }

        if (statusSimulation == States.simulationHandFollow && timer > timeToStopHandFollow) //la mano deja de seguir al pendulo
        {
            roboticHand.perfFollow = false;
            target.position = defHandPos.position;
            roboticHand.NewDestination(target);
            statusSimulation = States.simulationNoHand;
        }

        if (statusSimulation == States.simulationNoHand && timer > timeToStopSimulation) //la mano se dirige a la posicion predict
        {
            bool reachable = false;

            Vector3Class tPos = pendulum.CalculateFuturePosition(timeToPredictStopPosition);
            Vector3Class baseHandPos = new Vector3Class(roboticHand.transform.position);
            while (!reachable)
            {
                tPos = pendulum.CalculateFuturePosition(timeToPredictStopPosition);

                if (tPos.Distance(tPos, baseHandPos) < 2.2f)
                    reachable = true;
                else
                    timeToPredictStopPosition += 0.5f;
            }
            target.position = tPos.GetValues();
            roboticHand.NewDestination(target);
            timer = 0;
            statusSimulation = States.closeHand;
        }

        if (statusSimulation == States.closeHand && timer > timeToPredictStopPosition - 0.3f) //se empiezan a cerrar las manos
        {
            statusSimulation = States.stopPendulum;
            for (int i = 0; i < 5; i++)
            {
                fingers[i].GetComponent<FingerManager>().CloseHand();
            }
        }

        if (statusSimulation == States.stopPendulum && timer > timeToPredictStopPosition) //se para el pendulo
        {
            pendulum.SetMove(false);
            canvas.AllowInputP(true);
            timeToPredictStopPosition = initStopTime;
            statusSimulation = States.done;
            openedHands = false;
        }
    }

    void StartTheSimulation() {
        startSimulation = true;
    }
    public void SetTimeToStop(int value) {
        timeToStopSimulation = (float)value;
    }
}

