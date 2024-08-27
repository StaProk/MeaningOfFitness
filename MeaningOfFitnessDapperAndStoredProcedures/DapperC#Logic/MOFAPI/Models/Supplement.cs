namespace MOFAPI.Models
{
    public partial class Supplement
    {
        public int SupplementId {get; set;}
        public int UserId {get; set;}
        public string SupplementTitle {get; set;}
        public bool Shared {get; set;}
        public DateTime SupplementCreated {get; set;}
        public DateTime SupplementUpdated {get; set;}
        
        public Supplement()
        {
            if (SupplementTitle == null)
            {
                SupplementTitle = "";
            }
        }
    }
}