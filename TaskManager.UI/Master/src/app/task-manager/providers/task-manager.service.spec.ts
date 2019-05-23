import { Injectable } from '@angular/core';
import { Observable, of as ObservableOf } from 'rxjs';
import { Http, Response } from '@angular/http';
import { TaskItem } from '../../models/task-item';
const TaskDetails : any[] = [
    { "id": 101, "name": "Task 101", "startDate": "2018-07-23",
        "endDate" :"2018-07-28", "priority":10,"endTask":false,
        "parentTaskId":2, "parentName":"parent" },
    ];
const TaskDetail: TaskItem = new TaskItem();

export class MockTaskManagerService {
    
    getAllTasks(): Observable<TaskItem[]> {
        return ObservableOf(TaskDetails);
    }

    getTask(): Observable<TaskItem> {
        return ObservableOf(TaskDetail);
    }

    addTask(Item: TaskItem): Observable<string> {
        return ObservableOf("Success");
    }

    updateTask(Item: TaskItem, Id:number): Observable<string> {
        return ObservableOf("Success");
    }
}