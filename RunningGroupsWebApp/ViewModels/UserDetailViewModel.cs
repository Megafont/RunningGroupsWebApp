namespace RunningGroupsWebApp.ViewModels
{
    public class UserDetailViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public int? Pace { get; set; } // How long it takes the runner to run a mile
        public int? Mileage { get; set; } // How much you've run in a week 
    }
}
