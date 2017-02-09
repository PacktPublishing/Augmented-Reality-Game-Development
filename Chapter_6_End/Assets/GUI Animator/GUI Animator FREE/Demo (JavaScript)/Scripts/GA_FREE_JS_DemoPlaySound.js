// GUI Animator FREE
// Version: 1.0.1
// Compatilble: Unity 4.7.1 or higher and Unity 5.3.4 or higher, more info in Readme.txt file.
//
// Author:	Gold Experience Team (http://www.ge-team.com)
//
// Unity Asset Store:					https://www.assetstore.unity3d.com/en/#!/content/58843
// GE Store:							http://www.ge-team.com/store/en/products/gui-animator-free/
// Full version on Unity Asset Store:	https://www.assetstore.unity3d.com/en/#!/content/28709
// Full version on GE Store:			http://www.ge-team.com/store/en/products/gui-animator-for-unity-ui/
//
// Please direct any bugs/comments/suggestions to geteamdev@gmail.com

// ######################################################################
// GA_FREE_DemoPlaySound class
// This class plays AudioClip and button sounds.
// ######################################################################

class GA_FREE_JS_DemoPlaySound extends MonoBehaviour {

	// ########################################
	// Variables
	// ########################################
	
    var m_AudioSourceCount : int= 2;
	
    protected var m_AudioSource : AudioSource[]= null;
	
    var m_Audio_Button1 : AudioClip= null;
    var m_Audio_Button2 : AudioClip= null;

	// ########################################
	// MonoBehaviour Functions
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.html
	// ########################################
	
	
	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
   function Start () {

       // Create AudioSource list
       if(m_AudioSource==null)
       {
           m_AudioSource = new AudioSource[m_AudioSourceCount];
			
           for( var i : int=0;i<m_AudioSource.Length;i++)
           {
                var pAudioSource : AudioSource=  this.gameObject.AddComponent(AudioSource);
               pAudioSource.rolloffMode = AudioRolloffMode.Linear;
               m_AudioSource[i] = pAudioSource;
           }
   }
    }
	
	// Update is called every frame, if the MonoBehaviour is enabled.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
    function Update () {
		
    }

	// ########################################
	// Play sound Functions
	// ########################################
	
    // Play AudioClip
    function PlayOneShot ( pAudioClip : AudioClip  ) {

        for( var i : int=0;i<m_AudioSource.Length;i++)
        {
			if(m_AudioSource[i].isPlaying == false)
        {
				m_AudioSource[i].PlayOneShot(pAudioClip);
				break;
        }
}
}

// Play m_Audio_Button1 audio clip
function PlaySoundButton1 () {
    PlayOneShot(m_Audio_Button1);
}
	
// Play m_Audio_Button2 audio clip
function PlaySoundButton2 () {
    PlayOneShot(m_Audio_Button2);
}
}

