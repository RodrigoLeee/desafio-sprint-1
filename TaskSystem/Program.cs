using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagementSystem
{
    class Program
    {
        private static TaskFactory taskFactory = new TaskFactory();
        private static List<User> users = new List<User>();
        private static List<Project> projects = new List<Project>();
        private static User? currentUser = null;

        static void Main(string[] args)
        {
            Console.WriteLine("SISTEMA DE GERENCIAMENTO DE TAREFAS");
            Console.WriteLine("");
            
            // Criar alguns usuários e projetos iniciais para teste
            InitializeData();

            // Menu principal
            bool exit = false;
            while (!exit)
            {
                if (currentUser == null)
                {
                    LoginMenu();
                }
                else
                {
                    exit = MainMenu();
                }
            }
        }

        private static void InitializeData()
        {
            // Criar usuários de exemplo
            users.Add(new User(1, "Usuário 1"));
            users.Add(new User(2, "Usuário 2"));
            users.Add(new User(3, "Usuário 3"));
            
            // Criar projeto de exemplo
            var projeto1 = new Project(1, "Sistema de Vendas Online");
            var projeto2 = new Project(2, "Aplicativo Mobile");
            
            projects.Add(projeto1);
            projects.Add(projeto2);
            
            // Adicionar algumas tarefas de exemplo ao projeto 1
            var tarefa1 = taskFactory.CreateTask(TaskType.Simple, "Definir requisitos", "Levantar os requisitos iniciais do sistema", Priority.High);
            tarefa1.AssignTo(users[0]);
            projeto1.AddTask(tarefa1);
            
            var tarefa2 = taskFactory.CreateTask(TaskType.WithDeadline, "Criar protótipos", "Desenvolver protótipos de tela", Priority.Medium) as DeadlineTask;
            if (tarefa2 != null)
            {
                tarefa2.SetDeadline(DateTime.Now.AddDays(7));
                tarefa2.AssignTo(users[1]);
                projeto1.AddTask(tarefa2);
            }
        }

        private static void LoginMenu()
        {
            Console.Clear();
            Console.WriteLine("LOGIN");
            Console.WriteLine("Selecione um usuário:");
            
            for (int i = 0; i < users.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {users[i].Name}");
            }
            
            Console.WriteLine("0. Sair");
            
            Console.Write("\nOpção: ");
            if (int.TryParse(Console.ReadLine(), out int option))
            {
                if (option == 0)
                {
                    Environment.Exit(0);
                }
                else if (option > 0 && option <= users.Count)
                {
                    currentUser = users[option - 1];
                    Console.WriteLine($"Bem-vindo(a), {currentUser.Name}!");
                    Console.WriteLine("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        private static bool MainMenu()
        {
            Console.Clear();
            Console.WriteLine($"MENU PRINCIPAL - Usuário: {currentUser?.Name}");
            Console.WriteLine("1. Gerenciar Projetos");
            Console.WriteLine("2. Gerenciar Tarefas");
            Console.WriteLine("3. Ver Minhas Notificações");
            Console.WriteLine("4. Logout");
            Console.WriteLine("0. Sair");
            
            Console.Write("\nOpção: ");
            if (int.TryParse(Console.ReadLine(), out int option))
            {
                switch (option)
                {
                    case 1:
                        ProjectMenu();
                        return false;
                    case 2:
                        TaskMenu();
                        return false;
                    case 3:
                        ViewNotifications();
                        return false;
                    case 4:
                        currentUser = null;
                        return false;
                    case 0:
                        return true;
                    default:
                        Console.WriteLine("Opção inválida!");
                        Console.WriteLine("Pressione qualquer tecla para continuar...");
                        Console.ReadKey();
                        return false;
                }
            }
            return false;
        }

        private static void ProjectMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("GERENCIAMENTO DE PROJETOS");
                Console.WriteLine("1. Listar Projetos");
                Console.WriteLine("2. Criar Novo Projeto");
                Console.WriteLine("3. Visualizar Projeto");
                Console.WriteLine("0. Voltar");
                
                Console.Write("\nOpção: ");
                if (int.TryParse(Console.ReadLine(), out int option))
                {
                    switch (option)
                    {
                        case 1:
                            ListProjects();
                            break;
                        case 2:
                            CreateProject();
                            break;
                        case 3:
                            ViewProject();
                            break;
                        case 0:
                            back = true;
                            break;
                        default:
                            Console.WriteLine("Opção inválida!");
                            Console.WriteLine("Pressione qualquer tecla para continuar...");
                            Console.ReadKey();
                            break;
                    }
                }
            }
        }

        private static void ListProjects()
        {
            Console.Clear();
            Console.WriteLine("LISTA DE PROJETOS");
            
            if (projects.Count == 0)
            {
                Console.WriteLine("Nenhum projeto encontrado.");
            }
            else
            {
                for (int i = 0; i < projects.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {projects[i].Name} (ID: {projects[i].Id}) - {projects[i].GetTasks().Count} tarefas");
                }
            }
            
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }

        private static void CreateProject()
        {
            Console.Clear();
            Console.WriteLine("CRIAR NOVO PROJETO");
            
            Console.Write("Nome do projeto: ");
            string? name = Console.ReadLine();
            
            if (!string.IsNullOrWhiteSpace(name))
            {
                int newId = projects.Count > 0 ? projects.Max(p => p.Id) + 1 : 1;
                var project = new Project(newId, name);
                projects.Add(project);
                
                Console.WriteLine($"Projeto '{name}' criado com sucesso!");
            }
            else
            {
                Console.WriteLine("Nome de projeto inválido!");
            }
            
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }

        private static void ViewProject()
        {
            Console.Clear();
            Console.WriteLine("VISUALIZAR PROJETO");
            
            if (projects.Count == 0)
            {
                Console.WriteLine("Nenhum projeto encontrado.");
                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
                return;
            }
            
            for (int i = 0; i < projects.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {projects[i].Name}");
            }
            
            Console.Write("\nSelecione um projeto (0 para voltar): ");
            if (int.TryParse(Console.ReadLine(), out int option))
            {
                if (option == 0)
                {
                    return;
                }
                else if (option > 0 && option <= projects.Count)
                {
                    Project selectedProject = projects[option - 1];
                    ProjectDetailsMenu(selectedProject);
                }
                else
                {
                    Console.WriteLine("Opção inválida!");
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        private static void ProjectDetailsMenu(Project project)
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine($"PROJETO: {project.Name}");
                Console.WriteLine("1. Listar Todas as Tarefas");
                Console.WriteLine("2. Listar Tarefas por Prioridade");
                Console.WriteLine("3. Listar Tarefas por Status");
                Console.WriteLine("4. Adicionar Nova Tarefa");
                Console.WriteLine("0. Voltar");
                
                Console.Write("\nOpção: ");
                if (int.TryParse(Console.ReadLine(), out int option))
                {
                    switch (option)
                    {
                        case 1:
                            ListProjectTasks(project);
                            break;
                        case 2:
                            ListTasksByPriority(project);
                            break;
                        case 3:
                            ListTasksByStatus(project);
                            break;
                        case 4:
                            AddTaskToProject(project);
                            break;
                        case 0:
                            back = true;
                            break;
                        default:
                            Console.WriteLine("Opção inválida!");
                            Console.WriteLine("Pressione qualquer tecla para continuar...");
                            Console.ReadKey();
                            break;
                    }
                }
            }
        }

        private static void ListProjectTasks(Project project)
        {
            Console.Clear();
            Console.WriteLine($"TAREFAS DO PROJETO: {project.Name}");
            
            var tasks = project.GetTasks();
            if (tasks.Count == 0)
            {
                Console.WriteLine("Este projeto não possui tarefas.");
            }
            else
            {
                for (int i = 0; i < tasks.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {tasks[i]}");
                }
                
                Console.WriteLine("\nSelecione uma tarefa para ver detalhes (0 para voltar): ");
                if (int.TryParse(Console.ReadLine(), out int option))
                {
                    if (option > 0 && option <= tasks.Count)
                    {
                        ViewTaskDetails(tasks[option - 1], project);
                    }
                }
            }
            
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }

        private static void ListTasksByPriority(Project project)
        {
            Console.Clear();
            Console.WriteLine($"TAREFAS POR PRIORIDADE - PROJETO: {project.Name}");
            Console.WriteLine("1. Alta Prioridade");
            Console.WriteLine("2. Média Prioridade");
            Console.WriteLine("3. Baixa Prioridade");
            Console.WriteLine("0. Voltar");
            
            Console.Write("\nOpção: ");
            if (int.TryParse(Console.ReadLine(), out int option))
            {
                if (option >= 1 && option <= 3)
                {
                    Priority selectedPriority = (Priority)(option - 1);
                    
                    Console.Clear();
                    Console.WriteLine($"TAREFAS COM PRIORIDADE {selectedPriority} - PROJETO: {project.Name}");
                    
                    var tasks = project.GetTasksByPriority(selectedPriority);
                    if (tasks.Count == 0)
                    {
                        Console.WriteLine($"Nenhuma tarefa com prioridade {selectedPriority} encontrada.");
                    }
                    else
                    {
                        for (int i = 0; i < tasks.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {tasks[i]}");
                        }
                        
                        Console.WriteLine("\nSelecione uma tarefa para ver detalhes (0 para voltar): ");
                        if (int.TryParse(Console.ReadLine(), out int taskOption))
                        {
                            if (taskOption > 0 && taskOption <= tasks.Count)
                            {
                                ViewTaskDetails(tasks[taskOption - 1], project);
                            }
                        }
                    }
                }
            }
            
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }

        private static void ListTasksByStatus(Project project)
        {
            Console.Clear();
            Console.WriteLine($"TAREFAS POR STATUS - PROJETO: {project.Name}");
            Console.WriteLine("1. Pendentes");
            Console.WriteLine("2. Em Andamento");
            Console.WriteLine("3. Concluídas");
            Console.WriteLine("0. Voltar");
            
            Console.Write("\nOpção: ");
            if (int.TryParse(Console.ReadLine(), out int option))
            {
                if (option >= 1 && option <= 3)
                {
                    TaskStatus selectedStatus = (TaskStatus)(option - 1);
                    
                    Console.Clear();
                    Console.WriteLine($"TAREFAS COM STATUS {selectedStatus} - PROJETO: {project.Name}");
                    
                    var tasks = project.GetTasksByStatus(selectedStatus);
                    if (tasks.Count == 0)
                    {
                        Console.WriteLine($"Nenhuma tarefa com status {selectedStatus} encontrada.");
                    }
                    else
                    {
                        for (int i = 0; i < tasks.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {tasks[i]}");
                        }
                        
                        Console.WriteLine("\nSelecione uma tarefa para ver detalhes (0 para voltar): ");
                        if (int.TryParse(Console.ReadLine(), out int taskOption))
                        {
                            if (taskOption > 0 && taskOption <= tasks.Count)
                            {
                                ViewTaskDetails(tasks[taskOption - 1], project);
                            }
                        }
                    }
                }
            }
            
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }

        private static void AddTaskToProject(Project project)
        {
            Console.Clear();
            Console.WriteLine($"ADICIONAR TAREFA AO PROJETO: {project.Name}");
            Console.WriteLine("Tipo de Tarefa:");
            Console.WriteLine("1. Tarefa Simples");
            Console.WriteLine("2. Tarefa com Prazo");
            Console.WriteLine("3. Tarefa Recorrente");
            Console.WriteLine("4. Tarefa com Anexos");
            Console.WriteLine("0. Cancelar");
            
            Console.Write("\nOpção: ");
            if (int.TryParse(Console.ReadLine(), out int option))
            {
                if (option >= 1 && option <= 4)
                {
                    TaskType selectedType = (TaskType)(option - 1);
                    
                    Console.Write("Título da tarefa: ");
                    string? title = Console.ReadLine();
                    
                    Console.Write("Descrição: ");
                    string? description = Console.ReadLine();
                    
                    Console.WriteLine("Prioridade:");
                    Console.WriteLine("1. Baixa");
                    Console.WriteLine("2. Média");
                    Console.WriteLine("3. Alta");
                    
                    Priority priority = Priority.Medium;
                    Console.Write("\nOpção: ");
                    if (int.TryParse(Console.ReadLine(), out int priorityOption) && priorityOption >= 1 && priorityOption <= 3)
                    {
                        priority = (Priority)(priorityOption - 1);
                    }

                    if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(description))
                    {
                        var task = taskFactory.CreateTask(selectedType, title, description, priority);
                        
                        // Configurações específicas para cada tipo de tarefa
                        switch (selectedType)
                        {
                            case TaskType.WithDeadline:
                                if (task is DeadlineTask deadlineTask)
                                {
                                    Console.Write("Data de prazo (YYYY-MM-DD): ");
                                    if (DateTime.TryParse(Console.ReadLine(), out DateTime deadline))
                                    {
                                        deadlineTask.SetDeadline(deadline);
                                    }
                                }
                                break;
                            case TaskType.Recurring:
                                if (task is RecurringTask recurringTask)
                                {
                                    Console.WriteLine("Padrão de recorrência:");
                                    Console.WriteLine("1. Diário");
                                    Console.WriteLine("2. Semanal");
                                    Console.WriteLine("3. Mensal");
                                    
                                    RecurrencePattern pattern = RecurrencePattern.Weekly;
                                    Console.Write("\nOpção: ");
                                    if (int.TryParse(Console.ReadLine(), out int patternOption) && patternOption >= 1 && patternOption <= 3)
                                    {
                                        pattern = (RecurrencePattern)(patternOption - 1);
                                    }
                                    
                                    Console.Write("Intervalo (1-30): ");
                                    if (int.TryParse(Console.ReadLine(), out int interval) && interval >= 1 && interval <= 30)
                                    {
                                        recurringTask.SetRecurrencePattern(pattern, interval);
                                    }
                                }
                                break;
                            case TaskType.WithAttachments:
                                if (task is AttachmentTask attachmentTask)
                                {
                                    bool addMoreAttachments = true;
                                    while (addMoreAttachments)
                                    {
                                        Console.Write("Nome do anexo: ");
                                        string? attachmentName = Console.ReadLine();
                                        
                                        Console.Write("Caminho do arquivo: ");
                                        string? attachmentPath = Console.ReadLine();
                                        
                                        if (!string.IsNullOrWhiteSpace(attachmentName) && !string.IsNullOrWhiteSpace(attachmentPath))
                                        {
                                            attachmentTask.AddAttachment(attachmentName, attachmentPath);
                                            
                                            Console.Write("Adicionar outro anexo? (S/N): ");
                                            string? response = Console.ReadLine();
                                            addMoreAttachments = response?.Trim().ToUpper() == "S";
                                        }
                                        else
                                        {
                                            addMoreAttachments = false;
                                        }
                                    }
                                }
                                break;
                        }
                        
                        // Atribuir responsável
                        Console.WriteLine("\nAtribuir tarefa a um usuário:");
                        for (int i = 0; i < users.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {users[i].Name}");
                        }
                        
                        Console.Write("\nOpção (0 para nenhum): ");
                        if (int.TryParse(Console.ReadLine(), out int userOption))
                        {
                            if (userOption > 0 && userOption <= users.Count)
                            {
                                task.AssignTo(users[userOption - 1]);
                                
                                // Registrar o usuário atual como observador
                                if (currentUser != null)
                                {
                                    task.RegisterObserver(currentUser);
                                }
                                
                                // Registrar o responsável como observador
                                if (task.AssignedTo != null)
                                {
                                    task.RegisterObserver(task.AssignedTo);
                                }
                            }
                        }
                        
                        project.AddTask(task);
                        Console.WriteLine("\nTarefa adicionada com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine("\nTítulo ou descrição inválidos!");
                    }
                }
            }
            
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }

        private static void ViewTaskDetails(Task task, Project project)
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("DETALHES DA TAREFA");
                Console.WriteLine($"ID: {task.Id}");
                Console.WriteLine($"Título: {task.Title}");
                Console.WriteLine($"Descrição: {task.Description}");
                Console.WriteLine($"Prioridade: {task.Priority}");
                Console.WriteLine($"Status: {task.Status}");
                Console.WriteLine($"Responsável: {(task.AssignedTo != null ? task.AssignedTo.Name : "Não atribuído")}");
                Console.WriteLine($"Criado em: {task.CreatedAt}");
                Console.WriteLine($"Atualizado em: {task.UpdatedAt}");
                
                // Informações específicas por tipo de tarefa
                if (task is DeadlineTask deadlineTask)
                {
                    Console.WriteLine($"Prazo: {deadlineTask.Deadline.ToShortDateString()}");
                }
                else if (task is RecurringTask recurringTask)
                {
                    Console.WriteLine($"Padrão de Recorrência: {recurringTask.Pattern} (a cada {recurringTask.Interval})");
                }
                else if (task is AttachmentTask attachmentTask)
                {
                    Console.WriteLine("\nAnexos:");
                    var attachments = attachmentTask.GetAttachments();
                    if (attachments.Count == 0)
                    {
                        Console.WriteLine("  Nenhum anexo");
                    }
                    else
                    {
                        foreach (var attachment in attachments)
                        {
                            Console.WriteLine($"  - {attachment.Name}: {attachment.Path}");
                        }
                    }
                }
                
                Console.WriteLine("\nOpções:");
                Console.WriteLine("1. Alterar Status");
                Console.WriteLine("2. Alterar Responsável");
                Console.WriteLine("3. Registrar-se como Observador");
                Console.WriteLine("0. Voltar");
                
                Console.Write("\nOpção: ");
                if (int.TryParse(Console.ReadLine(), out int option))
                {
                    switch (option)
                    {
                        case 1:
                            ChangeTaskStatus(task);
                            break;
                        case 2:
                            ChangeTaskAssignee(task);
                            break;
                        case 3:
                            if (currentUser != null)
                            {
                                task.RegisterObserver(currentUser);
                                Console.WriteLine($"{currentUser.Name} agora receberá notificações desta tarefa.");
                                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                                Console.ReadKey();
                            }
                            break;
                        case 0:
                            back = true;
                            break;
                        default:
                            Console.WriteLine("Opção inválida!");
                            Console.WriteLine("\nPressione qualquer tecla para continuar...");
                            Console.ReadKey();
                            break;
                    }
                }
            }
        }

        private static void ChangeTaskStatus(Task task)
        {
            Console.Clear();
            Console.WriteLine("ALTERAR STATUS DA TAREFA");
            Console.WriteLine($"Tarefa: {task.Title}");
            Console.WriteLine($"Status atual: {task.Status}");
            Console.WriteLine("\nSelecione o novo status:");
            Console.WriteLine("1. Pendente");
            Console.WriteLine("2. Em Andamento");
            Console.WriteLine("3. Concluída");
            Console.WriteLine("0. Cancelar");
            
            Console.Write("\nOpção: ");
            if (int.TryParse(Console.ReadLine(), out int option))
            {
                if (option >= 1 && option <= 3)
                {
                    TaskStatus newStatus = (TaskStatus)(option - 1);
                    task.UpdateStatus(newStatus);
                    Console.WriteLine($"Status alterado para {newStatus}");
                }
            }
            
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }

        private static void ChangeTaskAssignee(Task task)
        {
            Console.Clear();
            Console.WriteLine("ALTERAR RESPONSÁVEL DA TAREFA");
            Console.WriteLine($"Tarefa: {task.Title}");
            Console.WriteLine($"Responsável atual: {(task.AssignedTo != null ? task.AssignedTo.Name : "Não atribuído")}");
            Console.WriteLine("\nSelecione o novo responsável:");
            
            for (int i = 0; i < users.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {users[i].Name}");
            }
            
            Console.WriteLine("0. Cancelar");
            
            Console.Write("\nOpção: ");
            if (int.TryParse(Console.ReadLine(), out int option))
            {
                if (option > 0 && option <= users.Count)
                {
                    User newAssignee = users[option - 1];
                    task.AssignTo(newAssignee);
                    task.RegisterObserver(newAssignee);
                    Console.WriteLine($"Responsável alterado para {newAssignee.Name}");
                }
            }
            
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }

        private static void TaskMenu()
        {
            Console.Clear();
            Console.WriteLine("MINHAS TAREFAS");
            
            List<Task> myTasks = new List<Task>();
            foreach (var project in projects)
            {
                foreach (var task in project.GetTasks())
                {
                    if (task.AssignedTo == currentUser)
                    {
                        myTasks.Add(task);
                    }
                }
            }
            
            if (myTasks.Count == 0)
            {
                Console.WriteLine("Você não possui tarefas atribuídas.");
            }
            else
            {
                Console.WriteLine("Suas tarefas atribuídas:");
                for (int i = 0; i < myTasks.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {myTasks[i]}");
                }
                
                Console.WriteLine("\nSelecione uma tarefa para ver detalhes ou 0 para voltar: ");
                if (int.TryParse(Console.ReadLine(), out int option))
                {
                    if (option > 0 && option <= myTasks.Count)
                    {
                        // Encontrar o projeto ao qual a tarefa pertence
                        Project? taskProject = null;
                        foreach (var project in projects)
                        {
                            if (project.GetTasks().Contains(myTasks[option - 1]))
                            {
                                taskProject = project;
                                break;
                            }
                        }
                        
                        if (taskProject != null)
                        {
                            ViewTaskDetails(myTasks[option - 1], taskProject);
                            return;
                        }
                    }
                }
            }
            
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }

        private static void ViewNotifications()
        {
            if (currentUser == null)
            {
                return;
            }
            
            Console.Clear();
            Console.WriteLine("MINHAS NOTIFICAÇÕES");
            
            List<string> notifications = currentUser.GetNotifications();
            if (notifications.Count == 0)
            {
                Console.WriteLine("Você não tem notificações.");
            }
            else
            {
                for (int i = 0; i < notifications.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {notifications[i]}");
                }
            }
            
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }

    #region Models

    // Enums
    public enum Priority { Low, Medium, High }
    public enum TaskStatus { Pending, InProgress, Completed }
    public enum TaskType { Simple, WithDeadline, Recurring, WithAttachments }
    public enum RecurrencePattern { Daily, Weekly, Monthly }

    // Observer Interface
    public interface ITaskObserver
    {
        void Update(Task task, string message);
    }

    // Task Observable Interface
    public interface ITaskObservable
    {
        void RegisterObserver(ITaskObserver observer);
        void RemoveObserver(ITaskObserver observer);
        void NotifyObservers(string message);
    }

    // Abstract Task
    public abstract class Task : ITaskObservable
    {
        private List<ITaskObserver> _observers = new List<ITaskObserver>();
        
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public Priority Priority { get; private set; }
        public TaskStatus Status { get; private set; }
        public User? AssignedTo { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        protected Task(int id, string title, string description, Priority priority)
        {
            Id = id;
            Title = title;
            Description = description;
            Priority = priority;
            Status = TaskStatus.Pending;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public void AssignTo(User user)
        {
            AssignedTo = user;
            UpdatedAt = DateTime.Now;
            NotifyObservers($"Tarefa '{Title}' atribuída a {user.Name}");
        }

        public void UpdateStatus(TaskStatus newStatus)
        {
            var oldStatus = Status;
            Status = newStatus;
            UpdatedAt = DateTime.Now;
            NotifyObservers($"Status da tarefa '{Title}' alterado de {oldStatus} para {newStatus}");
        }

        public void RegisterObserver(ITaskObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void RemoveObserver(ITaskObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers(string message)
        {
            foreach (var observer in _observers)
            {
                observer.Update(this, message);
            }
        }

        public override string ToString()
        {
            return $"[{Status}] {Title} (Prioridade: {Priority})";
        }
    }

    // Task Factory
    public class TaskFactory
    {
        private static int _nextTaskId = 1;

        public Task CreateTask(TaskType type, string title, string description, Priority priority)
        {
            Task task;
            int taskId = _nextTaskId++;
            
            switch (type)
            {
                case TaskType.WithDeadline:
                    task = new DeadlineTask(taskId, title, description, priority);
                    break;
                case TaskType.Recurring:
                    task = new RecurringTask(taskId, title, description, priority);
                    break;
                case TaskType.WithAttachments:
                    task = new AttachmentTask(taskId, title, description, priority);
                    break;
                default:
                    task = new SimpleTask(taskId, title, description, priority);
                    break;
            }
            
            return task;
        }
    }

    // Concrete Task Types
    public class SimpleTask : Task
    {
        public SimpleTask(int id, string title, string description, Priority priority) 
            : base(id, title, description, priority)
        {
        }
    }

    public class DeadlineTask : Task
    {
        public DateTime Deadline { get; private set; }

        public DeadlineTask(int id, string title, string description, Priority priority) 
            : base(id, title, description, priority)
        {
            Deadline = DateTime.Now.AddDays(7); // Default to 7 days from now
        }

        public void SetDeadline(DateTime deadline)
        {
            Deadline = deadline;
            NotifyObservers($"Prazo da tarefa '{Title}' definido para {deadline.ToShortDateString()}");
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Prazo: {Deadline.ToShortDateString()}";
        }
    }

    public class RecurringTask : Task
    {
        public RecurrencePattern Pattern { get; private set; }
        public int Interval { get; private set; }

        public RecurringTask(int id, string title, string description, Priority priority) 
            : base(id, title, description, priority)
        {
            Pattern = RecurrencePattern.Weekly;
            Interval = 1;
        }

        public void SetRecurrencePattern(RecurrencePattern pattern, int interval)
        {
            Pattern = pattern;
            Interval = interval;
            NotifyObservers($"Padrão de recorrência da tarefa '{Title}' definido para {pattern} a cada {interval}");
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Recorrência: {Pattern} (a cada {Interval})";
        }
    }

    public class Attachment
    {
        public string Name { get; private set; }
        public string Path { get; private set; }

        public Attachment(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }

    public class AttachmentTask : Task
    {
        private List<Attachment> _attachments = new List<Attachment>();

        public AttachmentTask(int id, string title, string description, Priority priority) 
            : base(id, title, description, priority)
        {
        }

        public void AddAttachment(string name, string path)
        {
            _attachments.Add(new Attachment(name, path));
            NotifyObservers($"Anexo '{name}' adicionado à tarefa '{Title}'");
        }

        public List<Attachment> GetAttachments()
        {
            return _attachments.ToList();
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Anexos: {_attachments.Count}";
        }
    }

    // User (Observer)
    public class User : ITaskObserver
    {
        private List<string> _notifications = new List<string>();

        public int Id { get; }
        public string Name { get; }

        public User(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void Update(Task task, string message)
        {
            _notifications.Add($"[{DateTime.Now}] {message}");
        }

        public List<string> GetNotifications()
        {
            return _notifications.ToList();
        }

        public void ClearNotifications()
        {
            _notifications.Clear();
        }
    }

    // Project (Task Container)
    public class Project
    {
        private List<Task> _tasks = new List<Task>();

        public int Id { get; }
        public string Name { get; }

        public Project(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void AddTask(Task task)
        {
            _tasks.Add(task);
        }

        public void RemoveTask(Task task)
        {
            _tasks.Remove(task);
        }

        public List<Task> GetTasks()
        {
            return _tasks.ToList();
        }

        public List<Task> GetTasksByPriority(Priority priority)
        {
            return _tasks.Where(t => t.Priority == priority).ToList();
        }

        public List<Task> GetTasksByStatus(TaskStatus status)
        {
            return _tasks.Where(t => t.Status == status).ToList();
        }

        public Task? FindTaskById(int id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }
    }

    #endregion
}