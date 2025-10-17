using System.Collections.Generic;
using Game.CodeBase;

namespace YG
{
    public partial class SavesYG
    {
        // Ваши данные для сохранения
        public List<SpawnObjectData> SpawnObjectsData;
        public int CurrentScore;
        public int HighScore;
        public bool SoundActive = true;
        public bool IsShowReview = false;
    }
}