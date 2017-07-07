using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskStatus : byte { Detached, Pending, Working, Success, Fail, Aborted }

public class Task
{
    public TaskStatus Status { get; private set; }
    public Task NextTask { get; private set; }

    public TaskCanBeInterrupted CanBeInterrupted;
    public TaskDoesInterrupt DoesInterrupt;

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

//-------------------------------------------------
// Menu Tasks
//-------------------------------------------------

public class Task_MenuTasks: Task
{
    public Deeper_MenuItem context;
    public TMPro.TextMeshPro myTMP;

    public float scaleSpeed = 120;
    public float sizeIdle = 20;
    public float sizeHighlight = 22;

    public Color colorVis = new Color(1, 1, 1, 1);
    public Color colorInvis = new Color(1, 1, 1, 0);
    public float colorNormalizedTime = .15f;
}

public class Task_MenuAnimation_Highlight : Task_MenuTasks
{
    public Task_MenuAnimation_Highlight (Deeper_MenuItem c)
    {
        context = c;
        myTMP = c.itemTMP;
        CanBeInterrupted = TaskCanBeInterrupted.Yes;
        DoesInterrupt = TaskDoesInterrupt.No;
    }

    public Task_MenuAnimation_Highlight(Deeper_MenuItem c, TaskCanBeInterrupted cI, TaskDoesInterrupt dI)
    {
        context = c;
        myTMP = c.itemTMP;
        CanBeInterrupted = cI;
        DoesInterrupt = dI;
    }

    public override void Init()
    {
        myTMP.fontSize = sizeIdle;
    }

    public override void TaskUpdate()
    {
        base.TaskUpdate();
        Debug.Log("Task_MenuAnimation_Highlight is running");
        myTMP.fontSize += Time.unscaledDeltaTime * scaleSpeed;
        if (myTMP.fontSize >= sizeHighlight)
            SetStatus(TaskStatus.Success);
    }

    public override void OnFail()
    {
        base.OnFail();
        myTMP.fontSize = sizeIdle;
    }

    public override void OnAbort()
    {
        OnFail();
    }

    public override void OnSuccess()
    {
        base.OnSuccess();
        myTMP.fontSize = sizeHighlight;
    }
}

public class Task_MenuAnimation_Unhighlight : Task_MenuTasks
{
    public Task_MenuAnimation_Unhighlight(Deeper_MenuItem c)
    {
        context = c;
        myTMP = c.itemTMP;
        CanBeInterrupted = TaskCanBeInterrupted.Yes;
        DoesInterrupt = TaskDoesInterrupt.No;
    }

    public Task_MenuAnimation_Unhighlight(Deeper_MenuItem c, TaskCanBeInterrupted cI, TaskDoesInterrupt dI)
    {
        context = c;
        myTMP = c.itemTMP;
        CanBeInterrupted = cI;
        DoesInterrupt = dI;
    }

    public override void Init()
    {
        myTMP.fontSize = sizeHighlight;
    }

    public override void TaskUpdate()
    {
        base.TaskUpdate();
        Debug.Log("Task_MenuAnimation_Unhighlight is running");
        myTMP.fontSize -= Time.unscaledDeltaTime * scaleSpeed;
        if (myTMP.fontSize <= sizeIdle)
            SetStatus(TaskStatus.Success);
    }

    public override void OnFail()
    {
        base.OnFail();
        myTMP.fontSize = sizeHighlight;
    }

    public override void OnAbort()
    {
        OnFail();
    }

    public override void OnSuccess()
    {
        base.OnSuccess();
        myTMP.fontSize = sizeIdle;
    }
}

public class Task_MenuAnimation_Visible : Task_MenuTasks
{
    public Task_MenuAnimation_Visible(Deeper_MenuItem c)
    {
        context = c;
        myTMP = c.itemTMP;
        CanBeInterrupted = TaskCanBeInterrupted.Yes;
        DoesInterrupt = TaskDoesInterrupt.No;
    }

    public Task_MenuAnimation_Visible(Deeper_MenuItem c, TaskCanBeInterrupted cI, TaskDoesInterrupt dI)
    {
        context = c;
        myTMP = c.itemTMP;
        CanBeInterrupted = cI;
        DoesInterrupt = dI;
    }

    private float timer;

    public override void Init()
    {
        myTMP.color = colorInvis;
        timer = 0;
    }

    public override void TaskUpdate()
    {
        base.TaskUpdate();
        Debug.Log("Task_MenuAnimation_Visible is running");
        timer += Time.unscaledDeltaTime;
        myTMP.color = Color.Lerp(colorInvis, colorVis, timer / colorNormalizedTime);
        if (timer / colorNormalizedTime >= 1)
            SetStatus(TaskStatus.Success);
    }

    public override void OnFail()
    {
        base.OnFail();
        myTMP.color = colorInvis;
    }

    public override void OnAbort()
    {
        OnFail();
    }

    public override void OnSuccess()
    {
        base.OnSuccess();
        myTMP.color = colorVis;
    }
}

public class Task_MenuAnimation_Invisible : Task_MenuTasks
{
    public Task_MenuAnimation_Invisible(Deeper_MenuItem c)
    {
        context = c;
        myTMP = c.itemTMP;
        CanBeInterrupted = TaskCanBeInterrupted.Yes;
        DoesInterrupt = TaskDoesInterrupt.No;
    }

    public Task_MenuAnimation_Invisible(Deeper_MenuItem c, TaskCanBeInterrupted cI, TaskDoesInterrupt dI)
    {
        context = c;
        myTMP = c.itemTMP;
        CanBeInterrupted = cI;
        DoesInterrupt = dI;
    }

    private float timer;

    public override void Init()
    {
        myTMP.color = colorVis;
        timer = 0;
    }

    public override void TaskUpdate()
    {
        base.TaskUpdate();
        Debug.Log("Task_MenuAnimation_Visible is running");
        timer += Time.unscaledDeltaTime;
        myTMP.color = Color.Lerp(colorVis, colorInvis, timer / colorNormalizedTime);
        if (timer / colorNormalizedTime >= 1)
            SetStatus(TaskStatus.Success);
    }

    public override void OnFail()
    {
        base.OnFail();
        myTMP.color = colorVis;
    }

    public override void OnAbort()
    {
        OnFail();
    }

    public override void OnSuccess()
    {
        base.OnSuccess();
        myTMP.color = colorInvis;
    }
}