using Player;

namespace Upgrade
{
    public abstract class Improvement
    {
        public ForceImprovementSO Info;

        public ImprovementState State { get; private set; }
        public bool IsAvailable => 
            _experience.Points >= Info.Cost;    

        private Experience _experience;
        protected Forces _forceStats;

        public Improvement(ForceImprovementSO improvementsInfo, Forces forceStats, Experience experience)
        {
            Info = improvementsInfo;
            _experience = experience;
            _forceStats = forceStats;

            //_currentImprovement = ImprovementsInfo[State.Level];

            //if (State.IsBought)
            //    Apply();
        }

        public virtual void Apply()
        {
        }
    }
}
