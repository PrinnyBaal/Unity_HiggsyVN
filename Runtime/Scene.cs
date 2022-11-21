using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HiggsyVN
{
    [CreateAssetMenu(fileName = "NewScene", menuName = "ScriptableObjects/SocialDialogueScene", order = 1)]
    public class Scene : ScriptableObject
    {
        public Line[] lines;
        public int activeLine = 0;
        public Scene nextScene;
        public bool phased;

        //start Scene method
        public Line StartScene()
        {
            activeLine = 0;
            return lines[activeLine];
        }

        public Line AdvanceScene()
        {
            activeLine++;
            if (activeLine < lines.Length)
            {
                return lines[activeLine];
            }
            else
            {
                //we've reached the end of the scene, start the next scene chained to this one
                return null;
            }

        }

        public void EndScene()
        {

        }
        //effects to be played at scene start
        // Start is called before the first frame update


        [System.Serializable]
        public class Line
        {

            [Header("Speaker and Dialogue")]
            public string speaker;
            //public Speakers activeSpeakers;
            //public Speakers highlightedSpeakers;

            //[Header("Dialogue")]
            [TextArea] public string text;
            [Header("Sprites")]
            public SpeakerSpritePair[] spriteChanges;
            public CutsceneManager.Background background;
            //bool endOfDay;




        }



        [System.Serializable]
        public struct SpeakerSpritePair
        {
            public CutsceneManager.Speakers speaker;
            public Sprite sprite;
        }

        //[System.Serializable]
        //public enum Characters
        //{
        //    Blake,
        //    Dad,
        //    Higgs,
        //    Marco
        //}


    }
}
