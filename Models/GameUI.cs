namespace Models;

public class GameUI
{
    public Game game { get; set; } = new Game();
    public List<Team> teams { get; set; } = new List<Team>();
    public List<Subcategory> subcategories { get; set; } = new List<Subcategory>();
    public List<QA> questions { get; set; } = new List<QA>();
    public List<Boardstate> boardstate { get; set; } = new List<Boardstate>();
}