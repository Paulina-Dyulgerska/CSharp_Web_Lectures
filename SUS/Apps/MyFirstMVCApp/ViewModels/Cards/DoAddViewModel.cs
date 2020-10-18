namespace MyFirstMVCApp.ViewModels.Cards
{
    public class DoAddViewModel
    {
        public string Name { get; set; }  
       
        public string ImageUrl { get; set; }
        
        public string Keyword { get; set; }
        
        public int Attack { get; set; }

        public int Health { get; set; }

        public string Description { get; set; }
     
        public int Damage => this.Attack * 10 + this.Health; //calculate property - has to be specified in a ViewModel!
    }
}
