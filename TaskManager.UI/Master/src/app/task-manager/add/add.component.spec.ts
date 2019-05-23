import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { of as observableOf, Observable, throwError } from 'rxjs';
import { FormsModule }   from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { Router,ActivatedRoute} from '@angular/router';
import { TaskItem } from '../../models/task-item';
import { TaskManagerService } from '../providers/task-manager.service';
import { AddComponent } from './add.component';
import { MockTaskManagerService } from '../providers/task-manager.service.spec';

describe('AddComponent', () => {
  let component: AddComponent;
  let fixture: ComponentFixture<AddComponent>;
  let service : TaskManagerService; 
  
  const TaskDetails : any[] = [{ "id": 1, "name": "Task 1", "startDate": Date.now, 
    "endDate" :Date.now, "priority":10, 
    "endTask":false, "parentTaskId":2, "parentName":"parent" },
    { "id": 2, "name": "Task 2", "startDate": Date.now, "endDate": Date.now,
     "priority":10, "endTask":true, "parentTaskId":2, "parentName":"parent" }
  ];

  let mockRouter = {
    navigate: jasmine.createSpy('navigate')
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ FormsModule, RouterTestingModule],
      declarations: [ AddComponent ] , 
      providers: [
        {provide: TaskManagerService, useClass: MockTaskManagerService},
        { provide: Router, useValue: mockRouter}
      ]
    })    
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddComponent);
    component = fixture.componentInstance;
    service = TestBed.get(TaskManagerService);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
    //expect('Task Manager').toBe(component.title)
  });

  it('should return Task details', () => {
    spyOn(service,'getAllTasks').and.returnValues(observableOf(TaskDetails));
    component.ngOnInit();
    expect(1).toBe(component.taskDetails.length);
    expect("Task 1").toBe(component.taskDetails[0].name);
  });

  it('Add should return Success', () => {
    component.taskDetail = new TaskItem();
    spyOn(service,'addTask').and.returnValue(observableOf("SUCCESS"));
    component.onAddTask();
    //expect(component.openModal).toHaveBeenCalled();
    expect("SUCCESS").toBe(component.results);
    expect(service.addTask).toHaveBeenCalled();    
  });

  it('Add should return Internal server error', () =>
  {
    component.taskDetail = new TaskItem();   
    var error = { status: 500, _body :'"Internal server error"'};
    spyOn(service , 'addTask').and.callFake(() => {
      return throwError(error);
    });
    component.onAddTask();     
    expect("Internal server error").toBe(component.results);
    expect(service.addTask).toHaveBeenCalled();    
  });

  it('Resetting Task Detail', () =>
  {
    var taskDetail = new TaskItem() ;   
    component.taskDetail = taskDetail;
    console.log(component.taskDetail.name);
    taskDetail.name ="task 1";
    taskDetail.id = 1;
    taskDetail.priority = 10;
    
    component.onResetTask();           
    expect(0).toBe(component.taskDetail.priority);
    expect(component.taskDetail.id).toBeUndefined();
    expect(component.taskDetail.name).toBeUndefined();   
  })

  it('onclose modal should go to view', () =>
  {
    component.closeModal();     
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/view']);
  })
});