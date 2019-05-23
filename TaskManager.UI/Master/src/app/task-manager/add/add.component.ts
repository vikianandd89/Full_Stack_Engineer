import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { Router} from '@angular/router';
import { TaskItem } from '../../models/task-item';
import { TaskManagerService } from '../providers/task-manager.service';

@Component({
  templateUrl: './add.component.html'
})
export class AddComponent implements OnInit {

  @ViewChild('showmodal') showmodal:ElementRef;
  taskDetails:TaskItem[]
  public taskDetail:TaskItem;
  results:string

  constructor(
    private taskManagerService: TaskManagerService,
    private router: Router
  ) { 
   this.taskDetail = new TaskItem();
   this.taskDetail.priority = 0;
  }

  ngOnInit() {
    this.taskManagerService.getAllTasks().subscribe(
      p=>this.taskDetails=p.filter(res => !res.endTask));
    }

    onAddTask()
    {
      this.taskManagerService.addTask(this.taskDetail).subscribe(response => 
        {
          this.results = response;
          this.openModal();
        },
        error =>
        {
          if(error.status < 200 || error.status > 300)
            this.results = JSON.parse(error._body);
            this.openModal();
        }
      );
    }

    onResetTask()
    {
      this.taskDetail = new TaskItem();
      this.taskDetail.priority = 0;
    }

    openModal() {
      this.showmodal.nativeElement.click();
    }
    closeModal() {
      this.router.navigate(['/view']);
    }
}