using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LD39.Config
{
    public class WritingTexts
    {
        public List<string> texts;

        public WritingTexts()
        {
            texts = new List<string>();
        }

        public void LoadConfig(string filename)
        {
            string resourcePath = Path.Combine("Config", filename);
            TextAsset t = Resources.Load<TextAsset>(resourcePath);
            var textsObj = JToken.Parse(t.text);
            foreach (var textObj in textsObj["texts"].Children())
            {
                texts.Add(textObj.Value<string>());
            }
        }
    }
}