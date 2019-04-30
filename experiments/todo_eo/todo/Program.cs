using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using eo.pipeline;
using eventstore;
using todo.messages.commands.createlist;
using todo.messages.commands.createtodo;
using todo.messages.queries.lists;
using todo.messages.queries.tasks;
using todo.messages.queries.tasksinlist;

namespace todo
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (Directory.Exists("eventstore")) Directory.Delete("eventstore", true);
            
            var es = new EventStore("eventstore");
            var mh = new MessageHandling(es);
            
            mh.Register<CreateListCommand>(new CreateListContextManagement(es), new CreateListProcessor());
            mh.Register<CreateTodoCommand>(new CreateTodoContextManagement(es), new CreateTodoProcessor());
            mh.Register<ListsQuery>(new ListsContextManagement(es), new ListsProcessor());
            mh.Register<TasksQuery>(new TasksContextManagement(es), new TasksProcessor());
            mh.Register<TasksInListQuery>(new TasksInListContextManagement(es), new TasksInListProcessor());
            
            
            // Use Case
            mh.Handle(new CreateListCommand{Name = "Heute"});

            var result = mh.Handle(new ListsQuery());
            var listId = ((ListsQueryResult) result).Lists.First().Id;
            
            mh.Handle(new CreateTodoCommand{Subject = "Aufräumen", ListId = listId});
            mh.Handle(new CreateTodoCommand{Subject = "Einkaufen", ListId = listId});
            
            result = mh.Handle(new TasksInListQuery{ListId = listId});
            foreach(var t in ((TasksInListQueryResult)result).Tasks)
                Console.WriteLine($"{t.Id}: {t.Subject}");
            
            
            mh.Handle(new CreateListCommand{Name = "Einkaufsliste"});
            result = mh.Handle(new ListsQuery());
            listId = ((ListsQueryResult) result).Lists.First(l => l.Name == "Einkaufsliste").Id;

            mh.Handle(new CreateTodoCommand {Subject = "Kartoffeln", ListId = listId});
            mh.Handle(new CreateTodoCommand {Subject = "Zwiebeln", ListId = listId});
            mh.Handle(new CreateTodoCommand {Subject = "Käse", ListId = listId});
            
            result = mh.Handle(new TasksInListQuery{ListId = listId});
            foreach(var t in ((TasksInListQueryResult)result).Tasks)
                Console.WriteLine($"{t.Id}: {t.Subject}");
        }
    }
}