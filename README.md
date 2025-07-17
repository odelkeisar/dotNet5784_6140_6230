# Restaurant Management System

A desktop system for managing staff, tasks, and operations in a restaurant.

## Overview

This WPF based desktop application enables restaurant managers to efficiently assign, track, and manage tasks and staff while enforcing task dependencies to ensure smooth workflow.  
Each chef can view their assigned tasks and update completion status. The manager can create, edit, or delete tasks and chefs, and visualize task timelines using a dynamic Gantt chart.

## Key Features

- Role-based access: Manager and Chefs  
- Add / update / delete chefs and tasks  
- Assign tasks to specific chefs  
- Mark tasks as completed (per chef)  
- View task timelines via interactive Gantt chart  
- XML-based structured data storage  
- Clean, user-friendly WPF interface
  
- Enforce task dependencies:
   - A task cannot be assigned or started unless all the tasks it depends on are completed.
   - A task cannot be deleted if other tasks depend on it, unless dependent tasks are deleted first

## Technologies

- Language: C#  
- Framework: .NET (WPF)  
- GUI: XAML  
- Data Storage: XML   
- IDE: Visual Studio

## How to Run

1. Clone the repository  
2. Open the solution in Visual Studio  
3. Build and run (F5)

## Screenshots 

Main Window-

<img width="598" height="486" alt="image" src="https://github.com/user-attachments/assets/ba53b1e5-6e06-4feb-8d4b-5c4cf7ac94a6" />

Manager Window-

<img width="600" height="485" alt="image" src="https://github.com/user-attachments/assets/e8685d53-c47f-458a-9594-b293d72e2d41" />

Chef List Window

<img width="975" height="777" alt="image" src="https://github.com/user-attachments/assets/891a0a42-658b-4045-9c39-a598dce62500" />


## Author 

Developed by Odel Keisar and Pnini Walles 
