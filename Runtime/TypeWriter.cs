using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using GameMasters;
using System;
using System.Text.RegularExpressions;

namespace HiggsyVN
{
    public class TypeWriter : MonoBehaviour
    {
        //Displays text as though written by a typewriter, one character at a time.
        // Start is called before the first frame update
        public bool isTyping;
        //Monomi.textContainer targetBox;
        public TextMeshProUGUI targetBox;
        string targetText;
        string fullText;
        float idleTimeAllowed;
        float timeIdle;
        int lettersTyped;
        List<markupData> markup = new List<markupData>();




        public event EventHandler OnTypingDone;

        void Start()
        {

        }

        public void BeginTyping(string text, float speed)
        {
            //textbox to type into
            //targetBox = dialogue;
            //speed in letters per second
            idleTimeAllowed = 1f / speed;
            //string of text to put into dialogue container
            fullText = text;
            setMarkup(text);

            //to immediately start with a typing event
            timeIdle = idleTimeAllowed;

            //resetting values
            lettersTyped = 0;
            isTyping = true;


            void setMarkup(string markedText)
            {
                Regex rx = new Regex("<.+?>");
                markup.Clear();
                foreach (Match match in rx.Matches(markedText))
                {
                    markupData data = new markupData();

                    int i = match.Index;
                    data.index = match.Index;
                    data.text = match.Value;
                    markup.Add(data);
                }

                targetText = rx.Replace(markedText, "");

            }
        }



        public void skipTypewrite()
        {
            if (isTyping)
            {
                isTyping = false;
                targetBox.text = fullText;

                OnTypingDone?.Invoke(this, EventArgs.Empty);

            }
        }

        // Update is called once per frame
        void Update()
        {
            if (isTyping)
            {
                timeIdle += Time.deltaTime;

                if (timeIdle >= idleTimeAllowed)
                {
                    string subString = targetText.Substring(0, lettersTyped);
                    int marksAdded = 0;
                    //type another letter
                    timeIdle = 0;

                    //targetBox.textMesh.text = targetText.Substring(0, lettersTyped);
                    lettersTyped++;

                    foreach (markupData mark in markup)
                    {
                        if (mark.index < subString.Length)
                        {
                            subString = subString.Insert(mark.index, mark.text);
                            marksAdded++;
                        }
                    }

                    if (marksAdded % 2 != 0 && markup.Count < marksAdded)
                    {
                        subString += markup[marksAdded].text;
                    }

                    //targetBox.textMesh.text = subString;
                    targetBox.text = subString;


                    //check if we're done typing
                    if (lettersTyped > targetText.Length)
                    {
                        isTyping = false;
                        OnTypingDone?.Invoke(this, EventArgs.Empty);

                    }
                }
            }
        }

        [System.Serializable]
        public struct markupData
        {
            public int index;
            public string text;
        }
    }
}
