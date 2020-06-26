﻿using System;
using System.Collections.Generic;

namespace GameFlow
{
    public class TaskCreator
    {
        private List<System.Func<Task>> funcs = new List<Func<Task>>();

        public void Add(System.Func<Task> creator)
        {
            funcs.Add(creator);
        }

        public void Remove(System.Func<Task> creator)
        {
            funcs.Remove(creator);
        }

        public TaskSet Invoke()
        {
            TaskSet set = new TaskSet();
            foreach (var item in funcs)
                set.Add(item.Invoke());
            return set;
        }

        public static TaskCreator operator +(TaskCreator creator, System.Func<Task> func)
        {
            creator.Add(func);
            return creator;
        }

        public static TaskCreator operator -(TaskCreator creator, System.Func<Task> func)
        {
            creator.Remove(func);
            return creator;
        }
    }
}