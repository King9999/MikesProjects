using System.Collections.Generic;

namespace TileCrusher.Scoring
{
    public class ScoreList
    {
        List<Score> scores = new List<Score>();
        int maxScoredStored = 10;

        public ScoreList()
        {
        }

        public void AddScore(Score newScore)
        {
            scores.Add(newScore);

            if (scores.Count <= maxScoredStored)
            {
                return;
            }
            else
            {
                scores.Sort((scoreOne, scoreTwo)
                    => (scoreTwo.Value.CompareTo(scoreOne.Value)));
                scores.RemoveRange(maxScoredStored,
                                   scores.Count - maxScoredStored);
            }
        }

        public List<Score> Scores
        {
            get
            {
                scores.Sort((scoreOne, scoreTwo)
                    => (scoreTwo.Value.CompareTo(scoreOne.Value)));
                return scores;
            }
        }
    }
}
