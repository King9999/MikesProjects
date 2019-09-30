using System.Collections.Generic;

using System.IO;
using System.IO.IsolatedStorage;

using System.Xml;
using System.Xml.Serialization;

namespace TileCrusher.Scoring
{
    class Scores
    {
        private static SerializableDictionary<string, ScoreList> highScores = new SerializableDictionary<string, ScoreList>();

        public void AddScore(string type, string name, int value)
        {
            if (!highScores.ContainsKey(type))
            {
                highScores.Add(type, new ScoreList());
            }

            highScores[type].AddScore(new Score(name, value));
        }

        public List<Score> HighScores(string type)
        {
            return highScores[type].Scores;
        }

        public int CurrentHighScore(string type)
        {
            if (highScores.ContainsKey(type))
            {
                return highScores[type].Scores[0].Value;
            }
            return 0;
        }

        public void Load()
        {
            using (IsolatedStorageFile storage
                = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!storage.FileExists("HighScores.xml"))
                {
                    return;
                }

                using (IsolatedStorageFileStream file
                    = storage.OpenFile("HighScores.xml", FileMode.Open))
                {
                    using (XmlReader reader = XmlReader.Create(file))
                    {
                        highScores.ReadXml(reader);
                    }
                }
            }
        }

        public void Save()
        {
            using (IsolatedStorageFile storage
                = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream file
                    = storage.CreateFile("HighScores.xml"))
                {
                    using (XmlWriter writer = XmlWriter.Create(file))
                    {
                        highScores.WriteXml(writer);
                    }
                }
            }
        }
    }
}
