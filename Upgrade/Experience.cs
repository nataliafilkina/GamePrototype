using System;
using UnityEngine;

namespace Upgrade
{
    public class Experience
    {
        public delegate void OnChangedExperience(int oldValue, int newValue);
        public event OnChangedExperience OnChanged;
        public Action<LevelSO> OnLevelUp;

        public int CurrentExperience { get; private set; }
        public int Points { get; private set; }
        public LevelSO CurrentLevel { get; private set; }

        public int ExperienceToNextLevel { get; private set; }

        public Experience()
        {
            //Load()
            /**/
            CurrentExperience = 147;
            Points = 150;
            CurrentLevel =  Resources.Load<LevelSO>("ScriptableObjects/Levels/Level1");
            ExperienceToNextLevel = CurrentLevel.NextLevel ? CurrentLevel.NextLevel.NeedExperience : int.MaxValue;
            /**/
        }

        public void AddExperience(int experience)
        {
            var oldValue = CurrentExperience;
            CurrentExperience += experience;

            if(CurrentExperience >= ExperienceToNextLevel)
            {
                SetNextLevel();

                if (ExperienceToNextLevel != int.MaxValue)
                    CurrentExperience = 0;
            }

            OnChanged?.Invoke(oldValue, CurrentExperience);
        }

        public void AddPoints(int points)
        {
            if(points > 0)
                Points += points;
        }

        private void SetNextLevel()
        {
            if (ExperienceToNextLevel == int.MaxValue)
                return;

            CurrentLevel = CurrentLevel.NextLevel;
            AddPoints(CurrentLevel.PointsGivesLevel);
            ExperienceToNextLevel = CurrentLevel.NextLevel ? CurrentLevel.NextLevel.NeedExperience : int.MaxValue;
            OnLevelUp?.Invoke(CurrentLevel);
        }
    }
}
