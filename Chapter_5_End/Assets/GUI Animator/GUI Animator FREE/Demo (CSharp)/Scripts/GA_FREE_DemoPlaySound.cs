// GUI Animator FREE
// Version: 1.0.1
// Unity 4.7.1 or higher and Unity 5.3.4 or higher compatilble, see more info in Readme.txt file.
//
// Author:				Gold Experience Team (http://www.ge-team.com)
//
// Unity Asset Store:					https://www.assetstore.unity3d.com/en/#!/content/58843
// GE Store:							http://www.ge-team.com/store/en/products/gui-animator-free/
// Full version on Unity Asset Store:	https://www.assetstore.unity3d.com/en/#!/content/28709
// Full version on GE Store:			http://www.ge-team.com/store/en/products/gui-animator-for-unity-ui/
//
// Please direct any bugs/comments/suggestions to geteamdev@gmail.com

#region Namespaces

using UnityEngine;
using System.Collections;

#endregion // Namespaces

// ######################################################################
// GA_FREE_DemoPlaySound class
// This class plays AudioClip and button sounds.
// ######################################################################

public class GA_FREE_DemoPlaySound : MonoBehaviour
{

	// ########################################
	// Variables
	// ########################################
	
	#region Variables

	public int m_AudioSourceCount = 2;
	
	AudioSource[] m_AudioSource = null;
	
	public AudioClip m_Audio_Button1 = null;
	public AudioClip m_Audio_Button2 = null;

	#endregion // Variables

	// ########################################
	// MonoBehaviour Functions
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.html
	// ########################################

	#region MonoBehaviour
	
	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
	void Start () {

		// Create AudioSource list
		if(m_AudioSource==null)
		{
			m_AudioSource = new AudioSource[m_AudioSourceCount];
			
			for(int i=0;i<m_AudioSource.Length;i++)
			{
				AudioSource pAudioSource =  this.gameObject.AddComponent<AudioSource>();
				pAudioSource.rolloffMode = AudioRolloffMode.Linear;
				m_AudioSource[i] = pAudioSource;
			}
		}
	}
	
	// Update is called every frame, if the MonoBehaviour is enabled.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
	void Update () {		
	}
	
	#endregion // MonoBehaviour

	// ########################################
	// Play sound Functions
	// ########################################

	#region Play sound
	
	// Play AudioClip
	void PlayOneShot(AudioClip pAudioClip)
	{

		for(int i=0;i<m_AudioSource.Length;i++)
		{
			if(m_AudioSource[i].isPlaying == false)
			{
				m_AudioSource[i].PlayOneShot(pAudioClip);
				break;
			}
		}
	}

	// Play m_Audio_Button1 audio clip
	public void PlaySoundButton1()
	{
		PlayOneShot(m_Audio_Button1);
	}
	
	// Play m_Audio_Button2 audio clip
	public void PlaySoundButton2()
	{
		PlayOneShot(m_Audio_Button2);
	}
	
	#endregion // Play sound
}

