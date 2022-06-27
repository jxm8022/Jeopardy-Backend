﻿using Models;

namespace BusinessLayer;
public interface IBusiness
{
    Task<List<QA>> GetQuestionsAsync(int category);
    Task CreateTeamsAsync(List<Team> teams);
    Task UpdateTeamAsync(Team team);
    Task CreatePlayersAsync(List<List<Player>> players);
    Task<List<Team>> GetTeamsSortedbyScoreAsync();
    Task<List<Player>> GetTeamMembersAsync(int team_id);
    Task<List<Category>> GetCategoriesAsync();
    Task<List<Subcategory>> GetSubcategoriesAsync(int category_id);
    Task CreateCategoryAsync(string categoryName);
    Task CreateSubcategoryAsync(Subcategory subcategory);
}
