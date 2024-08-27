namespace MOFAPI.Models
{
    public partial class Meal
    {
        public int MealId {get; set;}
        public int UserId {get; set;}
        public string MealTitle {get; set;}
        public string MealContent {get; set;}
        public int GramsOfCarbohydrates {get; set;}
        public int GramsOfProtein {get; set;}
        public int GramsOfFats {get; set;}
        public int GramsOfFibers {get; set;}
        public int TotalCalories {get; set;} 
        public bool Shared {get; set;}
        public DateTime MealCreated {get; set;}
        public DateTime MealUpdated {get; set;}

        public Meal()
        {
            if (MealTitle == null)
            {
                MealTitle = "";
            }
            if (MealContent == null)
            {
                MealContent = "";
            }
        }
    }
}