// 2D Sky
// Version: 1.1.6
// Unity 4.7.1 or higher and Unity 5.3.4 or higher compatilble, see more info in Readme.txt file.
//
// Author:				Gold Experience Team (http://www.ge-team.com)
//
// Unity Asset Store:	https://www.assetstore.unity3d.com/en/#!/content/11158
// GE Store:			http://www.ge-team.com/store/en/products/elementals/
//
// Please direct any bugs/comments/suggestions to support e-mail (geteamdev@gmail.com).

#region Namespaces

using UnityEngine;
using System.Collections;

using UnityEngine.UI;

#endregion

// ######################################################################
// GE_Elementals_Demo does switches category and show/hide the particle effects.
// This script file should be attached with Main Camera in the Demo scene.
// ######################################################################

public class GE_Elementals_Demo : MonoBehaviour
{
	// ########################################
	// Variables
	// ########################################

	#region Variables

	// Colors for category and paritcle Text
	public Color m_ColorFire = new Color(1, 1, 1, 1);
	public Color m_ColorWater = new Color(1, 1, 1, 1);
	public Color m_ColorWind = new Color(1, 1, 1, 1);
	public Color m_ColorEarth = new Color(1, 1, 1, 1);
	public Color m_ColorThunder = new Color(1, 1, 1, 1);
	public Color m_ColorIce = new Color(1, 1, 1, 1);
	public Color m_ColorLight = new Color(1, 1, 1, 1);
	public Color m_ColorDarkness = new Color(1, 1, 1, 1);

	// Categories to store particles
	public GameObject[] m_PrefabListFire;
	public GameObject[] m_PrefabListWater;
	public GameObject[] m_PrefabListWind;
	public GameObject[] m_PrefabListEarth;
	public GameObject[] m_PrefabListThunder;
	public GameObject[] m_PrefabListIce;
	public GameObject[] m_PrefabListLight;
	public GameObject[] m_PrefabListDarkness;

	// Index of current category
	int m_CurrentCategoryIndex = -1;

	// Index of current particle
	int m_CurrentParticleIndex = -1;

	// Name of current category
	string m_CategoryName = "";

	// Name of current particle
	string m_ParticleName = "";

	// Current category
	GameObject[] m_CurrentCategory = null;

	// Current particle
	GameObject m_CurrentParticle = null;

	// Unity UI elements
	Text m_Category = null;
	Text m_Particle = null;

	// Mouse and Touches
	Vector3 m_PreviousMousePosition;
	bool m_ShowParticleWhenTouchEnded = false;

	#endregion // Variables

	// ########################################
	// MonoBehaviour functions
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.html
	// ########################################

	#region MonoBehaviour

	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
	void Start()
	{
		// There is any particle in prefab list
		if (m_PrefabListFire.Length > 0 ||
			m_PrefabListWind.Length > 0 ||
			m_PrefabListWater.Length > 0 ||
			m_PrefabListEarth.Length > 0 ||
			m_PrefabListIce.Length > 0 ||
			m_PrefabListThunder.Length > 0 ||
			m_PrefabListLight.Length > 0 ||
			m_PrefabListDarkness.Length > 0)
		{
			// Reset category and particle indices
			m_CurrentCategoryIndex = 0;
			m_CurrentParticleIndex = 0;

			// Show first particle of first category
			ShowParticle();
		}

	}

	// Update is called every frame, if the MonoBehaviour is enabled.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
	void Update()
	{

		// If there is single touch
		if (Input.touchCount == 1)
		{
			// Get touch
			Touch CurrentTouch = Input.GetTouch(0);

			// Touch began
			if (CurrentTouch.phase == TouchPhase.Began)
			{
				// Prepare to show particle when touch ended
				m_ShowParticleWhenTouchEnded = true;
			}
			// Touch moved
			else if (CurrentTouch.phase == TouchPhase.Moved)
			{
				// Canceled showing particle when touch ended
				m_ShowParticleWhenTouchEnded = false;
			}
			// Touch ended
			else if (CurrentTouch.phase == TouchPhase.Ended)
			{
				// Show current particle when tap
				if (m_ShowParticleWhenTouchEnded == true)
				{
					ShowParticle();
				}

				// Reset m_ShowParticleWhenTouchEnded
				m_ShowParticleWhenTouchEnded = false;
			}
		}
		// If there is no touch then respond to mouse input
		else if (Input.touchCount == 0)
		{
			// Mouse left button down
			if (Input.GetMouseButtonDown(0))
			{
				// Keep mouse position when mouse left button down
				m_PreviousMousePosition = Input.mousePosition;
			}
			// Mouse left button up
			else if (Input.GetMouseButtonUp(0))
			{
				// find distance between previous mouse position and current position
				float distance = Vector2.Distance(Input.mousePosition, m_PreviousMousePosition);

				// Show current particle when mouse up without dragged (or dragged for a little distance)
				if (distance < 5)
				{
					ShowParticle();
				}
			}
		}

		// There is any particle in prefab list
		if (m_CurrentCategoryIndex != -1 && m_CurrentParticleIndex != -1)
		{
			// User released Up arrow key
			if (Input.GetKeyUp(KeyCode.UpArrow))
			{
				// Show previouse category
				PreviousCategory();
			}
			// User released Down arrow key
			else if (Input.GetKeyUp(KeyCode.DownArrow))
			{
				// Show next category
				NextCategory();
			}
			// User released Left arrow key
			else if (Input.GetKeyUp(KeyCode.LeftArrow))
			{
				// Show previous particle
				PreviousParticle();
			}
			// User released Right arrow key
			else if (Input.GetKeyUp(KeyCode.RightArrow))
			{
				// Show next particle
				NextParticle();
			}
			// User released Space key
			else if (Input.GetKeyUp(KeyCode.Space))
			{
				// Show particle
				ShowParticle();
			}
		}
	}

	#endregion // MonoBehaviour

	// ########################################
	// Switch category functions
	// In runtime, press Left/Right key or use previous/next category buttons to change category.
	// ########################################

	#region Switch category

	// Switch to previous category then show first particle in its list
	public void PreviousCategory()
	{
		m_CurrentCategoryIndex--;
		m_CurrentParticleIndex = 0;
		ShowParticle();
	}

	// Switch to next category then show first particle in its list
	public void NextCategory()
	{
		m_CurrentCategoryIndex++;
		m_CurrentParticleIndex = 0;
		ShowParticle();
	}

	#endregion // Switch category

	// ########################################
	// Switch particle functions
	// In runtime, press Left/Right key or use previous/next particle buttons to change particles.
	// ########################################

	#region Switch particle

	// Switch to previous particle
	public void PreviousParticle()
	{
		m_CurrentParticleIndex--;
		ShowParticle();
	}

	// Switch to next particle
	public void NextParticle()
	{
		m_CurrentParticleIndex++;
		ShowParticle();
	}

	#endregion // Switch particle

	// ########################################
	// Show Particles function
	// ########################################

	#region Show Particles

	// Remove old particle and create new in the scene
	public void ShowParticle()
	{
		// Clamp m_CurrentCategoryIndex between 0 to 7
		if (m_CurrentCategoryIndex > 7)
		{
			m_CurrentCategoryIndex = 0;
		}
		else if (m_CurrentCategoryIndex < 0)
		{
			m_CurrentCategoryIndex = 7;
		}

		// Find Category Text element
		GameObject go = GameObject.Find("Text Category_Name");
		if (go)
			m_Category = go.GetComponent<Text>();

		// Find Particle Text element
		go = GameObject.Find("Text Particle_Name");
		if (go)
			m_Particle = go.GetComponent<Text>();

		// Update current m_CurrentCategory and m_CategoryName

		// Fire category
		if (m_CurrentCategoryIndex == 0)
		{
			m_CurrentCategory = m_PrefabListFire;		// set m_CurrentCategory to new list
			m_CategoryName = "FIRE";					// set category name
			m_Category.color = m_ColorFire;				// set m_ColorFire color to m_Category text
			m_Particle.color = m_ColorFire;				// set m_ColorFire color to m_Particle text
		}
		// Water category
		else if (m_CurrentCategoryIndex == 1)
		{
			m_CurrentCategory = m_PrefabListWater;      // set m_CurrentCategory to new list
			m_CategoryName = "WATER";					// set category name
			m_Category.color = m_ColorWater;			// set m_ColorWater color to m_Category text
			m_Particle.color = m_ColorWater;			// set m_ColorWater color to m_Particle text
		}
		// Wind category
		else if (m_CurrentCategoryIndex == 2)
		{
			m_CurrentCategory = m_PrefabListWind;       // set m_CurrentCategory to new list
			m_CategoryName = "WIND";					// set category name
			m_Category.color = m_ColorWind;				// set m_ColorWind color to m_Category text
			m_Particle.color = m_ColorWind;				// set m_ColorWind color to m_Particle text
		}
		// Earth category
		else if (m_CurrentCategoryIndex == 3)
		{
			m_CurrentCategory = m_PrefabListEarth;      // set m_CurrentCategory to new list
			m_CategoryName = "EARTH";					// set category name
			m_Category.color = m_ColorEarth;			// set m_ColorEarth color to m_Category text
			m_Particle.color = m_ColorEarth;			// set m_ColorEarth color to m_Particle text
		}
		// Thunder category
		else if (m_CurrentCategoryIndex == 4)
		{
			m_CurrentCategory = m_PrefabListThunder;	// set m_CurrentCategory to new list
			m_CategoryName = "THUNDER";					// set category name
			m_Category.color = m_ColorThunder;			// set m_ColorThunder color to m_Category text
			m_Particle.color = m_ColorThunder;			// set m_ColorThunder color to m_Particle text
		}
		// Ice category
		else if (m_CurrentCategoryIndex == 5)
		{
			m_CurrentCategory = m_PrefabListIce;
			m_CategoryName = "ICE";						// set category name
			m_Category.color = m_ColorIce;				// set m_ColorIce color to m_Category text
			m_Particle.color = m_ColorIce;				// set m_ColorIce color to m_Particle text
		}
		// Light category
		else if (m_CurrentCategoryIndex == 6)
		{
			m_CurrentCategory = m_PrefabListLight;      // set m_CurrentCategory to new list
			m_CategoryName = "LIGHT";					// set category name
			m_Category.color = m_ColorLight;			// set m_ColorLight color to m_Category text
			m_Particle.color = m_ColorLight;			// set m_ColorLight color to m_Particle text
		}
		// Darkness category
		else if (m_CurrentCategoryIndex == 7)
		{
			m_CurrentCategory = m_PrefabListDarkness;   // set m_CurrentCategory to new list
			m_CategoryName = "DARKNESS";				// set category name
			m_Category.color = m_ColorDarkness;			// set m_ColorDarkness color to m_Category text
			m_Particle.color = m_ColorDarkness;			// set m_ColorDarkness color to m_Particle text
		}

		// Update UI text
		m_Category.text = m_CategoryName;

		// Make m_CurrentParticleIndex be rounded
		if (m_CurrentParticleIndex >= m_CurrentCategory.Length)
		{
			m_CurrentParticleIndex = 0;
		}
		else if (m_CurrentParticleIndex < 0)
		{
			m_CurrentParticleIndex = m_CurrentCategory.Length - 1;
		}

		// Update current m_ParticleName
		m_ParticleName = m_CurrentCategory[m_CurrentParticleIndex].name;

		// Update UI text
		m_Particle.text = "(" + (m_CurrentParticleIndex + 1) + "/" + m_CurrentCategory.Length + ") " + m_ParticleName;

		// Remove old particle
		if (m_CurrentParticle != null)
		{
			DestroyObject(m_CurrentParticle);
		}

		// Create new particle
		m_CurrentParticle = (GameObject) Instantiate(m_CurrentCategory[m_CurrentParticleIndex]);
	}

	#endregion // Show Particles
}
