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

    private List<Task> examinedTasks = new List<Task>();

    public void AddTask(Task myTask)
    {
        //Debug.Log("Adding a task");
        Debug.Assert(myTask != null);
        Debug.Assert(!myTask.IsAttached);

        examinedTasks.Clear();
        // If it does interrupt, look for and hold every other task that it shares a context with.
        // Each item in that list, if it can be interrupted, then interrupt it.
        if (myTask.DoesInterrupt == TaskDoesInterrupt.Yes)
        {
            //Debug.Log("The new taks does interrupt");

            Task_MenuTasks mOfMyTask = myTask as Task_MenuTasks;
            if (mOfMyTask != null)
            {
                //Debug.Log("New task is a menuTask");

                foreach (Task t in TasksList)
                {
                    //Debug.Log("Going through TasksList");
                    Task_MenuTasks mInTaskList = t as Task_MenuTasks;
                    if (mInTaskList != null)
                    {
                        //Debug.Log("Found a menu task");
                        if (mOfMyTask.context == mInTaskList.context)
                        {
                            examinedTasks.Add(mInTaskList);
                            //Debug.Log("Added it to the eamedTasks list.");
                        }
                    }
                }

                if (examinedTasks.Count >= 1)
                {
                    //Debug.Log("There are " + examinedTasks.Count + " tasks that share the same context");
                    foreach (Task t in TasksList)
                    {
                        Task_MenuTasks mInExamined = (Task_MenuTasks)t;
                        if (mInExamined.CanBeInterrupted == TaskCanBeInterrupted.Yes)
                        {
                            mInExamined.SetStatus(TaskStatus.Aborted);
                        }
                    }
                }
            }
        }
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

public enum TaskCanBeInterrupted { Yes, No}
public enum TaskDoesInterrupt { Yes, No}

