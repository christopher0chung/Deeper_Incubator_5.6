using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deeper_TaskManager : Deeper_Component
{
    [SerializeField] private List<Task> TasksList = new List<Task>();

    void Start()
    {
        Initialize(5000);
    }

    void Update()
    {
        for (int i = TasksList.Count - 1; i >= 0; --i)
        {
            Task task = TasksList[i];

            if (task.IsPending)
            {
                task.SetStatus(TaskStatus.Working);
            }

            task.TaskUpdate();

            if (task.IsFinished)
            {
                HandleCompletion(task, i);
            }
        }
    }
    public override void EarlyUpdate()
    {
        for (int i = TasksList.Count - 1; i >= 0; --i)
        {
            Task task = TasksList[i];

            if (task.IsPending)
            {
                task.SetStatus(TaskStatus.Working);
            }

            task.TaskDeeperEarlyUpdate();

            if (task.IsFinished)
            {
                HandleCompletion(task, i);
            }
        }
    }
    public override void NormUpdate()
    {
        for (int i = TasksList.Count - 1; i >= 0; --i)
        {
            Task task = TasksList[i];

            if (task.IsPending)
            {
                task.SetStatus(TaskStatus.Working);
            }

            task.TaskDeeperNormUpdate();

            if (task.IsFinished)
            {
                HandleCompletion(task, i);
            }
        }
    }
    public override void PostUpdate()
    {
        for (int i = TasksList.Count - 1; i >= 0; --i)
        {
            Task task = TasksList[i];

            if (task.IsPending)
            {
                task.SetStatus(TaskStatus.Working);
            }

            task.TaskDeeperPostUpdate();

            if (task.IsFinished)
            {
                HandleCompletion(task, i);
            }
        }
    }

    public void AddTask(Task myTask)
    {
        Debug.Assert(myTask != null);
        Debug.Assert(!myTask.IsAttached);
        TasksList.Add(myTask);
        myTask.SetStatus(TaskStatus.Pending);
    }

    public void AbortTask(Task myTask)
    {
        foreach (Task t in TasksList)
        {
            if (t == myTask)
                t.SetStatus(TaskStatus.Aborted);
        }
    }

    private void HandleCompletion(Task task, int taskIndex)
    {
        if (task.NextTask != null && task.IsSuccessful)
        {
            AddTask(task.NextTask);
        }
        TasksList.RemoveAt(taskIndex);

        task.SetStatus(TaskStatus.Detached);
    }
}

