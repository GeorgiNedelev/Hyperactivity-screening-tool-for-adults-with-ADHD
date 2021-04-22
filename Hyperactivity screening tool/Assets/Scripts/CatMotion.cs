using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CatMotion : MonoBehaviour
{

	public Animator anim;
	private float time= 0;
	private bool playSound = false;
	string m_ClipName;
	AnimatorClipInfo[] m_CurrentClipInfo;

	// Use this for initialization
	void Start()
	{
		anim = GetComponent<Animator>();
		//time = Time.realtimeSinceStartup;
	}

	// Update is called once per frame
	void Update()
	{
		//Fetch the current Animation clip information for the base layer
		
	}



	public void CatMoves()
    {

		//if (anim.GetBool("walkingBool") == false)
		//{
		//	anim.SetBool("walkingBool", true);

		//}
		

		m_CurrentClipInfo = anim.GetCurrentAnimatorClipInfo(0);
		m_ClipName = m_CurrentClipInfo[0].clip.name;

		if (m_ClipName == "Walk")
		{
			if (Time.realtimeSinceStartup > time + 0.01f)
			{

				GetComponent<Transform>().position = GetComponent<Transform>().position + new Vector3(0, 0, 0.5f);
				time = Time.realtimeSinceStartup;
				
				
			}
			

		}

	}
}
