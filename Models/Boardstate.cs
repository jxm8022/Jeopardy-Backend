namespace Models;

public class Boardstate
{
    public int boardstate_id { get; set; } = -1;
    public int x_position { get; set; } = -1;
    public int y_position { get; set; } = -1;
    public bool answered { get; set; } = false;
    public int question_id { get; set; } = -1;
    public int game_id { get; set; } = -1;
}