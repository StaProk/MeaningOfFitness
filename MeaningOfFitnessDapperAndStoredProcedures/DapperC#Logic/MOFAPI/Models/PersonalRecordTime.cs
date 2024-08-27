namespace MOFAPI.Models
{
    public partial class PersonalRecordTime
    {
        public int PersonalRecordTimeId {get; set;}
        public int UserId {get; set;}
        public string ExerciseTitle {get; set;}
        public int TimeInSeconds {get; set;}
        public bool Shared {get; set;}
        public DateTime PersonalRecordTimeCreated {get; set;}
        public DateTime PersonalRecordTimeUpdated {get; set;}
        
        public PersonalRecordTime()
        {
            if (ExerciseTitle == null)
            {
                ExerciseTitle = "";
            }
        }
    }
}