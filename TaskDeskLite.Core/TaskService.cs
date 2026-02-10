namespace TaskDeskLite.Core;

public class TaskService : ITaskService
{
    // Persistência em memória
    private readonly List<TaskItem> _tasks = new();

    public IReadOnlyList<TaskItem> GetAll()
        => _tasks.OrderByDescending(t => t.CreatedAt).ToList();

    public TaskItem GetById(Guid id)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task is null) throw new NotFoundException("Tarefa não encontrada.");
        return task;
    }

    public TaskItem Create(TaskItem task)
    {
        if (string.IsNullOrWhiteSpace(task.Title)) // IsNullOrWhiteSpace verifica se é null, vazio ou só espaços em branco.
            throw new DomainValidationException("O título é obrigatório.");
        if (task is null)
            throw new DomainValidationException ("A tarefa é obrigatória."); // o domain serve para validar regras de negócio.

        task.Id = Guid.NewGuid(); 
        task.Title = task.Title;
        task.Description = task.Description;
        task.Status = TaskStatus.Pending;

        _tasks.Add(task);

        return task;

        // TODO: validar
        // TODO: garantir Id novo e Status Pending
        // TODO: adicionar na lista
        // TODO: retornar a tarefa criada

    }

    public TaskItem Update(TaskItem task)
    {
        TaskValidator.ValidateForCreateOrUpdate(task);// Validate serve para validar regras de negócio, como por exemplo, se o título é obrigatório ou se tem palavras proibidas.
        var taskExistente = GetById(task.Id);

        if (taskExistente.Status == TaskStatus.Done) // se a tarefa já estiver concluída, não pode ser editada.
            throw new BusinessRuleException("Tarefa concluidas não podem ser editadas. "); // business serve para validar regras de negócio.

        taskExistente.Title = task.Title;
        taskExistente.Status = task.Status;
        taskExistente.DueDate = task.DueDate;

        return taskExistente;

        // TODO: validar
        // TODO: buscar existente
        // TODO: regra: se Status Done -> não pode editar (BusinessRuleException)
        // TODO: atualizar campos permitidos
        // TODO: retornar atualizado

    }

    public void Delete(Guid id)
    {
        var task = GetById(id);
        _tasks.Remove(task);

        // TODO: se não existir -> NotFoundException
        // TODO: remover
        
    }

    public TaskItem MarkAsDone(Guid id)
    {
        var taskExistente = GetById(id);
        if (taskExistente.Status == TaskStatus.Done)
            throw new BusinessRuleException("Tarefa já está concluída."); 

        taskExistente.Status = TaskStatus.Done; 

        return taskExistente;

        // TODO: buscar existente
        // TODO: marcar Done
        // TODO: retornar
       
    }
}
