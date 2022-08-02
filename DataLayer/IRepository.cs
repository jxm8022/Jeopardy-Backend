﻿using Models;

namespace DataLayer;
public interface IRepository
{
    // Question
    Task<List<List<QA>>> GetQuestionsAsync(List<int> subcategories);
    Task<List<Question>> GetAllQuestionsAsync(int subcategory);
    Task<int> CreateQuestionAsync(Question question);
    Task CreateAnswerAsync(Answer answer);

    // Player
    Task<Admin> GetAdminAsync(string username, string password);
    Task<List<Player>> GetTeamMembersAsync(int team_id);
    Task CreatePlayersAsync(List<List<Player>> players);

    // Team
    Task<List<Team>> GetTeamsSortedbyScoreAsync();
    Task UpdateTeamsAsync(List<Team> teams);
    Task CreateTeamsAsync(List<Team> teams);

    // Category
    Task<List<Models.Type>> GetCategoriesAsync();
    Task CreateCategoryAsync(string categoryName);
    Task CreateSubcategoryAsync(Subcategory subcategory);

    // Game
    Task<int> CreateGameAsync(Game game);
    Task CreateGamestateAsync(List<Gamestate> gamestates);
    Task CreateBoardstateAsync(List<Boardstate> boardstates);
    Task<List<GameUI>> GetSavedGamesAsync();
}
