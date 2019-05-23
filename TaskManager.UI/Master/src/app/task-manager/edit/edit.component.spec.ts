import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule }   from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { Router,ActivatedRoute} from '@angular/router';
import { EditComponent } from './edit.component';
import { TaskManagerService } from '../providers/task-manager.service';
import { MockTaskManagerService } from '../providers/task-manager.service.spec';
import { from, of as observableOf, throwError } from 'rxjs';
import { TaskItem } from '../../models/task-item';

describe('EditComponent', () => {
  let component: EditComponent;
  let fixture: ComponentFixture<EditComponent>;
  let service : TaskManagerService; 
  let mockRouter = {
    navigate: jasmine.createSpy('navigate')
  };
  const TaskDetails : any[] = [
    { "id": 101, "name": "Task 101", "startDate": Date.now,"endDate" :Date.now, "priority":10,"endTask":false, "parentTaskId":2, "parentName":"parent" },
    { "id": 102, "name": "Task 102", "startDate": Date.now, "endDate" :Date.now, "priority":10, "endTask":true, "parentTaskId":2, "parentName":"parent" },
    { "id": 103, "name": "Task 103", "startDate": Date.now, "endDate" :Date.now, "priority":10, "endTask":false, "parentTaskId":2, "parentName":"parent" },
    { "id": 104, "name": "Task 104", "startDate": Date.now, "endDate" :Date.now, "priority":10, "endTask":false, "parentTaskId":2, "parentName":"parent" }
    ];

    const TaskDetail : any =  { "id": 101, "name": "Task 101", "startDate": Date.now,"endDate" :Date.now,
     "priority":10,"endTask":false, "parentTaskId":103, "parentName":"Task 103" };

  //let params: Subject<Params>;
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ FormsModule, RouterTestingModule],
      declarations: [ EditComponent ],
      providers: [
        {provide: TaskManagerService, useClass: MockTaskManagerService},
        { provide: ActivatedRoute, useValue: { 'queryParams': from([{ 'id': 101 }]) } },
        { provide: Router, useValue: mockRouter}
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditComponent);
    component = fixture.componentInstance;
    service = TestBed.get(TaskManagerService);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('Should get correct id from query string', () => {
    spyOn(service,'getAllTasks').and.returnValues(observableOf(TaskDetails));
    spyOn(service,'getTask').and.returnValues(observableOf(TaskDetail));
    component.ngOnInit();
    expect(101).toBe(component.updateTaskId);  
  });

  it('Should have only 2 task details', () => {
    spyOn(service,'getAllTasks').and.returnValues(observableOf(TaskDetails));
    spyOn(service,'getTask').and.returnValues(observableOf(TaskDetail));
    component.ngOnInit();
    expect(2).toBe(component.taskDetails.length);  
  });

  it('Should retrieve correct task details', () => {
    spyOn(service,'getAllTasks').and.returnValues(observableOf(TaskDetails));
    spyOn(service,'getTask').and.returnValues(observableOf(TaskDetail));
    component.ngOnInit();
    expect(service.getTask).toHaveBeenCalled();   
    expect(service.getAllTasks).toHaveBeenCalled();   
    expect("Task 101").toBe(component.taskDetail.name);  
    expect(10).toBe(component.taskDetail.priority);  
    expect("Task 103").toBe(component.taskDetail.parentName);  
    expect(103).toBe(component.taskDetail.parentTaskId);  
    expect(false).toBe(component.showError);
  });

  it('Should handle status code 400 failure', () => {
    spyOn(service,'getAllTasks').and.returnValues(observableOf(TaskDetails));    
    var error = { status: 400, _body :'"Bad Request"'};
    spyOn(service,'getTask').and.returnValue(throwError(error));
    component.ngOnInit();
    expect("Bad Request").toBe(component.results);
    expect(true).toBe(component.showError);
    expect(service.getTask).toHaveBeenCalled();    
    expect(service.getAllTasks).toHaveBeenCalled();   
  });

  it('Update should return Success', () =>
  {
    var taskDetail = new TaskItem();
    taskDetail.id= 101;
    component.taskDetail = taskDetail;
    spyOn(service,'updateTask').and.returnValue(observableOf("SUCCESS"));
    component.onUpdateTask();   
    expect("SUCCESS").toBe(component.results);
    expect(service.updateTask).toHaveBeenCalledWith(taskDetail, 101);    
  });

  it('Update should handle Internal server error', () =>
  {
    var taskDetail = new TaskItem();
    taskDetail.id= 101;
    component.taskDetail = taskDetail;
    var error = { status: 500, _body :'"Internal server error"'};
    spyOn(service,'updateTask').and.returnValue(throwError(error));
    component.onUpdateTask();     
    expect("Internal server error").toBe(component.results);
    expect(service.updateTask).toHaveBeenCalledWith(taskDetail, 101); 
  });

  it('Update should handle Bad Request', () =>
  {
    var taskDetail = new TaskItem();
    taskDetail.id= 101;
    component.taskDetail = taskDetail;
    var error = { status: 400, _body :'"Bad Request"'};
    spyOn(service,'updateTask').and.returnValue(throwError(error));
    component.onUpdateTask();     
    expect("Bad Request").toBe(component.results);
    expect(service.updateTask).toHaveBeenCalledWith(taskDetail, 101); 
  });

  it('onCancel should go to view', () => {
    component.onCancel();     
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/view']);
  })

  it('onclose modal should go to view', () => {
    component.closeModal();     
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/view']);
  })
});