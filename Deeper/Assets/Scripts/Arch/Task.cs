using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskStatus : byte { Detached, Pending, Working, Success, Fail, Aborted }

public class Task
{
    public TaskStatus Status { get; private set; }
    public Task NextTask { get; private set; }

    public bool IsDetached { get { return Status == TaskStatus.Detached; } }
    public bool IsAttached { get { return Status != TaskStatus.Detached; } }
    public bool IsPending { get { return Status == TaskStatus.Pending; } }
    public bool IsWorking { get { return Status == TaskStatus.Working; } }
    public bool IsSuccessful { get { return Status == TaskStatus.Success; } }
    public bool IsFailed { get { return Status == TaskStatus.Fail; } }
    public bool IsAborted { get { return Status == TaskStatus.Aborted; } }
    public bool IsFinished { get { return (Status == TaskStatus.Fail || Status == TaskStatus.Success || Status == TaskStatus.Aborted); } }

    internal void SetStatus(TaskStatus newStatus)
    {
        if (Status == newStatus) return;

        Status = newStatus;

        switch (newStatus)
        {
            case TaskStatus.Working:
                Init();
                break;

            case TaskStatus.Success:
                OnSuccess();
                CleanUp();
                break;

            case TaskStatus.Aborted:
                OnAbort();
                CleanUp();
                break;

            case TaskStatus.Fail:
                OnFail();
                CleanUp();
                break;

            case TaskStatus.Detached:
            case TaskStatus.Pending:
                break;
            default:
                Debug.Log("Task error:" + Status);
                break;
        }
    }

    public virtual void Init() { }

    public virtual void TaskUpdate() { }
    public virtual void TaskDeeperEarlyUpdate() { }
    public virtual void TaskDeeperNormUpdate() { }
    public virtual void TaskDeeperPostUpdate() { }

    public virtual void CleanUp() { }

    public virtual void OnSuccess() { }

    public virtual void OnFail() { }

    public virtual void OnAbort() { }

    public virtual void Abort()
    {
        SetStatus(TaskStatus.Aborted);
    }

    public Task Then(Task task)
    {
        Debug.Assert(!task.IsAttached);
        NextTask = task;
        return task;
    }
}

public class TestTask : Task
{
    private GameObject gO;
    private TMPro.TextMeshPro myTMP;

    public override void Init()
    {
        base.Init();
        Debug.Log("Init was run");

        gO = new GameObject();
        gO.transform.position = new Vector3(0, 0, 20);
        myTMP = gO.AddComponent<TMPro.TextMeshPro>();
        myTMP.SetText("Garblk" + (int)UnityEngine.Random.Range(10, 101209));
    }

    private float timer;


    public override void TaskUpdate()
    {
        base.TaskUpdate();
        Debug.Log("TaskUpdate is running");

        timer += Time.unscaledDeltaTime;
        myTMP.color = new Color(1, 0, 0, 1 - timer);

        if (timer > 1)
            SetStatus(TaskStatus.Success);
    }

    public override void OnSuccess()
    {
        base.OnSuccess();
        Debug.Log("Success!!");
        GameObject.Destroy(gO);
    }
}

public class Task_MenuAnimation_Highlight : Task
{
    private Deeper_MenuItem context;
    private TMPro.TextMeshPro myTMP;

    public Task_MenuAnimation_Highlight (Deeper_MenuItem c)
    {
        context = c;
        myTMP = c.gameObject.GetComponent<TMPro.TextMeshPro>();
    }

    public override void TaskUpdate()
    {
        base.TaskUpdate();
        Debug.Log("Task_MenuAnimation_Highlight is running");
        myTMP.fontSize += Time.unscaledDeltaTime * 10;
        if (myTMP.fontSize >= 30)
            SetStatus(TaskStatus.Success);
    }

    public override void OnFail()
    {
        base.OnFail();
        myTMP.fontSize = 20;
    }

    public override void OnSuccess()
    {
        base.OnSuccess();
        myTMP.fontSize = 30;
    }
}