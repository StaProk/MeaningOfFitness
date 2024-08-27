namespace MOFAPI.Models
{
    public partial class PersonalRecordKilos
    {
        public int PersonalRecordKilosId {get; set;}
        public int UserId {get; set;}
        public string ExerciseTitle {get; set;}
        public int WeightInKilos {get; set;}
        public int NumberOfRepetitions {get; set;}
        public bool Shared {get; set;}
        public DateTime PersonalRecordKilosCreated {get; set;}
        public DateTime PersonalRecordKilosUpdated {get; set;}
        public PersonalRecordKilos()
        {
            if (ExerciseTitle == null)
            {
                ExerciseTitle = "";
            }
        }
    }
}