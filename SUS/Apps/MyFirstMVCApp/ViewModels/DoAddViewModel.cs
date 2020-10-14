namespace MyFirstMVCApp.ViewModels
{
    public class DoAddViewModel
    {
        public int Attack { get; set; }

        public int Health { get; set; }

        public int Damage => this.Attack * 10 + this.Health; //calculate property - has to be specified in a ViewModel!
    }
}
