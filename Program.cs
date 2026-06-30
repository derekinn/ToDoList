using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace ToDoList
{
    public class Program
    {
        private static List<Tarefa> tarefas = new();

        public static void Main()
        {
            LoadTasks();
            bool inExecution = true;

            while (inExecution)
            {
                DisplayMenu();

                string? option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        CreateTask();
                        break;

                    case "2":
                        ListTasks();
                        break;

                    case "3":
                        CompleteTask();
                        break;

                    case "4":
                        RemoveTask();
                        break;

                    case "5":
                        EditTask();
                        break;

                    case "6":
                        UncompleteTask();
                        break;

                    case "9":
                        inExecution = false;
                        Console.WriteLine("\nAté mais.");
                        break;

                    default:
                        Console.WriteLine("\nOpção inválida! Escolha somente as opções disponíveis.\n");
                        break;
                }
            }
        }

        public static void CreateTask()
        {
            Console.Write("\nInforme o título da tarefa: ");
            string? tituloDigitado = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(tituloDigitado))
            {
                Console.WriteLine("O título da tarefa não pode ser vazio!\n");
                return;
            }

            Tarefa novaTarefa = new Tarefa
            {
                Titulo = tituloDigitado,
                Concluida = false
            };

            tarefas.Add(novaTarefa);
            SaveTasks();

            Console.WriteLine($"Tarefa criada com sucesso!\n");
        }

        public static void ListTasks()
        {
            if (tarefas.Count == 0)
            {
                Console.WriteLine("Lista de tarefas vazia");
                return;
            }

            for (int i = 0; i < tarefas.Count; i++)
            {
                string status;
                int numero = i + 1;

                if (tarefas[i].Concluida)
                {
                    status = "[X]";
                }
                else
                {
                    status = "[ ]";
                }

                Console.WriteLine($"{numero}. {tarefas[i].Titulo} {status}");
            }
        }

        public static void CompleteTask()
        {
            if (tarefas.Count == 0)
            {
                Console.WriteLine("\nNão existem tarefas cadastradas");
                return;
            }

            Console.WriteLine("\nQual tarefa você deseja concluir? (Digite o número)");
            for (int i = 0; i < tarefas.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {tarefas[i].Titulo}"); ;
            }

            if (!int.TryParse(Console.ReadLine(), out int numeroDigitado))
            {
                Console.WriteLine("\nPor favor, informe um número válido.");
                return;
            }

            if (numeroDigitado < 1 || numeroDigitado > tarefas.Count)
            {
                Console.WriteLine("\nNúmero da tarefa inválido. Verifique e tente novamente");
                return;
            }

            int indice = numeroDigitado - 1;

            if (tarefas[indice].Concluida)
            {
                Console.WriteLine("A tarefa já está concluída");
                return;
            }

            tarefas[indice].Concluida = true;
            SaveTasks();
            Console.WriteLine("Tarefa concluída com sucesso");
        }

        public static void EditTask()
        {
            if (tarefas.Count == 0)
            {
                Console.WriteLine("Lista de tarefas vazia");
                return;
            }

            Console.WriteLine("Qual tarefa você deseja editar?");

            for (int i = 0; i < tarefas.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {tarefas[i].Titulo}");
            }

            if (!int.TryParse(Console.ReadLine(), out int numeroTarefa))
            {
                Console.WriteLine("\nDigite um número válido.");
                return;
            }

            if (numeroTarefa < 1 || numeroTarefa > tarefas.Count)
            {
                Console.WriteLine("\nNúmero inválido.");
                return;
            }

            int indice = numeroTarefa - 1;

            Console.WriteLine($"\nTítulo atual: {tarefas[indice].Titulo}");
            Console.WriteLine("Digite o novo título:");

            string? novoTitulo = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(novoTitulo))
            {
                Console.WriteLine("O título da tarefa não pode ser vazio!");
                return;
            }

            if (novoTitulo == tarefas[indice].Titulo)
            {
                Console.WriteLine("A tarefa já possui esse título.");
                return;
            }

            tarefas[indice].Titulo = novoTitulo;

            SaveTasks();
            Console.WriteLine("Tarefa alterada com sucesso!");
        }

        public static void UncompleteTask()
        {
            if (tarefas.Count == 0)
            {
                Console.WriteLine("Lista de tarefas vazia");
                return;
            }

            Console.WriteLine("Qual tarefa você deseja desmarcar?");

            for (int i = 0; i < tarefas.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {tarefas[i].Titulo}");
            }

            if (!int.TryParse(Console.ReadLine(), out int numeroTarefa))
            {
                Console.WriteLine("\nDigite um número válido.");
                return;
            }

            if (numeroTarefa < 1 || numeroTarefa > tarefas.Count)
            {
                Console.WriteLine("\nNúmero inválido.");
                return;
            }

            int indice = numeroTarefa - 1;

            if (!tarefas[indice].Concluida)
            {
                Console.WriteLine("Essa tarefa ainda não está concluída.");
                return;
            }

            Console.WriteLine("Deseja realmente desmarcar esta tarefa? (S/N)");
            string? opcao = Console.ReadLine();

            if (opcao?.ToUpper() != "S")
            {
                Console.WriteLine("Operação cancelada.");
                return;
            }

            tarefas[indice].Concluida = false;

            SaveTasks();
            Console.WriteLine("Tarefa desmarcada com sucesso!");
        }

        public static void RemoveTask()
        {
            if (tarefas.Count == 0)
            {
                Console.WriteLine("Lista de tarefas vazia");
                return;
            }

            Console.WriteLine("Qual tarefa você deseja remover? ");

            for (int i = 0; i < tarefas.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {tarefas[i].Titulo}"); ;
            }

            if (!int.TryParse(Console.ReadLine(), out int numRemover))
            {
                Console.WriteLine("\nDigite um número válido.");
                return;
            }

            if (numRemover < 1 || numRemover > tarefas.Count)
            {
                Console.WriteLine("\nNúmero inválido.");
                return;
            }

            tarefas.RemoveAt(numRemover - 1);

            Console.WriteLine("\nTarefa removida com sucesso!");
        }

        private static void SaveTasks()
        {
            string json = JsonSerializer.Serialize(tarefas, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText("tarefas.json", json);
        }

        private static void LoadTasks()
        {
            if (!File.Exists("tarefas.json"))
                return;

            string json = File.ReadAllText("tarefas.json");

            tarefas = JsonSerializer.Deserialize<List<Tarefa>>(json) ?? new List<Tarefa>();
        }

        public static void DisplayMenu()
        {
            Console.WriteLine("======================");
            Console.WriteLine("      ToDoList");
            Console.WriteLine("======================");
            Console.WriteLine("\n1. Criar tarefa");
            Console.WriteLine("2. Listar tarefas");
            Console.WriteLine("3. Concluir tarefa");
            Console.WriteLine("4. Remover tarefa");
            Console.WriteLine("5. Editar tarefa");
            Console.WriteLine("6. Desmarcar tarefa");
            Console.WriteLine("9. Sair");
            Console.Write("\nEscolha uma opção: ");
        }
    }
}