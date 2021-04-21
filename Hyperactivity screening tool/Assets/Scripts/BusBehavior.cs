using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusBehavior : MonoBehaviour
{
    float elapsed = 0f;
    float period = 120f;
    bool direction;
    public float speed;
    Vector3 pointBPos = new Vector3(-2683, -300, -5960);
    Vector3 pointAPos = new Vector3(-2683, -300, 3631);
    // Start is called before the first frame update
    void Start()
    {
        speed = 500f;
        direction = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (elapsed >= period)
        {
            Move(direction);

            if (elapsed >=145)
            {
                elapsed = 0;

                switch (direction)
                {
                    case true:
                        direction = false;
                    break;

                    case false:
                        direction = true;
                    break;
                }
            }
        }
        elapsed += Time.deltaTime;
    }

    void Move(bool direct)
    {
        float step = speed * Time.deltaTime;

        if (direct == true)
        {
            transform.rotation = Quaternion.Euler (0, 270 ,0);
            transform.position = Vector3.MoveTowards(transform.position, pointBPos, step);
        }

        if (direct == false)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            transform.position = Vector3.MoveTowards(transform.position, pointAPos, step);
        }
    }
}
