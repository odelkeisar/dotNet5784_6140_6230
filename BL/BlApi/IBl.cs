﻿namespace BlApi;
public interface IBl
{
    public ITask1 Task1 { get; }
    public IChef Chef { get; }
    public void InitializeDB();
    public void InitializeResetB();
}

