using AuthMvcApp.Interfaces.Repositories;
using AuthMvcApp.Interfaces.Services;
using AuthMvcApp.Models;

namespace AuthMvcApp.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;

    public TaskService(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<TaskItem>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task AddAsync(TaskItem task)
    {
        await _repository.AddAsync(task);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task UpdateAsync(TaskItem task)
    {
        await _repository.UpdateAsync(task);
    }

}