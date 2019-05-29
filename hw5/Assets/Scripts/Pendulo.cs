using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulo : MonoBehaviour
{

    public bool move;

    public Transform pos1;
    // ball's position
    public Transform pos2;

    public float gravity;
    public float cDrag;
    public float initAngularVelX;
    public float initAngleX;

    public float initAngularVelZ;
    public float initAngleZ;

    private float curAngularVelX;
    public float curAngleX;

    private float curAngularVelZ;
    private float curAngleZ;

    public float stringDistance;

    private float time = 0; //Tiempo total de ejecución

    public bool method;

    Vector3Class stringVector = new Vector3Class();
    // Use this for initialization
    void Start()
    {
        if (method)
        {
            initAngularVelX /= Mathf.Rad2Deg;
            initAngularVelZ /= Mathf.Rad2Deg;
        }

        Vector3Class p1 = new Vector3Class(pos1.position);
        Vector3Class p2 = new Vector3Class(pos2.position);
        Vector3Class stringVector = (p2 - p1);

        initAngleX = transform.localEulerAngles.x;
        initAngleZ = transform.localEulerAngles.z;

        curAngleX = initAngleX * Mathf.Deg2Rad;
        curAngularVelX = initAngularVelX;

        curAngleZ = initAngleZ * Mathf.Deg2Rad;
        curAngularVelZ = initAngularVelZ;
        stringDistance = stringVector.Size();
        //regula angulos
        if (curAngleX > Mathf.PI)
        {
            curAngleX -= 2 * Mathf.PI;
        }
        if (curAngleX < -Mathf.PI)
        {
            curAngleX += 2 * Mathf.PI;
        }

        if (curAngleZ > Mathf.PI)
        {
            curAngleZ -= 2 * Mathf.PI;
        }
        if (curAngleZ < -Mathf.PI)
        {
            curAngleZ += 2 * Mathf.PI;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (move)
        {
            time += Time.deltaTime;

            if (!method)
            {

                //FORMULA CALCULO DE ANGULO:
                //PART1
                float formulaPart1X;

                formulaPart1X = initAngularVelX / Mathf.Sqrt(gravity / stringDistance) * Mathf.Sin(Mathf.Sqrt(gravity / stringDistance) * time);
                //PART2
                float formulaPart2X;

                formulaPart2X = initAngleX * Mathf.Cos(Mathf.Sqrt(gravity / stringDistance) * time);

                float newAngleX = formulaPart1X + formulaPart2X;

                //---------

                //FORMULA CALCULO DE ANGULO:
                //PART1
                float formulaPart1Z;

                formulaPart1Z = initAngularVelZ / Mathf.Sqrt(gravity / stringDistance) * Mathf.Sin(Mathf.Sqrt(gravity / stringDistance) * time);
                //PART2
                float formulaPart2Z;

                formulaPart2Z = initAngleZ * Mathf.Cos(Mathf.Sqrt(gravity / stringDistance) * time);

                float newAngleZ = formulaPart1Z + formulaPart2Z;

                //---------

                transform.localEulerAngles = new Vector3(newAngleX, 0, newAngleZ);

            }

            else
            {
                Vector3Class p1 = new Vector3Class(pos1.position);
                Vector3Class p2 = new Vector3Class(pos2.position);
                stringVector = (p2 - p1);


                Vector3Class stringX = new Vector3Class(stringVector.x, stringVector.y, stringVector.z);
                stringX.x = 0;
                Vector3Class stringZ = new Vector3Class(stringVector.x, stringVector.y, stringVector.z);
                stringZ.z = 0;

                float glX = gravity / stringX.Size();
                float glZ = gravity / stringZ.Size();

                float dragX = cDrag / stringX.Size() * curAngularVelX;
                float dragZ = cDrag / stringZ.Size() * curAngularVelZ;

                //calculo numerico pendulo
                curAngularVelX += Time.deltaTime * ((-1 * glX * Mathf.Sin(curAngleX)) - dragX);
                curAngleX += Time.deltaTime * curAngularVelX;

                curAngularVelZ += Time.deltaTime * ((-1 * glZ * Mathf.Sin(curAngleZ)) - dragZ);
                curAngleZ += Time.deltaTime * curAngularVelZ;

                //regula angulos
                if (curAngleX > Mathf.PI)
                {
                    curAngleX -= 2 * Mathf.PI;
                }
                if (curAngleX < -Mathf.PI)
                {
                    curAngleX += 2 * Mathf.PI;
                }

                if (curAngleZ > Mathf.PI)
                {
                    curAngleZ -= 2 * Mathf.PI;
                }
                if (curAngleZ < -Mathf.PI)
                {
                    curAngleZ += 2 * Mathf.PI;
                }
              
                transform.localEulerAngles = new Vector3(curAngleX * Mathf.Rad2Deg, 0, curAngleZ * Mathf.Rad2Deg); ;
            }
            DrawLines();
        }
    }

    public Vector3Class CalculateFuturePosition(float seconds)
    {

        float timeCalc = 0;

        Vector3 initAngles = transform.localEulerAngles;

        float tempAngleX = curAngleX;
        float tempAngleZ = curAngleZ;
        float tempAngularVelX = curAngularVelX;
        float tempAngularVelZ = curAngularVelZ;
        if (!method)
        {
            float predictedTime = time + seconds;
            //FORMULA CALCULO DE ANGULO:
            //PART1
            float formulaPart1X;

            formulaPart1X = initAngularVelX / Mathf.Sqrt(gravity / stringDistance) * Mathf.Sin(Mathf.Sqrt(gravity / stringDistance) * predictedTime);
            //PART2
            float formulaPart2X;

            formulaPart2X = initAngleX * Mathf.Cos(Mathf.Sqrt(gravity / stringDistance) * predictedTime);

            tempAngleX = formulaPart1X + formulaPart2X;

            //---------

            //FORMULA CALCULO DE ANGULO:
            //PART1
            float formulaPart1Z;

            formulaPart1Z = initAngularVelZ / Mathf.Sqrt(gravity / stringDistance) * Mathf.Sin(Mathf.Sqrt(gravity / stringDistance) * predictedTime);
            //PART2
            float formulaPart2Z;

            formulaPart2Z = initAngleZ * Mathf.Cos(Mathf.Sqrt(gravity / stringDistance) * predictedTime);

            tempAngleZ = formulaPart1Z + formulaPart2Z;

            //---------

            transform.localEulerAngles = new Vector3(tempAngleX, 0, tempAngleZ);

        }
        else
        {
            while (timeCalc < seconds)
            {
                timeCalc += Time.deltaTime;

                Vector3Class p1 = new Vector3Class(pos1.position);
                Vector3Class p2 = new Vector3Class(pos2.position);
                Vector3Class stringVector = (p2 - p1);


                Vector3Class stringX = new Vector3Class(stringVector.x, stringVector.y, stringVector.z);
                stringX.x = 0;
                Vector3Class stringZ = new Vector3Class(stringVector.x, stringVector.y, stringVector.z);
                stringZ.z = 0;

                float glX = gravity / stringX.Size();
                float glZ = gravity / stringZ.Size();

                float dragX = cDrag / stringX.Size() * curAngularVelX;
                float dragZ = cDrag / stringZ.Size() * curAngularVelZ;

                //calculo numerico pendulo
                tempAngularVelX += Time.deltaTime * ((-1 * glX * Mathf.Sin(tempAngleX)) - dragX);
                tempAngleX += Time.deltaTime * tempAngularVelX;

                tempAngularVelZ += Time.deltaTime * ((-1 * glZ * Mathf.Sin(tempAngleZ)) - dragZ);
                tempAngleZ += Time.deltaTime * tempAngularVelZ;

                //regula angulos
                if (tempAngleX > Mathf.PI)
                {
                    curAngleX -= 2 * Mathf.PI;
                }
                if (tempAngleX < -Mathf.PI)
                {
                    curAngleX += 2 * Mathf.PI;
                }

                if (tempAngleZ > Mathf.PI)
                {
                    curAngleZ -= 2 * Mathf.PI;
                }
                if (tempAngleZ < -Mathf.PI)
                {
                    curAngleZ += 2 * Mathf.PI;
                }

                transform.localEulerAngles = new Vector3(tempAngleX * Mathf.Rad2Deg, 0, tempAngleZ * Mathf.Rad2Deg);
            }
        }

        Vector3Class predictedTarget = new Vector3Class(pos2.position);
        transform.localEulerAngles = initAngles;
        return predictedTarget;
    }

        public void SetMove(bool var)
    {
        move = var;
    }
    public Transform GetBall()
    {
        return pos2;
    }
    public void ResetPendulum()
    {
        if (method)
        {
            initAngularVelX /= Mathf.Rad2Deg;
            initAngularVelZ /= Mathf.Rad2Deg;
        }

        Vector3Class p1 = new Vector3Class(pos1.position);
        Vector3Class p2 = new Vector3Class(pos2.position);
        Vector3Class stringVector = (p2 - p1);

        initAngleX = transform.localEulerAngles.x;
        initAngleZ = transform.localEulerAngles.z;

        curAngleX = initAngleX * Mathf.Deg2Rad;
        curAngularVelX = initAngularVelX;

        curAngleZ = initAngleZ * Mathf.Deg2Rad;
        curAngularVelZ = initAngularVelZ;
        stringDistance = stringVector.Size();
        //regula angulos
        if (curAngleX > Mathf.PI)
        {
            curAngleX -= 2 * Mathf.PI;
        }
        if (curAngleX < -Mathf.PI)
        {
            curAngleX += 2 * Mathf.PI;
        }

        if (curAngleZ > Mathf.PI)
        {
            curAngleZ -= 2 * Mathf.PI;
        }
        if (curAngleZ < -Mathf.PI)
        {
            curAngleZ += 2 * Mathf.PI;
        }
    }
    public void NewPendulumAngle(Vector3Class angle)
    {
        transform.localEulerAngles = angle.GetValues();
    }
    public void SetAngularVelX(float value)
    {
        initAngularVelX = value;
    }
    public void SetAngularVelZ(float value)
    {
        initAngularVelZ = value;
    }
    public void SetGravity(float value)
    {
        gravity = value;
    }
    public void SetFriction(float value)
    {
        cDrag = value;
    }
    void DrawLines()
    {
        Vector3Class angularVec = new Vector3Class(curAngularVelX, 0, curAngularVelZ);
        angularVec = angularVec.CrossProduct(angularVec, stringVector);

        Debug.DrawLine(pos2.position, angularVec.GetValues(), Color.green);
    }
}