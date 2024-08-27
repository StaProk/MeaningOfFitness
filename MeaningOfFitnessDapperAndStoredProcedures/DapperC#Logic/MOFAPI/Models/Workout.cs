namespace MOFAPI.Models
{
    public partial class Workout
    {
        public int WorkoutId {get; set;}
        public int UserId {get; set;}
        public string WorkoutTitle {get; set;}
        public string WorkoutContent {get; set;}
        public bool Shared {get; set;}
        public DateTime WorkoutCreated {get; set;}
        public DateTime WorkoutUpdated {get; set;}
        

        public Workout()
        {
            if (WorkoutTitle == null)
            {
                WorkoutTitle = "";
            }
            if (WorkoutContent == null)
            {
                WorkoutContent = "";
            }
        }
    }
}